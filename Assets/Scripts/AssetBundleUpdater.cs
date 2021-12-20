using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetBundleUpdater : MonoSingleton<AssetBundleUpdater>
{
    private string sliderPath = "GameSlider";
    private string loadTextPath = "LoadText";
    private string appversionPath = "app_version.bytes";
    private string resversionPath = "res_version.bytes";
    private bool needUpdateRes = false;
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
        transform.FindAll(loadTextPath).GetComponent<Text>().text= "正在检测资源...."; 
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
        var start = DateTime.Now;
        yield return CheckAppVersion();
        Debug.Log(string.Format("Init AppVersion use {0}ms", (DateTime.Now - start).Milliseconds));
        bool isEditor = false;
#if UNITY_EDITOR
        isEditor = true;
#endif

        yield return CheckResVersion(isEditor);
    }
    IEnumerator CheckAppVersion()
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
        if (currentVersionCode.CheckIsNewVersion(lastedVersionCode)) {
            //弹出noticetips redownload game
            UINoticeTip.Instance.ShowTips(TipsType.AppUpdate, DownLoadApp, Cancel);
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
            string lastedVersionCode = creater.text;
            creater.Dispose();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        //get version code for server
#endif
            if ("1.0.000".CheckIsNewVersion(lastedVersionCode))
            {
                //弹出noticetips download bundle
                UINoticeTip.Instance.ShowTips(TipsType.ResUpdate, UpdateRes, Cancel);
                yield return UINoticeTip.Instance.Response();
                Debug.Log("结束调用");
            }
        }
        else { 
            
        }
        
        yield break;//相当于return，直接结束当前方法
    }

    IEnumerator StartGame() {
        transform.FindAll(loadTextPath).GetComponent<Text>().text = "正在加载核心数据....";
        XLuaManager.Instance.StartRun();
        UINoticeTip.Instance.Destroy();
        Destroy(gameObject,0.5f);
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
