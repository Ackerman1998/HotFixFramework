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
    private string noticeTipsUIName = "UINoticeTip";
    //更新器
    private AssetBundleUpdater bundleUpdater=null;
    public override void Awake()
    {
        base.Awake();
    }
    IEnumerator Start()
    {
        var start = DateTime.Now;
        //yield return InitAppVersion();
        //Debug.Log(string.Format("InitAppVersion use {0}ms", (DateTime.Now - start).Milliseconds));
        //start = DateTime.Now;
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
        XLuaManager.Instance.Init();
        //XLuaManager.Instance.StartHotFix();暂不开启热修复
        Debug.Log(string.Format("XLuaManager init use {0}ms", (DateTime.Now - start).Milliseconds));
        InitLaunchUI();
        InitNoticeTipUI();
        if (bundleUpdater!=null) {
            AssetBundleUpdater.Instance.CheckUpdate();
        }
       
        yield break;
    }

    private void InitLaunchUI() {
        var ui = AssetBundleManager.Instance.LoadAssets<GameObject>(launchUIName);
        GameObject obj = InstantiateUI(ui);
        if (obj.GetComponent<AssetBundleUpdater>() == null)
        {
            bundleUpdater = obj.AddComponent<AssetBundleUpdater>();
        }
        else
        {
            bundleUpdater = obj.GetComponent<AssetBundleUpdater>();
        }
    }
    private void InitNoticeTipUI() {
        var ui = AssetBundleManager.Instance.LoadAssets<GameObject>(noticeTipsUIName);
        GameObject obj = InstantiateUI(ui);
        obj.AddComponent<UINoticeTip>();
    }
    public GameObject InstantiateUI(GameObject prefab_ui) {
        GameObject obj = Instantiate(prefab_ui);
        var launchLayer = GameObject.Find("UIRoot/LaunchLayer");
        obj.transform.SetParent(launchLayer.transform);
       
        var rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.localPosition = Vector3.zero;
        return obj;
    }
}
