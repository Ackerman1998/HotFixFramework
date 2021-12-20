using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;

public class AssetBundleManager : MonoSingleton<AssetBundleManager>
{
    //可同时创建的数量
    private int maxCreateAssetBundleNum = 5;
    private string assetbundleEndName = "assetbundle";//后缀名
    private string assetMappingName = "assetsmapping_bytes";//映射文件名
    private string assetMappingRealName = "AssetsMapping";
    private string assetbundleRootPath = Application.streamingAssetsPath + "/Assetbundle";
    private Manifest manifest;
    //常驻Assetbundle容器
    private Dictionary<string, AssetBundle> assetBundle_Container = new Dictionary<string, AssetBundle>();
    //加载的资源映射表
    private List<AssetMapping> mapping_List = new List<AssetMapping>();
    //加载资源请求
    //Dictionary<string, ResourceWebRequester> webRequestingContainer = new Dictionary<string, ResourceWebRequester>();
    //等待处理的资源请求
    Queue<ResourceWebRequester> webRequesterQueue = new Queue<ResourceWebRequester>();
    //正在处理的资源请求
    List<ResourceWebRequester> prosessingWebRequester = new List<ResourceWebRequester>();
    //常驻Assetbundle
    List<string> residentAssetBundle = new List<string>();
    int currentNum = 0;
    public string StreamingRootFolderName
    {
        get
        {
            return "Assetbundle";
        }
        private set { }
    }
    public Manifest GetAssetBundleManifest {
        get {
            return manifest;
        }
        private set { }
    }
    public override void Awake()
    {
        base.Awake();
        assetbundleRootPath = assetbundleRootPath.CheckEnvironmentPath();
    }
    //load assetbundle 
    public IEnumerator Initialize() {

#if UNITY_EDITOR
        if (EditorConfig.SelectMode == 0)
        {//Editor Mode,skip
           
        }
#endif
        //load assetbundlemanifest and assetmapping
        manifest = new Manifest();
        var createrManifest = RequestAssetBundleAsync(StreamingRootFolderName,false);
        var createrAssetMapping = RequestAssetBundleHasEndAsync(assetMappingName,false);
        yield return createrManifest;
        AssetBundle assetbundle_Manifest = createrManifest.assetbundle;
        manifest.LoadFromAssetbundle(assetbundle_Manifest);
        assetbundle_Manifest.Unload(false);
        createrManifest.Dispose();
        yield return createrAssetMapping;
        AssetBundle assetbundle_Mapping = createrAssetMapping.assetbundle;
        ReadAssetsMapping(assetbundle_Mapping);
        createrAssetMapping.Dispose();
        //string[] allAssetbundle = manifest.GetAllAssetBundleNames();
        //foreach (string ab in allAssetbundle) {
        //    AddResidentAssetBundle(ab);
        //}
        yield break;
    }
    public void AddResidentAssetBundle(string abName) {
        if (!residentAssetBundle.Contains(abName)) {
            residentAssetBundle.Add(abName);
            RequestAssetBundleAsync(abName,true);
        }
    }
    public void LoadAssetbundle(string abName) {
        string finalPath = Path.Combine(assetbundleRootPath, abName + "." + assetbundleEndName);
        string key = abName;
        if (assetBundle_Container.ContainsKey(key)) {
            Debug.LogError("assetbundle : ["+key+"] is already exist...");
        }
        RequestTools.Instance.LoadAsset<AssetBundle>(finalPath, (obj) => {
            assetBundle_Container.Add(key, obj);
        });
    }
    /// <summary>
    /// 加载 Object类型
    /// </summary>
    public T LoadAssets<T>(string assetName,bool isUnload=false) where T : Object
    {
        AssetMapping assetMapping = mapping_List.Find((x) => (x.assetName == assetName));
        if (assetMapping.assetbundleName.Length != 0 && assetMapping.assetName.Length != 0 && assetMapping.loadPath.Length != 0)
        {
            if (assetBundle_Container.ContainsKey(assetMapping.assetbundleName))
            {
                T obj = assetBundle_Container[assetMapping.assetbundleName].LoadAsset<T>(assetName);
                if (isUnload) {
                    assetBundle_Container[assetMapping.assetbundleName].Unload(false);
                }
                return obj;
            }
            else
            {
                Debug.LogError("asset bundle:[" + assetMapping.assetbundleName + "] not exist,please check asset content"+ assetBundle_Container.Count);
            }
        }
        else {
            Debug.LogError("asset:["+assetName+"] not exist,please check asset content");
        }
        return null;
    }
    public GameObject LoadGameObject(string assetName, bool isUnload = false)
    {
        AssetMapping assetMapping = mapping_List.Find((x) => (x.assetName == assetName));
        if (assetMapping.assetbundleName.Length != 0 && assetMapping.assetName.Length != 0 && assetMapping.loadPath.Length != 0)
        {
            if (assetBundle_Container.ContainsKey(assetMapping.assetbundleName))
            {
                GameObject obj = assetBundle_Container[assetMapping.assetbundleName].LoadAsset<GameObject>(assetName);
                if (isUnload)
                {
                    assetBundle_Container[assetMapping.assetbundleName].Unload(false);
                }
#if UNITY_EDITOR
                var go = obj;
                if (go != null)
                {
                    var renderers = go.GetComponentsInChildren<Renderer>();
                    for (int j = 0; j < renderers.Length; j++)
                    {
                        var mat = renderers[j].sharedMaterial;
                        if (mat == null)
                        {
                            continue;
                        }

                        var shader = mat.shader;
                        if (shader != null)
                        {
                            var shaderName = shader.name;
                            mat.shader = Shader.Find(shaderName);
                        }
                    }
                }
#endif
                return obj;
            }
            else
            {
                Debug.LogError("asset bundle:[" + assetMapping.assetbundleName + "] not exist,please check asset content");
            }
        }
        else
        {
            Debug.LogError("asset:[" + assetName + "] not exist,please check asset content");
        }
        return null;
    }
    //read analysis assetsmapping file
    public void ReadAssetsMapping(AssetBundle assetbundle_Mapping) {
        TextAsset ts = assetbundle_Mapping.LoadAsset(assetMappingRealName) as TextAsset;
        //load assets mapping 
        string mapping_texts = ts.text;
        string[] mapping_text = mapping_texts.Split('\n');
        for (int i = 0; i < mapping_text.Length; i++)
        {
            string assetbundle_name = mapping_text[i].Split(',')[0];
            int first = mapping_text[i].Split(',')[1].LastIndexOf('/')+1;
            int end = mapping_text[i].Split(',')[1].LastIndexOf('.');
            string file_name = mapping_text[i].Split(',')[1].Substring(first, end-first);
            int first_index = mapping_text[i].Split(',')[1].FirstIndexOf('/')+1;
            string path_name = mapping_text[i].Split(',')[1].Substring(first_index, end-first_index);
            AssetMapping assetMapping = new AssetMapping();
            assetMapping.assetbundleName = assetbundle_name;
            assetMapping.assetName = file_name;
            assetMapping.loadPath = path_name;
            mapping_List.Add(assetMapping);
        }
        assetbundle_Mapping.Unload(true);
        //print("mapping_List.Count:" + mapping_List.Count);
        //foreach (AssetMapping assetMapping in mapping_List)
        //{
        //    print(assetMapping.assetbundleName + ",,," + assetMapping.assetName + "..." + assetMapping.loadPath);
        //}
    }
    static string GetMD5(byte[] data)
    {
        MD5 md5 = MD5.Create();
        md5 = new MD5CryptoServiceProvider();
        byte[] targetData = md5.ComputeHash(data);
        StringBuilder strBuilder = new StringBuilder();
        for (int i = 0; i < targetData.Length; i++)
        {
            strBuilder.AppendFormat("{0:x2}", targetData[i]);
        }
        Debug.Log(strBuilder.ToString());
        return strBuilder.ToString();
    }
    #region Load Assets
    /// <summary>
    /// 异步请求常规资源(非Assetbundle资源) 如：文本文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="streamingAssetsOnly"></param>
    public ResourceWebRequester RequestAssetFileAsync(string filePath) {
        var creater = ResourceWebRequester.Get();
        string path = Path.Combine(assetbundleRootPath, filePath);
        creater.Init(filePath, path,false);
        webRequesterQueue.Enqueue(creater);
        return creater;
    }
    /// <summary>
    /// 异步请求Assetbundle资源 无后缀
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public ResourceWebRequester RequestAssetBundleAsync(string filePath,bool cache=true)
    {
        var creater = ResourceWebRequester.Get();
        string path = Path.Combine(assetbundleRootPath, filePath);
        creater.Init(filePath, path, cache);
        webRequesterQueue.Enqueue(creater);
        return creater;
    }
    /// <summary>
    /// 异步请求Assetbundle资源 带后缀.assetbundle
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="cache"></param>
    /// <returns></returns>
    public ResourceWebRequester RequestAssetBundleHasEndAsync(string filePath, bool cache = true)
    {
        var creater = ResourceWebRequester.Get();
        string path = Path.Combine(assetbundleRootPath, filePath);
        path= path.AddEndName(assetbundleEndName);
        creater.Init(filePath, path, cache);
        webRequesterQueue.Enqueue(creater);
        return creater;
    }
    #endregion

