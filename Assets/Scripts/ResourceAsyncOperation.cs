using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 异步请求基类
/// </summary>
public abstract class ResourceAsyncOperation : IEnumerator, IDisposable
{
    public object Current
    {
        get {
            return null;
        }
    }

    public bool isDone
    {
        get
        {
            return IsDone();
        }
    }

    public float progress
    {
        get
        {
            return Progress();
        }
    }


    public virtual void Dispose()
    {
        
    }

    public bool MoveNext()
    {
        return !IsDone();
    }

    public void Reset()
    {
        
    }
    abstract public void Update();
    abstract public bool IsDone();
    abstract public float Progress();
}
/// <summary>
/// 异步加载ab基类
/// </summary>
public abstract class BaseAssetBundleAsyncLoader : ResourceAsyncOperation
{
    public string assetbundleName
    {
        get;
        protected set;
    }


    public AssetBundle assetbundle
    {
        get;
        protected set;
    }
    public override void Dispose()
    {
        base.Dispose();
        assetbundle = null;
        assetbundleName = null;
    }
}
//异步加载资源文件
abstract public class BaseAssetAsyncLoader : ResourceAsyncOperation
{
    public UnityEngine.Object asset
    {
        get;
        protected set;
    }

    public override void Dispose()
    {
        asset = null;
    }
}