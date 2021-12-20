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
    public override void Awake()
    {
        base.Awake();
        InitUpdater();
    }
    private void InitUpdater() {
        transform.FindAll(sliderPath).GetComponent<Slider>().value=0; 
        transform.FindAll(loadTextPath).GetComponent<Text>().text="Check Resource..."; 
    }
    public void CheckUpdate() {
        StartCoroutine(Check());
    }
    IEnumerator Check() {
        yield return CheckAppVersion();

        yield return CheckResVersion();
    }
    IEnumerator CheckAppVersion()
    {
        var creater = AssetBundleManager.Instance.RequestAssetFileAsync(appversionPath);
        yield return creater;//当creater.MoveNext()等于true时会挂起这里的代码，等于false时继续执行后面的代码
        string currentVersionCode = creater.text;
        string lastedVersionCode = creater.text;//编辑器环境下，不做appversion的更新
        creater.Dispose();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        //get version code for server
#endif
        if (!currentVersionCode.CheckIsNewVersion(lastedVersionCode)) {
            //弹出noticetips redownload game
            UINoticeTip.Instance.ShowTips(TipsType.AppUpdate, DownLoadApp, Cancel);
        }
        yield break;//相当于return，直接结束当前方法
    }
    IEnumerator CheckResVersion()
    {
        var creater = AssetBundleManager.Instance.RequestAssetFileAsync(resversionPath);
        yield return creater;
        string currentVersionCode = creater.text;
        string lastedVersionCode = creater.text;
        creater.Dispose();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        //get version code for server
#endif
        if (!currentVersionCode.CheckIsNewVersion(lastedVersionCode))
        {
            //弹出noticetips download bundle
            UINoticeTip.Instance.ShowTips(TipsType.ResUpdate, UpdateRes, Cancel);
        }
        yield break;//相当于return，直接结束当前方法
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