    #region Mono Function
    private void Update()
    {
        OnProsessingWebRequester();
    }
    
    //处理加载资源的请求
    private void OnProsessingWebRequester() {
        if (prosessingWebRequester.Count > 0)
        {
            for (int i = prosessingWebRequester.Count - 1; i >= 0; i--)
            {
                var creater = prosessingWebRequester[i];
                creater.Update();
                if (creater.IsDone())
                {
                    //load success，recycle creater
                    prosessingWebRequester.RemoveAt(i);
                    if (creater.Cache)
                    {
                        AddAssetBundleCache(creater.assetbundleName, creater.assetbundle);
                        //creater.Dispose();
                    }
                    else
                    {
                        //不缓存，手动回收
                    }
                }
            }
        }
        currentNum = prosessingWebRequester.Count;
        while (currentNum < maxCreateAssetBundleNum&& webRequesterQueue.Count>0) {
            var creater = webRequesterQueue.Dequeue();
            creater.Start();
            prosessingWebRequester.Add(creater);
            currentNum++;
        }
        currentNum = 0;
    }
    private void AddAssetBundleCache(string name,AssetBundle ab) {
        if (!assetBundle_Container.ContainsKey(name)) {
            assetBundle_Container.Add(name, ab);
        }
    }
    //获取映射文件中的目标文件路径(Editor下用)
    public string GetFilePathInAssetMappingForEditor(string fileName) {
        AssetMapping assetMapping = mapping_List.Find((x) => (x.assetName == fileName));
        if (assetMapping.assetbundleName.Length != 0 && assetMapping.assetName.Length != 0 && assetMapping.loadPath.Length != 0)
        {
            return assetMapping.loadPath;
        }
        return null;
    }
    #endregion
}
