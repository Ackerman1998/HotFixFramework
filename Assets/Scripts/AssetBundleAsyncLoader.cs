using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleAsyncLoader : BaseAssetBundleAsyncLoader
{
    private bool isOver = false;
    static Queue<AssetBundleAsyncLoader> assetBundleAsyncLoaders = new Queue<AssetBundleAsyncLoader>();
    static int count = 0;
    private List<string> waitLoaders = new List<string>();
    public static AssetBundleAsyncLoader Get() {
        AssetBundleAsyncLoader aba = null;
        if (assetBundleAsyncLoaders.Count > 0)
        {
            aba = assetBundleAsyncLoaders.Dequeue();
        }
        else {
            aba = new AssetBundleAsyncLoader(++count);
        }
        return aba;
    }
    public void Recycle(AssetBundleAsyncLoader loader) {
        assetBundleAsyncLoaders.Enqueue(loader);
    }
    public void Init(string assetName) {
        isOver = false;
        assetbundleName = assetName;
        var ab = AssetBundleManager.Instance.GetAssetBundleCache(assetbundleName);
        if (ab == null)
        {
            waitLoaders.Add(assetbundleName);
        }
    }
    public AssetBundleAsyncLoader(int cc) {
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

    public override float Progress()
    {
        return 0;
    }

    public override void Update()
    {
        if (IsDone()) {
            return;
        }
        for (int i= waitLoaders.Count-1;i>=0;i--) {
            var obj = waitLoaders[i];
            if (obj== assetbundleName) {
                assetbundle = AssetBundleManager.Instance.GetAssetBundleCache(obj);
                if (assetbundle!=null) {
                    waitLoaders.RemoveAt(i);
                }
            }
        }

        isOver = (waitLoaders.Count == 0);
    }
    public override void Dispose()
    {
        base.Dispose();
        waitLoaders.Clear();
        Recycle(this);
    }
}
