using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

public class RequestTools : MonoSingleton<RequestTools>
{
    public override void Awake()
    {
        base.Awake();
    }
    public void LoadAsset<T>(string assetPath,Action<T> callback)where T : Object{
        StartCoroutine(Load<T>(assetPath, callback));
    }
    IEnumerator Load<T>(string assetPath, Action<T> callback) where T : Object
    {
        assetPath = assetPath.CheckEnvironmentPath();
        UnityWebRequest request = UnityWebRequest.Get(assetPath);
        yield return request.SendWebRequest();
        if (!request.isHttpError && request.error == null)
        {
            byte [] buffer = request.downloadHandler.data;
            switch (typeof(T).ToString()) {
                case "UnityEngine.AssetBundle":
                    AssetBundle assetBundle = AssetBundle.LoadFromMemory(buffer);
                    callback?.Invoke(assetBundle as T);
                    break;
                case "string":
                 
                    break;
            }
        }
        else {
            Debug.LogError("Load Asset Failed : "+ request.error);
            yield break;
        }
    }
}
