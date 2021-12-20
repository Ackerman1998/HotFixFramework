using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorConfig 
{
#if UNITY_EDITOR
    /// <summary>
    /// 模式
    /// </summary>
    public static int SelectMode
    {
        get
        {
            return UnityEditor.EditorPrefs.GetInt("selectMode");
        }
        set
        {
            UnityEditor.EditorPrefs.SetInt("selectMode", value);
        }
    }
    /// <summary>
    /// App版本号
    /// </summary>
    public static string AppVersion
    {
        get
        {
            return UnityEditor.EditorPrefs.GetString("AppVersion");
        }
        set
        {
            UnityEditor.EditorPrefs.SetString("AppVersion", value);
        }
    }
    /// <summary>
    /// 资源版本号
    /// </summary>
    public static string ResVersion
    {
        get
        {
            return UnityEditor.EditorPrefs.GetString("ResVersion");
        }
        set
        {
            UnityEditor.EditorPrefs.SetString("ResVersion", value);
        }
    }
#endif
}
