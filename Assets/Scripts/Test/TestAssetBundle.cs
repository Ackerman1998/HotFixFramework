using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestAssetBundle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Load());
    }
    IEnumerator Load() {
        string path = Path.Combine(Application.streamingAssetsPath, "AssetbundleTest", "AssetBundles");
        WWW www = new WWW(path);
        yield return www;
        if (www.isDone)
        {
            AssetBundle ab = www.assetBundle;
            AssetBundleManifest assetBundleManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            Debug.Log(assetBundleManifest.GetAllAssetBundles().Length);
            foreach (string str in assetBundleManifest.GetAllAssetBundles()) {
                print(str+"**********************************************");
                foreach (string depency in assetBundleManifest.GetAllDependencies(str)) {
                    print(str+" Depency:" + depency);
                }
            }
        }
        else {
            Debug.LogError(www.error);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
