using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using XLua;
using DateTime = System.DateTime;
[Hotfix]
[LuaCallCSharp]
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
    // 逻辑层正在等待的ab加载异步句柄
    List<AssetBundleAsyncLoader> prosessingAssetBundleAsyncLoader = new List<AssetBundleAsyncLoader>();
    List<AssetAsyncLoader> assetAsyncLoaders = new List<AssetAsyncLoader>();
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
    //引用是否清理完成
    public bool ClearCompleted
    {
        get {
            return webRequesterQueue.Count == 0 && prosessingWebRequester.Count == 0 && residentAssetBundle.Count == 0 &&
                mapping_List.Count == 0 && assetBundle_Container.Count== 0;
        }
    }

    public override void Awake()
    {
        base.Awake();
        assetbundleRootPath = assetbundleRootPath.CheckEnvironmentPath();
    }
    //load assetbundle 
    public IEnumerator Initialize() {
        manifest = new Manifest();
        var createrManifest = RequestAssetBundleAsync(StreamingRootFolderName,false);
        yield return createrManifest;
        AssetBundle assetbundle_Manifest = createrManifest.assetbundle;
        manifest.LoadFromAssetbundle(assetbundle_Manifest);
        assetbundle_Manifest.Unload(false);
        createrManifest.Dispose();
        var allAssetbundleNames = manifest.GetAllAssetBundleNames();
        foreach (var curAssetbundleName in allAssetbundleNames)
        {
            if (string.IsNullOrEmpty(curAssetbundleName))
            {
                continue;
            }
            SetAssetBundleResident(curAssetbundleName, true);
        }
#if UNITY_EDITOR
        if (EditorConfig.SelectMode == 0)
        {
            //when editor mode，read assetpackage mappingfile
            LoadAssetMappingByAssetPackage();
            yield break;
        }
#endif
        var createrAssetMapping = RequestAssetBundleHasEndAsync(assetMappingName,false);
        yield return createrAssetMapping;
        AssetBundle assetbundle_Mapping = createrAssetMapping.assetbundle;
        ReadAssetsMapping(assetbundle_Mapping);
        createrAssetMapping.Dispose();
        yield break;
    }

    public void SetAssetBundleResident(string assetbundleName,bool resident) {
        bool exist = residentAssetBundle.Contains(assetbundleName);
        if (exist&& !resident)
        {
            residentAssetBundle.Remove(assetbundleName);
        }
        else if (!exist && resident)
        {
            residentAssetBundle.Add(assetbundleName);
        }
    }

    //clear all assetbundle
    public IEnumerator Clear() {
        yield return new WaitUntil(()=> {
            return !ClearCompleted;
        });
        //clear assetbundle and unload
        foreach (AssetBundle val in assetBundle_Container.Values) {
            if (val!=null) {
                val.Unload(false);
            }
        }
        assetBundle_Container.Clear();
        webRequesterQueue.Clear();
        prosessingWebRequester.Clear();
        residentAssetBundle.Clear();
        mapping_List.Clear();
        yield break;
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

    public string GetAssetBundleNameForAsset(string assetName)
    {
        AssetMapping assetMapping = mapping_List.Find((x) => (x.assetName == assetName));
        if (assetMapping.assetbundleName.Length != 0 && assetMapping.assetName.Length != 0 && assetMapping.loadPath.Length != 0)
        {
            string newStr = assetMapping.assetbundleName.Replace(".assetbundle", "");
            return newStr;
        }
        else {
            return null;
        }
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
                Debug.LogError("asset bundle:[" + assetMapping.assetbundleName + "] not exist,please check asset content："+ assetName);
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
            string newStr = assetbundle_name.Replace(".assetbundle", "");
            assetMapping.assetbundleName = newStr;
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
    //从本地读取AssetMapping配置文件
    private void LoadAssetMappingByAssetPackage(){
        string finalPath = Path.Combine(Application.dataPath,"AssetPackage",assetMappingRealName+".bytes");
        string mapping_texts = File.ReadAllText(finalPath);
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
            string newStr = assetbundle_name.Replace(".assetbundle", "");
            assetMapping.assetbundleName = newStr;
            assetMapping.assetName = file_name;
            assetMapping.loadPath = path_name;
            mapping_List.Add(assetMapping);
        }
    }

    public AssetBundle GetAssetBundleCache(string name)
    {
        if (assetBundle_Container.ContainsKey(name))
        {
            return assetBundle_Container[name];
        }
        return null;
    }

    public UnityEngine.Object GetAssetCache(string assetName) {
        UnityEngine.Object result = LoadAssets<UnityEngine.Object>(assetName);
        return result;
    }
    //检测资源是否存在
    public bool AssetCacheIsExisted(string assetName) {
        AssetMapping assetMapping = mapping_List.Find((x) => (x.assetName == assetName));
        if (assetMapping.assetbundleName.Length != 0 && assetMapping.assetName.Length != 0 && assetMapping.loadPath.Length != 0)
        {
            if (assetBundle_Container.ContainsKey(assetMapping.assetbundleName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    #region Async Load Assets
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
    /// <summary>
    /// 异步加载assetbundle
    /// </summary>
    public BaseAssetBundleAsyncLoader LoadAssetBundleAsync(string name) {
        var loader = AssetBundleAsyncLoader.Get();
        loader.Init(name);
        RequestAssetBundleHasEndAsync(name);
        prosessingAssetBundleAsyncLoader.Add(loader);
        return loader;
    }

    public BaseAssetAsyncLoader LoadAssetAsync(string assetName) {
        var loader = AssetAsyncLoader.Get();
        bool isExist = AssetCacheIsExisted(assetName);
        assetAsyncLoaders.Add(loader);
        
        if (isExist) {//target asset is existed 
            loader.Init(assetName, GetAssetCache(assetName));
        }
        else {
            //获取packageName
            string assetBundleName = GetAssetBundleNameForAsset(assetName);
            var abLoader = LoadAssetBundleAsync(assetBundleName);
            loader.Init(assetName, abLoader);
        }
      
        return loader;
    }
    

    
    #endregion

    #region Mono Function
    private void Update()
    {
        OnProsessingWebRequester();
        OnProsessingAssetBundleAsyncLoader();
        OnProsessingAssetAsyncLoader();
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
                        creater.Dispose();
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
    //处理异步加载AssetBundle的请求
    private void OnProsessingAssetBundleAsyncLoader() {
        if (prosessingAssetBundleAsyncLoader.Count > 0) {
            for (int i= prosessingAssetBundleAsyncLoader.Count-1;i>=0;i--) {
                prosessingAssetBundleAsyncLoader[i].Update();
                if (prosessingAssetBundleAsyncLoader[i].IsDone()) {
                    prosessingAssetBundleAsyncLoader.RemoveAt(i);
                }
            }
        }
    }
    //处理异步加载Asset的请求
    private void OnProsessingAssetAsyncLoader()
    {
        if (assetAsyncLoaders.Count > 0)
        {
            for (int i = assetAsyncLoaders.Count - 1; i >= 0; i--)
            {
                assetAsyncLoaders[i].Update();
                if (assetAsyncLoaders[i].IsDone())
                {
                    assetAsyncLoaders.RemoveAt(i);
                }
            }
        }
    }
    private void AddAssetBundleCache(string name,AssetBundle ab) {
        string newStr = name.Replace(".assetbundle", "");
        if (!assetBundle_Container.ContainsKey(newStr)) {
            assetBundle_Container.Add(newStr, ab);
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
