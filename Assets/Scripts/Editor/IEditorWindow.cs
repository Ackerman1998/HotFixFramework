using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 编辑器窗口基类
/// </summary>
public abstract class IEditorWindow : EditorWindow
{
    #region 常用编辑器函数
    /// <summary>
    /// 编辑器提示弹框
    /// </summary>
    /// <param name="msg"></param>
    public void ShowDisplayDialog(string msg)
    {
        EditorUtility.DisplayDialog("EditorWindow", msg, "ok!");
    }
    /// <summary>
    /// 刷新编辑器ASSETS
    /// </summary>
    protected void RefreshEditor()
    {
        AssetDatabase.Refresh();
    }
    Vector2 scrollPos;
    /// <summary>
    /// 滑动布局
    /// </summary>
    /// <param name="callback"></param>
    protected void ScrollLayout(Action callback)
    {

        scrollPos = GUILayout.BeginScrollView(scrollPos);
        callback?.Invoke();
        GUILayout.EndScrollView();
    }

    /// <summary>
    /// 纵向布局
    /// </summary>
    /// <param name="callback"></param>
    protected void VerticalLayout(Action callback)
    {
        GUILayout.BeginVertical("box");
        callback?.Invoke();
        GUILayout.EndVertical();
    }
    /// <summary>
    /// 横向布局
    /// </summary>
    /// <param name="callback"></param>
    protected void HorizontalLayout(Action callback)
    {
        GUILayout.BeginHorizontal("box");
        callback?.Invoke();
        GUILayout.EndHorizontal();
    }
    /// <summary>
    /// 创建文字框
    /// </summary>
    /// <param name="msg"></param>
    protected void CreateLabel(string msg)
    {
        GUIStyle style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 15;
        style.normal.textColor = Color.white;
        GUILayout.Label(msg, style);
    }
    protected void CreateLabel(string msg, Color color)
    {
        GUIStyle style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 15;
        style.normal.textColor = color;
        GUILayout.Label(msg, style);
    }
    /// <summary>
    /// 创建按钮
    /// </summary>
    /// <param name="btnName"></param>
    /// <param name="btnEvent"></param>
    protected void CreateButton(string btnName, Action btnEvent)
    {
        if (GUILayout.Button(btnName))
        {
            btnEvent?.Invoke();
        }
    }
    /// <summary>
    /// 创建文字输入框
    /// </summary>
    /// <param name="fieldName"></param>
    /// <param name="defualtVal"></param>
    /// <returns></returns>
    protected string CreateTextField(string fieldName, string defualtVal)
    {
        return EditorGUILayout.TextField(fieldName, defualtVal);
    }

    protected int CreateSelectMenu(string label,int num,string [] selectContent) {
        return EditorGUILayout.Popup(label,num, selectContent);
    }
    #endregion
}
