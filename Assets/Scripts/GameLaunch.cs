using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Game Launch Manager
/// </summary>
public class GameLaunch : MonoSingleton<GameLaunch>
{
    private string appversionPath = "app_version.bytes";
    private string launchUIName = "UILaunch";
    public override void Awake()
    {
        base.Awake();
    }
    IEnumerator Start()
    {
        var start = DateTime.Now;
        yield return InitAppVersion();
        Debug.Log(string.Format("InitAppVersion use {0}ms", (DateTime.Now - start).Milliseconds));
        start = DateTime.Now;
        yield return AssetBundleManager.Instance.Initialize();
        Debug.Log(string.Format("AssetBundleManager Initialize use {0}ms", (DateTime.Now - start).Milliseconds));
        Manifest manifest = AssetBundleManager.Instance.GetAssetBundleManifest;
        foreach (string assetbundle in manifest.GetAllAssetBundleNames()) {
            start = DateTime.Now;
            var request = AssetBundleManager.Instance.RequestAssetBundleAsync(assetbundle, true);
            yield return request;
            Debug.Log(string.Format("Load AssetBundle {0} use {1}ms", assetbundle, (DateTime.Now - start).Milliseconds));
            request.Dispose();
        }
        XLuaManager.Instance.StartRun();
        //XLuaManager.Instance.StartHotFix();暂不开启热修复
        Debug.Log(string.Format("XLuaManager init use {0}ms", (DateTime.Now - start).Milliseconds));
        InitLaunchUI();
    }

    IEnumerator InitAppVersion() {
        var creater = AssetBundleManager.Instance.RequestAssetFileAsync(appversionPath);
        yield return creater;//当creater.MoveNext()等于true时会挂起这里的代码，等于false时继续执行后面的代码
        string currentVersionCode = creater.text;
        //string lastedVersionCode = creater.text;
        creater.Dispose();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        //get version code for server
#endif
        //check version code
        //if (!currentVersionCode.CheckIsNewVersion(lastedVersionCode)) { 

        //}
        print(creater.url + ",,," + creater.MoveNext());
        yield break;//相当于return，直接结束当前方法
    }
    private void InitLaunchUI() {
        var ui = AssetBundleManager.Instance.LoadAssets<GameObject>(launchUIName);
        InstantiateUI(ui);
    }
    public void InstantiateUI(GameObject prefab_ui) {
        GameObject obj = Instantiate(prefab_ui);
        var launchLayer = GameObject.Find("UIRoot/LaunchLayer");
        obj.transform.SetParent(launchLayer.transform);
        var rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.localPosition = Vector3.zero;
    }
}
