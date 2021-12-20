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
    public override void Awake()
    {
        base.Awake();
        Button_Download = transform.FindAll("Button_Download").GetComponent<Button>();
        Button_Exit = transform.FindAll("Button_Exit").GetComponent<Button>();
        Button_Update = transform.FindAll("Button_Update").GetComponent<Button>();
        TipsText = transform.FindAll("TipsText").GetComponent<Text>();
        Hide();
    }
    public void ShowTips(TipsType tipsType, UnityAction callback1, UnityAction callbackClose) {
        Show();
        switch (tipsType) {
            case TipsType.AppUpdate:
                Button_Download.onClick.AddListener(callback1);
                Button_Exit.onClick.AddListener(callbackClose);
                TipsText.text = "App Version is Oldest,Please Update App...";
                break;
            case TipsType.ResUpdate:
                Button_Update.onClick.AddListener(()=> {
                    callback1();
                    Hide();
                });
                Button_Exit.onClick.AddListener(callbackClose);
                TipsText.text = "Res Version is Oldest,Please Download Res...";
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
}
/// <summary>
/// 提示类型
/// </summary>
public enum TipsType { 
    AppUpdate,
    ResUpdate,
    NoUpdate
}