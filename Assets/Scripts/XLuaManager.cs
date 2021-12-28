using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class XLuaManager : MonoSingleton<XLuaManager>
{
    private LuaEnv _luaEnv = null;
    private string mainLuaName = "GameMain";//主入口
    private string hotFixLuaName = "HotfixMain";
    private string commonLoadLuaName = "CommonLoad";
    public string LuaAssetBundleName
    {
        get {
            return "scripts";
        }
        private set { }
    }
    public override void Awake()
    {
        base.Awake();
    }
    public LuaEnv GetLuaEnv() {
        return _luaEnv;
    }
    /// <summary>
    /// 启动
    /// </summary>
    public override void Init()
    {
        base.Init();
        InitLuaEnv();
    }
    private void InitLuaEnv() {
        _luaEnv = new LuaEnv();
        if (_luaEnv != null)
        {
            _luaEnv.AddLoader(CustomLoader);
         
        }
        else {
            Debug.LogError("Init LuaEnv Failed...");
        }
    }
    public void DoString(string scriptContent) {
        _luaEnv.DoString(scriptContent);
    }
    public void DoRequire(string scriptContent) {
        _luaEnv.DoString(string.Format("require ('{0}')", scriptContent));
    }
    /// <summary>
    /// 开始热修复 restart:是否重启
    /// </summary>
    public void StartHotFix(bool restart=false)
    {
        if (_luaEnv == null) {
            return;
        }
        if (restart)
        {
            StopHotFix();
            ReloadLua(hotFixLuaName);
        }
        else {
            DoRequire(hotFixLuaName);
        }
        DoString("HotfixMain.Start()");
    }
    /// <summary>
    /// 停止热修复
    /// </summary>
    public void StopHotFix() {
        DoString("HotfixMain.Stop()");
    }
    /// <summary>
    /// 重新加载lua
    /// </summary>
    public void ReloadLua(string luaName) {
        DoString(string.Format("package.loaded['{0}']=nil",luaName));
        DoRequire(luaName);
    }

    /// <summary>
    /// 自定义加载(从AssetBundle中加载) require 'Global.Global'
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    private byte[] CustomLoader(ref string filepath)
    {
        string path = "";
        if (filepath.Contains("."))
        {
            path = filepath.Substring(filepath.LastIndexOf(".") + 1);
        }
        else
        {
            path = filepath;
        }
#if UNITY_EDITOR
        if (EditorConfig.SelectMode == 0)//Editor Mode ： 方便调试用
        {
            string newPath = path + ".lua";
            string file = AssetBundleManager.Instance.GetFilePathInAssetMappingForEditor(newPath);
         
            string pp = Application.dataPath + "/HotfixScripts/" + file;
            string content = File.ReadAllText(pp);
            return System.Text.Encoding.UTF8.GetBytes(content);
        }
#endif
        TextAsset ts = AssetBundleManager.Instance.LoadAssets<TextAsset>(path + ".lua");
        return ts.bytes;
    }
    /// <summary>
    /// 初始化GameMain，加载全局类
    /// </summary>
    public void StartLoad() {
        if (_luaEnv != null) {
            DoRequire(commonLoadLuaName);
            DoRequire(mainLuaName);
        }
    }

    public void Restart() {
        Dispose();
        InitLuaEnv();
        StartLoad();
        LuaUpdateManager.Instance.StartUp(_luaEnv);
    }
    /// <summary>
    /// 开始执行主方法,游戏开始运行
    /// </summary>
    public void StartGame() {
        if (_luaEnv!=null) {
            DoString("GameMain.Start()");
        }
    }
    
    //析构方法
    public override void Dispose() {
        if (_luaEnv != null)
        {
            _luaEnv.Dispose();
            _luaEnv = null;
        }
    }
}
