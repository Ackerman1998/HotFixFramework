using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 异步加载assets
/// </summary>
public class AssetAsyncLoader : BaseAssetAsyncLoader
{
    bool isOver = false;
    static Queue<AssetAsyncLoader> assetAsyncLoaders = new Queue<AssetAsyncLoader>();
    static int count = 0;
    private string assetName;
    private BaseAssetBundleAsyncLoader asyncLoader=null;
    public static AssetAsyncLoader Get()
    {
        AssetAsyncLoader aba = null;
        if (assetAsyncLoaders.Count > 0)
        {
            aba = assetAsyncLoaders.Dequeue();
        }
        else
        {
            aba = new AssetAsyncLoader(++count);
        }
        return aba;
    }
    public void Recycle(AssetAsyncLoader loader)
    {
        assetAsyncLoaders.Enqueue(loader);
    }
    public void Init(string assetName,UnityEngine.Object obj)
    {
        asset = obj;
        this.assetName = assetName;
        isOver = true;
        asyncLoader = null;
    }
    public void Init(string assetName, BaseAssetBundleAsyncLoader loader)
    {
        asset = null;
        this.assetName = assetName;
        asyncLoader = loader;
        isOver = false;
    }
    public AssetAsyncLoader(int cc)
    {
        Count = cc;
    }
    public int Count
    {
        get;
        protected set;
    }
    public override bool IsDone()
    {
        return isOver;
    }
    //暂时没用到
    public override float Progress()
    {
        return 0;
    }

    public override void Update()
    {
        if (IsDone()) {
            return;
        }
        isOver = asyncLoader.IsDone();
        if (isOver) {
            //load success
            asset = AssetBundleManager.Instance.GetAssetCache(assetName);
            asyncLoader.Dispose();
        }
    }
    public override void Dispose()
    {
        base.Dispose();
        this.assetName = null;
        asyncLoader = null;
        isOver = false;
    }
}
