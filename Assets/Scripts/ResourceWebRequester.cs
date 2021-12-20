using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 资源请求
/// </summary>
public class ResourceWebRequester : ResourceAsyncOperation
{
    static Queue<ResourceWebRequester> resourceWebRequestersQueue = new Queue<ResourceWebRequester>();
    static int count=0;
    private WWW www = null;
    private bool isOver = false;
    public static ResourceWebRequester Get() {
        if (resourceWebRequestersQueue.Count > 0)
        {
            return resourceWebRequestersQueue.Dequeue();
        }
        else {
            return new ResourceWebRequester(++count);
        }
    }
    public ResourceWebRequester(int num) {
        Count = num;
    }
    //recycle
    public static void Recycle(ResourceWebRequester creater)
    {
        resourceWebRequestersQueue.Enqueue(creater);
    }
    //ResourceWebRequester total nums
    public int Count
    {
        get;
        protected set;
    }

    public void Init(string assetbundleName, string url, bool Cache = true) {
        this.assetbundleName = assetbundleName;
        this.url = url;
        this.Cache = Cache;
        www = null;
        isOver = false;
    }

    public bool Cache
    {
        get;
        protected set;
    }

    public string assetbundleName
    {
        get;
        protected set;
    }

    public string url
    {
        get;
        protected set;
    }

    public AssetBundle assetbundle
    {
        get
        {
            return www.assetBundle;
        }
    }

    public byte[] bytes
    {
        get
        {
            return www.bytes;
        }
    }

    public string text
    {
        get
        {
            return www.text;
        }
    }

    public string error
    {
        get
        {
            return string.IsNullOrEmpty(www.error) ? null : www.error;
        }
    }

    public override bool IsDone()
    {
        return isOver;
    }

    public override float Progress()
    {
        if (isDone) {
            return 1;
        }
        return (www == null) ? 0 : www.progress;
    }
    
    public override void Update()
    {
        if (isDone)
        {
            return;
        }

        isOver = www != null && (www.isDone || !string.IsNullOrEmpty(www.error));
        if (!isOver)
        {
            return;
        }

        if (www != null && !string.IsNullOrEmpty(www.error))
        {
            
        }
    }
    /// <summary>
    /// start download
    /// </summary>
    /// <returns></returns>
    public void Start()
    {
        www = new WWW(url);
        if (www == null)
        {
            isOver = true;
            Debug.LogError("Downloading Error: " + url);
            //Debug.Log("Download ["+url+"] complete...");
        }
        else
        {
          
           
        }
    }
    public override void Dispose()
    {
        base.Dispose();
        if (www!=null) {
            www.Dispose();
            www = null;
        }
        Recycle(this);
    }
}
