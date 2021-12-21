using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
/// <summary>
/// 提示弹窗（通用）
/// </summary>
public class UINoticeTip : MonoSingleton<UINoticeTip>
{
    private Button Button_Download;
    private Button Button_Exit;
    private Button Button_Update;
    private Text TipsText;
    int LastIndex = -1;
    public override void Awake()
    {
        base.Awake();
        Button_Download = transform.FindAll("Button_Download").GetComponent<Button>();
        Button_Exit = transform.FindAll("Button_Exit").GetComponent<Button>();
        Button_Update = transform.FindAll("Button_Update").GetComponent<Button>();
        TipsText = transform.FindAll("TipsText").GetComponent<Text>();
        Hide();
    }
    public void ShowTips(TipsType tipsType, UnityAction callback1, UnityAction callbackClose,string tips) {
        RemoveAllListenerForButton();
        Show();
        switch (tipsType) {
            case TipsType.AppUpdate:
                Button_Download.onClick.AddListener(()=> {
                    callback1?.Invoke();
                    LastIndex = 1;
                });
                Button_Exit.onClick.AddListener(callbackClose);

                TipsText.text = tips;
                break;
            case TipsType.ResUpdate:
                Button_Update.onClick.AddListener(() => {
                    callback1?.Invoke();
                    LastIndex = 1;
                    Hide();
                });
                Button_Exit.onClick.AddListener(callbackClose);
                TipsText.text = tips;
                break;
        }
    }
    public void Hide() {
        transform.FindAll("Root").gameObject.SetActive(false);
    }
    public void Show() {
        transform.FindAll("Root").gameObject.SetActive(true);
    }
    public void RemoveAllListenerForButton() {
        Button_Download.onClick.RemoveAllListeners();
        Button_Exit.onClick.RemoveAllListeners();
        Button_Update.onClick.RemoveAllListeners();
    }
    public void Destroy() {
        mInstance = null;
        Destroy(gameObject);
    }
    /// <summary>
    /// 按钮响应
    /// </summary>
    /// <returns></returns>
    public IEnumerator Response() {
        yield return new WaitUntil(()=> {
            return LastIndex != -1;
        });
        yield break;
    }
}
/// <summary>
/// 提示类型
/// </summary>
public enum TipsType { 
    AppUpdate,
    ResUpdate,
    NoUpdate
}