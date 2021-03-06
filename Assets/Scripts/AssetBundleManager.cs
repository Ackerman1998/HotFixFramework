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
    private string assetMappingEditorName = "EditorAssetsMapping.bytes";//映射文件(Editor用)
    private string assetMappingRealName = "AssetsMapping";
    private string assetEditorPath = "AssetPackage";
    private string assetbundleRootPath = Application.streamingAssetsPath + "/Assetbundle";
    private Manifest manifest;
    //Assetbundle缓存
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
    //assetbundle的引用计数
    Dictionary<string, int> assetbundleRefCount = new Dictionary<string, int>();
    //编辑器模式下的文件映射
    Dictionary<string, string> editorAssetMappingDict = new Dictionary<string, string>();
    int currentNum = 0;
    public string StreamingRootFolderName
    {
        get
        {
            return "AssetBundle";
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
        assetbundleRefCount.Clear();
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

    public void RemoveAssetBundleCache(string name) {
        if (assetBundle_Container.ContainsKey(name)) {
            assetBundle_Container.Remove(name);
        }
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
        Debug.Log(path);
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
        InCreaseAssetBundleRef(filePath.AddEndName(assetbundleEndName));
        return creater;
    }
    /// <summary>
    /// 异步加载assetbundle
    /// </summary>
    public BaseAssetBundleAsyncLoader LoadAssetBundleAsync(string name) {
        var loader = AssetBundleAsyncLoader.Get();
        
        prosessingAssetBundleAsyncLoader.Add(loader);
        if (manifest != null)
        {
            //load manifest
            string[] dependency = manifest.GetAllDependencies(name);
            for (int i=0;i<dependency.Length;i++) {
                if (dependency.Length>0&& !dependency[i].Equals(name)) {
                    RequestAssetBundleHasEndAsync(dependency[i]);
                    InCreaseAssetBundleRef(dependency[i]);
                }
            }
            loader.Init(name);
        }
        else {
            loader.Init(name);
        }
        RequestAssetBundleHasEndAsync(name);
        InCreaseAssetBundleRef(name);
        return loader;
    }

    public BaseAssetAsyncLoader LoadAssetAsync(string assetName) {
        //editor mode
        //Debug.LogError(assetName);
#if UNITY_EDITOR
        if (EditorConfig.SelectMode==0) {
            ReadEditorAssetMapping();
            string path = GetAssetPathByEditorAssetMapping(assetName);
            print(path);
            UnityEngine.Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path,typeof(UnityEngine.Object));
            print(obj==null);
            return new EditorAssetAsyncLoader(obj);
        }
#endif
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
    /// <summary>
    /// 卸载
    /// containerEnd ： 是否需要添加后缀(.assetbundle)
    /// </summary>
    /// <param name="assetbundleName"></param>
    /// <param name="containerEnd"></param>
    /// <returns></returns>
    public bool UnLoadAssetBundle(string assetbundleName,bool containerEnd) {
        string abName;
        if (containerEnd)
        {
            abName=assetbundleName.AddEndName(assetbundleEndName);
        }
        else {
            abName = assetbundleName;
        }
        if (GetAssetBundleRef(abName) >0) {
            Debug.Log(string.Format("[UnLoadAssetBundle {0} Failed: AssetBundle RefNumber>0]", assetbundleName));
            return false;
        }
        var assetbundle = GetAssetBundleCache(assetbundleName);
        if (assetbundle!=null) {
            assetbundle.Unload(false);
            RemoveAssetBundleCache(assetbundleName);
            Debug.Log(string.Format("[UnLoadAssetBundle {0} Completed ! ]", assetbundleName));
            //remove dependence assetbundle
            if (manifest!=null) {
                string[] depedency = manifest.GetAllDependencies(abName);
                if (depedency.Length>0) {
                    for (int i=0;i<depedency.Length;i++) {
                        int refCount = assetbundleRefCount[depedency[i]];
                        //引用计数=0,移除
                        if (refCount<=0) {
                            UnLoadAssetBundle(depedency[i],false);
                        }
                    }
                }
            }
        }
        return false;
    }

    public int GetAssetBundleRef(string assetbundleName) {
        int count = 0;
        assetbundleRefCount.TryGetValue(assetbundleName, out count);
        return count;
    }

    public void InCreaseAssetBundleRef(string assetbundleName) {
        int count = 0;
        assetbundleRefCount.TryGetValue(assetbundleName,out count);
        count++;
        assetbundleRefCount[assetbundleName] = count;
    }

    public int DeCreaseAssetBundleRef(string assetbundleName,bool autoUnLoad=false)
    {
        int count = 0;
        assetbundleRefCount.TryGetValue(assetbundleName, out count);
        count--;
        if (count <= 0 && autoUnLoad)
        {
            UnLoadAssetBundle(assetbundleName,false);
        }
        else {
            assetbundleRefCount[assetbundleName] = count;
        }
        return count;
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
                    if (creater.Cache&& JudgeCanAddCache(creater.assetbundleName))
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
                    DeCreaseAssetBundleRef(prosessingAssetBundleAsyncLoader[i].assetbundleName.AddEndName(assetbundleEndName),true);
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
    /// <summary>
    /// 判断是否已经添加到了内存中
    /// </summary>
    private bool JudgeCanAddCache(string name) {
        string newStr = name.Replace(".assetbundle", "");
        if (!assetBundle_Container.ContainsKey(newStr))
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Unity Editor Read Asset
    public void ReadEditorAssetMapping() {
        if (editorAssetMappingDict.Count <= 0)
        {
            string targetPath = Path.Combine(Application.dataPath, assetEditorPath, assetMappingEditorName);
            //read
            string [] editorAssetMappings = File.ReadAllLines(targetPath);
            foreach (string asm in editorAssetMappings) {
                string [] asName = asm.Split(',');
                if (!editorAssetMappingDict.ContainsKey(asName[0])) {
                    editorAssetMappingDict.Add(asName[0], asName[1]);
                }
            }
        }
    }
    public string GetAssetPathByEditorAssetMapping(string asset) {
        if (editorAssetMappingDict.ContainsKey(asset))
        {
            return editorAssetMappingDict[asset];
        }
        return null;
    }
    [BlackList]
    //获取映射文件中的目标文件路径(Editor下用)
    public string GetFilePathInAssetMappingForEditor(string fileName)
    {
        //AssetMapping assetMapping = mapping_List.Find((x) => (x.assetName == fileName));
        //if (assetMapping.assetbundleName.Length != 0 && assetMapping.assetName.Length != 0 && assetMapping.loadPath.Length != 0)
        //{
        //    return assetMapping.loadPath;
        //}
        //return null;
#if UNITY_EDITOR
        if (EditorConfig.SelectMode == 0)
        {
            ReadEditorAssetMapping();
            string path = GetAssetPathByEditorAssetMapping(fileName);
            string newPath = path.Replace("Assets/AssetPackage/Scripts/", "");
            string newPath2 = newPath.Substring(0,newPath.IndexOf("."))+".lua";
            return newPath2;
        }
#endif
        return null;
    }
    #endregion
}
