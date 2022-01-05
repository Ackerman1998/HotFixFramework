using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[XLua.ReflectionUse]
[XLua.LuaCallCSharp]
public static class LuaCallCsharpFunc 
{
    public static GameObject GetAsset(string name)
    {
        GameObject obj = AssetBundleManager.Instance.LoadAssets<GameObject>(name);
        return obj;
    }
    public static ResourceWebRequester GetResourceAsync(string name)
    {
        var loader = AssetBundleManager.Instance.RequestAssetFileAsync(name);
        return loader;
    }
}
