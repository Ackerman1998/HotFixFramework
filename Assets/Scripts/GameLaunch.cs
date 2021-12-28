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
    private string fontName = "font";
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
        yield return AssetBundleManager.Instance.Initialize();
        Debug.Log(string.Format("AssetBundleManager Initialize use {0}ms", (DateTime.Now - start).Milliseconds));
        start = DateTime.Now;

        yield return InitFont();
        Debug.Log(string.Format("Init Font use {0}ms", (DateTime.Now - start).Milliseconds));
        start = DateTime.Now;
        yield return InitLaunchUI();
        Debug.Log(string.Format("InitLaunchUI use {0}ms", (DateTime.Now - start).Milliseconds));
        start = DateTime.Now;

        XLuaManager.Instance.Init();
        string luaAssetbundleName = XLuaManager.Instance.LuaAssetBundleName;
        AssetBundleManager.Instance.SetAssetBundleResident(luaAssetbundleName, true);
        var loader = AssetBundleManager.Instance.LoadAssetBundleAsync(luaAssetbundleName);
        yield return loader;
        loader.Dispose();
        Debug.Log(string.Format("LUAManager Initialize use {0}ms", (DateTime.Now - start).Milliseconds));
        start = DateTime.Now;
        XLuaManager.Instance.StartLoad();
     
        yield return InitNoticeTipUI();
        yield return new WaitForSeconds(0.2f);
        Debug.Log(string.Format("InitNoticeTipUI use {0}ms", (DateTime.Now - start).Milliseconds));
        if (bundleUpdater != null)
        {
            AssetBundleUpdater.Instance.CheckUpdate();
        }
        yield break;
    }
    [System.Obsolete]
    private void OldLoad() {
        //Manifest manifest = AssetBundleManager.Instance.GetAssetBundleManifest;
        //foreach (string assetbundle in manifest.GetAllAssetBundleNames()) {
        //    start = DateTime.Now;
        //    var request = AssetBundleManager.Instance.RequestAssetBundleAsync(assetbundle, true);
        //    yield return request;
        //    Debug.Log(string.Format("Load AssetBundle {0} use {1}ms", assetbundle, (DateTime.Now - start).Milliseconds));
        //    request.Dispose();
        //}
        //XLuaManager.Instance.Init();
        //XLuaManager.Instance.StartHotFix();暂不开启热修复
        //Debug.Log(string.Format("XLuaManager init use {0}ms", (DateTime.Now - start).Milliseconds));
    }
    //init font
    private IEnumerator InitFont() {
        var loader = AssetBundleManager.Instance.LoadAssetBundleAsync(fontName);
        yield return loader;
        loader.Dispose();
        yield break;
    }
    //load launch ui
    private IEnumerator InitLaunchUI() {
        var loader = AssetBundleManager.Instance.LoadAssetAsync(launchUIName);
        yield return loader;
        GameObject ui = loader.asset as GameObject;
        loader.Dispose();
        GameObject obj = InstantiateUI(ui);
        //var ui = AssetBundleManager.Instance.LoadAssets<GameObject>(launchUIName);
        //GameObject obj = InstantiateUI(ui);
        if (obj.GetComponent<AssetBundleUpdater>() == null)
        {
            bundleUpdater = obj.AddComponent<AssetBundleUpdater>();
        }
        else
        {
            bundleUpdater = obj.GetComponent<AssetBundleUpdater>();
        }
        yield break;
    }
    private IEnumerator InitNoticeTipUI() {
        var loader = AssetBundleManager.Instance.LoadAssetAsync(noticeTipsUIName);
        yield return loader;
        GameObject ui = loader.asset as GameObject;
        loader.Dispose();
        //var ui = AssetBundleManager.Instance.LoadAssets<GameObject>(noticeTipsUIName);
        GameObject obj = InstantiateUI(ui);
        obj.AddComponent<UINoticeTip>();
        yield break;
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
