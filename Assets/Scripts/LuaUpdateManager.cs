using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
/// <summary>
/// Lua代码刷新
/// </summary>
[CSharpCallLua]
[Hotfix]
public class LuaUpdateManager : MonoSingleton<LuaUpdateManager>
{
    //要执行的方法
    private Action<float, float> updateFunc = null;
    private Action laterUpdateFunc = null;
    private Action fixedUpdateFunc = null;
    private LuaEnv _luaEnv = null;
    public override void Awake()
    {
        base.Awake();
    }
    public override void Init()
    {
        base.Init();
    }
    //start 
    public void StartUp(LuaEnv luaEnv) {
        Run(luaEnv);
    }
    private void Run(LuaEnv luaEnv) {
        _luaEnv = luaEnv;
        if (_luaEnv!=null) {
            updateFunc = _luaEnv.Global.Get<Action<float,float>>("Update");
            laterUpdateFunc = _luaEnv.Global.Get<Action>("LateUpdate");
           // fixedUpdateFunc = _luaEnv.Global.Get<Action>("FixedUpdate");
        }
    }
    private void Update()
    {
        if (updateFunc!=null) {
            updateFunc(Time.deltaTime,Time.unscaledDeltaTime);
        }
    }
    private void LateUpdate()
    {
        if (laterUpdateFunc != null)
        {
            laterUpdateFunc();
        }
    }
    //private void FixedUpdate()
    //{
    //    if (fixedUpdateFunc != null)
    //    {
    //        fixedUpdateFunc();
    //    }
    //}
}
