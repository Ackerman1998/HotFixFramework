using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using XLua;

public class AssetBundleUpdater : MonoSingleton<AssetBundleUpdater>
{
    private string sliderPath = "GameSlider";
    private string loadTextPath = "LoadText";
    private string appversionPath = "app_version.bytes";
    private string resversionPath = "res_version.bytes";
    private bool needUpdateRes = false;
    private string assetbundleRootPath = "AssetBundle";
    #region Version Code
    //客户端 Appversion
    private string clientAppVersion = null;
    private string clientResVersion = null;
    //远端 Appversion
    private string serverAppVersion = null;
    private string serverResVersion = null;
    #endregion
    public override void Awake()
    {
        base.Awake();
        InitUpdater();
        
    }
    private void InitUpdater() {
        transform.FindAll(sliderPath).GetComponent<Slider>().value=0; 
        transform.FindAll(loadTextPath).GetComponent<Text>().text= "Detecting resources...."; 
    }
    public void CheckUpdate() {
        StartCoroutine(Check());
    }

    
    IEnumerator Check() {
#if UNITY_EDITOR
        if (EditorConfig.SelectMode==0) { //Editor Mode
            yield return StartGame();
            yield break;
        }
#endif
        bool isEditor = false;
#if UNITY_EDITOR
        isEditor = true;
#endif
        var start = DateTime.Now;
        yield return CheckAppVersion(isEditor);
        Debug.Log(string.Format("Init AppVersion use {0}ms", (DateTime.Now - start).Milliseconds));
        yield return CheckResVersion(isEditor);
        yield return StartGame();
    }
    IEnumerator CheckAppVersion(bool isEditor)
    {
        if (isEditor)
        {
            var creater = AssetBundleManager.Instance.RequestAssetFileAsync(appversionPath);
            yield return creater;//当creater.MoveNext()等于true时会挂起这里的代码，等于false时继续执行后面的代码
            string currentVersionCode = creater.text;
            string lastedVersionCode = creater.text;//编辑器环境下，不做appversion的更新
            clientAppVersion = currentVersionCode;
            creater.Dispose();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        //get version code for server
#endif

#if UNITY_EDITOR
            serverAppVersion = lastedVersionCode;
#endif
            if (currentVersionCode.CheckIsNewVersion(lastedVersionCode))
            {
                //noticetips redownload game
                UINoticeTip.Instance.ShowTips(TipsType.AppUpdate, DownLoadApp, Cancel, "App Version is Oldest,Please Update App...");
            }
        }
        else {
            //Real Mode Get
        }
        yield break;//相当于return，直接结束当前方法
    }
    IEnumerator CheckResVersion(bool isEditor)
    {
        if (isEditor)//Editor下获取Res
        {
            var creater = AssetBundleManager.Instance.RequestAssetFileAsync(resversionPath);
            yield return creater;
            string currentVersionCode = creater.text;
        
            creater.Dispose();
            string directory = Path.Combine(System.IO.Directory.GetParent(Application.dataPath).ToString(), assetbundleRootPath);
            var createrServer = AssetBundleManager.Instance.RequestAssetFileAsync(Path.Combine(directory,resversionPath));
            yield return createrServer;
            string lastedVersionCode = createrServer.text;
            createrServer.Dispose();
            if (currentVersionCode.CheckIsNewVersion(lastedVersionCode))
            {
                //start download file
                yield return ResUpdate(lastedVersionCode);
                yield return StartGame();
            }
            else {
                //res version is lastest , start game
                yield return StartGame();
            }
        }
        else { 
            //Real Mode Get 
        }
        
        yield break;//相当于return，直接结束当前方法
    }

    IEnumerator ResUpdate(string lastVersionCode) {
        List<string> allAssetBundleNames = new List<string>();
        foreach (string name in AssetBundleManager.Instance.GetAssetBundleManifest.GetAllAssetBundleNames())
        {
            allAssetBundleNames.Add(name);
        }
        Dictionary<string, string> needUpdateContainer = new Dictionary<string, string>();
        for (int i=0;i< allAssetBundleNames.Count;i++) {
            string serverPath = Path.Combine(System.IO.Directory.GetParent(Application.dataPath).ToString(), assetbundleRootPath, allAssetBundleNames[i]);
            string localPath = Path.Combine(Application.streamingAssetsPath, assetbundleRootPath, allAssetBundleNames[i]);
            needUpdateContainer.Add(serverPath,localPath);
        }
        List<string> needUpdateRes = CheckNeedUpdate(needUpdateContainer);
        //noticetips download bundle
        UINoticeTip.Instance.ShowTips(TipsType.ResUpdate, UpdateRes, Cancel, "Download Res , Need Flow Size :"+FileTools.GetFileListSize(needUpdateRes));
        yield return UINoticeTip.Instance.Response();
        //click update ,start download ,first delete file , secondly copy file to target content
#if UNITY_EDITOR
        foreach (string key in needUpdateRes) {
            string value = needUpdateContainer[key];
            GameUtility.SafeDeleteFile(value);
            UnityEditor.FileUtil.CopyFileOrDirectoryFollowSymlinks(key, value);
        }
        FileTools.UpdateLocalVersionCode(Path.Combine(Application.streamingAssetsPath, assetbundleRootPath, resversionPath), lastVersionCode);
#endif
        yield break;
    }
    private List<string> CheckNeedUpdate(Dictionary<string, string> needUpdateContainer) {
        List<string> finalNeedUpdateList = new List<string>();
        foreach (string key in needUpdateContainer.Keys) {
            string serverMD5Value = FileTools.GetFileMD5Value(key);
            string localMD5Value = FileTools.GetFileMD5Value(needUpdateContainer[key]);
            //Debug.Log("key:"+key+",value:"+ needUpdateContainer[key]+",\n"+ "serverMD5Value:"+ serverMD5Value+ "\nlocalMD5Value"+ localMD5Value);
            if (!serverMD5Value.Equals(localMD5Value)) {
                finalNeedUpdateList.Add(key);
            }
        }
        return finalNeedUpdateList;
    }
    IEnumerator StartGame() {
        transform.FindAll(loadTextPath).GetComponent<Text>().text = "Loading core data....";
        yield return AssetBundleManager.Instance.Clear();

        Debug.Log("clear assetbundle completed...");
        yield return AssetBundleManager.Instance.Initialize();

        Manifest manifest = AssetBundleManager.Instance.GetAssetBundleManifest;
        foreach (string assetbundle in manifest.GetAllAssetBundleNames())
        {
            var request = AssetBundleManager.Instance.RequestAssetBundleAsync(assetbundle, true);
            yield return request;
            request.Dispose();
        }
        yield return new WaitForSeconds(0.5f);
        XLuaManager.Instance.Restart();
        XLuaManager.Instance.StartGame();
        UINoticeTip.Instance.Destroy();
        Destroy(gameObject);
        yield break;
    }

    private void Cancel() {
        Application.Quit();
    }
    private void DownLoadApp() {
        Application.Quit();
    }
    private void UpdateRes() { 
        
    }
}
