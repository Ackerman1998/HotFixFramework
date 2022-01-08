using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;


public class AssetBundleTools : IEditorWindow
{
    private static string assetbundleRootPath = Application.dataPath + "/AssetPackage";
    private static string assetsScritpsRootPath = Application.dataPath + "/AssetPackage/Scripts";
    private static string scriptRootPath = Application.dataPath + "/HotfixScripts";
    private static string endName = "bytes";
    private static string assetbundleEndName = "assetbundle";
    private static string assetsMappingFilePath = Application.dataPath + "/AssetPackage/AssetsMapping.bytes";
    private static string assetsMappingEditorFilePath = Application.dataPath + "/AssetPackage/EditorAssetsMapping.bytes";
    private static string assetsMappingFileImporterPath = "Assets/AssetPackage/AssetsMapping.bytes";
    private static string appversionPath =  "app_version.bytes";
    private static string resversionPath = "res_version.bytes";
    private static string assetbundleOutputPath = Application.streamingAssetsPath + "/Assetbundle";
    private static List<string> unUsedExtension = new List<string>() {".meta" };//ignore extension 
    private string[] modes = new string[] {"Editor Mode","Real Mode" };
    private static string subStringPath = "Assets/AssetPackage/";
    private int currentSelectNum = 1;
    [MenuItem("AssetBundle/AssetBundle Config")]
    public static void  OpenAssetBundleTools() {
        EditorWindow editorWindow = EditorWindow.GetWindow(typeof(AssetBundleTools),false, "AssetBundle Config");
        editorWindow.position = new Rect(Screen.width/2,Screen.height/2, Screen.width/2 , Screen.height/2 );
        editorWindow.Show();
    }

    private void OnGUI()
    {
        VerticalLayout(()=> {
            EditorConfig.SelectMode = CreateSelectMenu("Mode Select",EditorConfig.SelectMode, modes);
            EditorConfig.AppVersion = CreateTextField("App Version Code",EditorConfig.AppVersion);
            EditorConfig.ResVersion = CreateTextField("Res Version Code",EditorConfig.ResVersion);
        });
    }

    //生成资源映射文件(仅用于Editor模式加载资源)
    [MenuItem("AssetBundle/GenAssetsMappingForEditor")]
    public static void GenAssetsMapping() {
        GenAssetsMappingForEditor(assetbundleRootPath);
    }
    public static void GenAssetsMappingForEditor(string assetsRootPath) {
        if (!Directory.Exists(assetsRootPath))
        {
            return;
        }
        List<string> fileMapping = new List<string>();
  
        string[] aaa = Directory.GetDirectories(assetsRootPath);
        List<string> allFilePackage = new List<string>();
        allFilePackage.Add(assetsRootPath);
        foreach (string package in allFilePackage)
        {
            string packName = Path.GetFileName(package);
            List<string> ff = GetAllFilePathForEditor(package.Replace(@"\", "/"), packName);
            if (ff.Count > 0)
            {
                fileMapping.InsertRange(0, ff);
            }
        }
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < fileMapping.Count; i++)
        {
            if (i == fileMapping.Count - 1)
            {
                sb.Append(fileMapping[i]);
            }
            else
            {
                sb.Append(fileMapping[i] + "\n");
            }
        }
        FileTools.WriteText(assetsMappingEditorFilePath, sb.ToString());
        AssetDatabase.Refresh();
    }

    public static List<string> GetAllFilePathForEditor(string assetsRootPath, string packName)
    {
        List<string> fileMapping = new List<string>();
        if (!Directory.Exists(assetsRootPath))
        {
            return fileMapping;
        }
        else
        {
            string[] allFile = Directory.GetFiles(assetsRootPath);
            foreach (string file in allFile)
            {
                if (!unUsedExtension.Contains(Path.GetExtension(file)))
                {
                    string pathName = file.Replace(assetbundleRootPath, "");
                    string rootPath = Application.dataPath;
                    rootPath = rootPath.Substring(0,rootPath.LastIndexOf("/")+1);
                    string newPath = file.Replace(@"\", "/").Replace(rootPath,"");
                    fileMapping.Add(file.Substring(file.LastIndexOf(@"\") + 1,file.IndexOf(".")-1- file.LastIndexOf(@"\")) +","+ newPath);
                }
            }
            string[] allFilePackage = Directory.GetDirectories(assetsRootPath);
            foreach (string package in allFilePackage)
            {
                //check next son root path 
                List<string> ff = GetAllFilePathForEditor(package.Replace(@"\", "/"), packName);
                if (ff.Count > 0)
                {
                    fileMapping.InsertRange(0, ff);
                }
            }
            return fileMapping;
        }
    }


    /// <summary>
    /// 获取目录下所有文件路径并生成映射
    /// </summary>
    /// <param name="assetsRootPath"></param>
    /// <returns></returns>
    public static List<string> GetAllFilePath(string assetsRootPath,string packName)
    {
        List<string> fileMapping = new List<string>();
        if (!Directory.Exists(assetsRootPath))
        {
            return fileMapping;
        }
        else
        {
            string[] allFile = Directory.GetFiles(assetsRootPath);
            foreach (string file in allFile)
            {
                if (!unUsedExtension.Contains(Path.GetExtension(file))) {
                    string pathName = file.Replace(assetbundleRootPath, "");
                    pathName = pathName.Remove(0,1);
                    pathName = pathName.Replace(@"\", "/");
                    fileMapping.Add(packName.ToLower() + "." + assetbundleEndName + "," + pathName);
                }
            }
            string[] allFilePackage = Directory.GetDirectories(assetsRootPath);
            foreach (string package in allFilePackage)
            {
                //check next son root path 
                List<string> ff = GetAllFilePath(package.Replace(@"\", "/"), packName);
                if (ff.Count>0) {
                    fileMapping.InsertRange(0, ff);
                }
            }
            return fileMapping;
        }
    }

    [MenuItem("AssetBundle/CopyAndGenBytesFile")]
    public static void GenAndCopyBytesFile() {
        var now = DateTime.Now;
        FileTools.ClearFilePackage(assetsScritpsRootPath);
        FileTools.CopyFilePackage(scriptRootPath, assetsScritpsRootPath);
        //FileTools.AlterAllFileEndNameByDirectory(assetsScritpsRootPath, endName);
        XLuaMenu.GenLuaBytesFile();
        Debug.Log("CopyAndGenBytesFile Compelted... using time : "+((float)((DateTime.Now - now).TotalMilliseconds / 100))+"s");
        AssetDatabase.Refresh();
    }
    [MenuItem("AssetBundle/Encrypt AssetBundle")]
    public static void EncryptAssetBundle() { 
    
    }

    #region AssetBundle Build
    //标记ab包
    [MenuItem("Assets/快速标记")]
    public static void Abs()
    {
        MarkAB(GetSelectedPath());
    }
    public static string GetSelectedPath()
    {
        var path = string.Empty;
        foreach (var obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                return path;
            }
        }
        return path;
    }
    public static void MarkAB(string path, bool isFolder = false)
    {
        if (!string.IsNullOrEmpty(path))
        {
            if (isFolder)
            {
                path = Path.GetDirectoryName(path);
            }
            else
            {

            }
            var ai = AssetImporter.GetAtPath(path);
            var dir = new DirectoryInfo(path);
            if (ai.assetBundleName == "" && ai.assetBundleVariant == "")
            {
                ai.assetBundleName = dir.Name.Replace(".", "_")+ "."+ assetbundleEndName;
                ai.assetBundleVariant = "";
                Debug.Log(path+"标记" + ai.assetBundleName + "成功");
            }
            else
            {
                Debug.Log("取消标记" + ai.assetBundleName);
                ai.assetBundleVariant = "";
                ai.assetBundleName = "";
            }
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }
    }

    [MenuItem("AssetBundle/Build Assetbundle")]
    public static void PackageAbs()
    {
        string directory = Path.Combine(System.IO.Directory.GetParent(Application.dataPath).ToString(), "AssetBundle");
        //string directory = assetbundleOutputPath;
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
      
        AssetBundleManifest assetBundleManifest = BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        if (assetBundleManifest != null)
        {
            //生成资源映射文件
            GenAssetMapFile(assetBundleManifest);
            //重新打一次AB
            BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
            Debug.Log("Build Assetbundle Success：" + directory);
        }
        // DirectoryInfo代表文件夹的一个类 可实例化 ,Directory 静态类 不可实例化
        DirectoryInfo directoryInfo = new DirectoryInfo(directory);
        FileInfo[] files = directoryInfo.GetFiles();
        foreach (FileInfo file in files)
        {
            //清除manifest文件，打包目录
            if (file.Name.EndsWith("manifest") )
            {
                File.Delete(file.FullName);
            }
        }
        FileTools.WriteText(Path.Combine(directory, appversionPath), EditorConfig.AppVersion);
        FileTools.WriteText(Path.Combine(directory, resversionPath), EditorConfig.ResVersion);
        EditorUtility.DisplayDialog("AssetBundle", "Build Assetbundle Success...","OK");
        //AssetDatabase.Refresh();
    }
    [MenuItem("AssetBundle/Clear Assetbundle")]
    public static void ClearAbs()
    {
        string directory = assetbundleOutputPath;
        DirectoryInfo directoryInfo = new DirectoryInfo(directory);
        FileInfo[] files = directoryInfo.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            File.Delete(files[i].FullName);
        }
        AssetDatabase.Refresh();
        Debug.Log("Clear AssetBundle Success...");
    }
    [MenuItem("AssetBundle/Copy AssetBundle To StreamingAsset")]
    public static void CopyAssetBundleToStreamingAsset() {
        string targetPath = assetbundleOutputPath;
        string sourcePath = Path.Combine(System.IO.Directory.GetParent(Application.dataPath).ToString(), "AssetBundle");
        GameUtility.SafeDeleteDir(targetPath);
        FileUtil.CopyFileOrDirectoryFollowSymlinks(sourcePath, targetPath);
        AssetDatabase.Refresh();
    }
    #endregion

    #region AssetBundle Function 
    private static void GenAssetMapFile(AssetBundleManifest assetBundleManifest) {
        List<string> mapping = new List<string>();
        string[] abPackageNames = assetBundleManifest.GetAllAssetBundles();
        foreach (string ab in abPackageNames)
        {
            string[] assets = AssetDatabase.GetAssetPathsFromAssetBundle(ab);
            foreach (string ass in assets)
            {
                //Debug.Log(ab+","+ass.Replace(subStringPath,""));
                mapping.Add(string.Format("{0}{1}{2}", ab, ",", ass.Replace(subStringPath, "")));
            }
        }
        
        string assetsRootPath = assetbundleRootPath;
        if (!Directory.Exists(assetsRootPath))
        {
            return;
        }
        List<string> fileMapping = new List<string>(mapping);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < fileMapping.Count; i++)
        {
            if (i == fileMapping.Count - 1)
            {
                sb.Append(fileMapping[i]);
            }
            else
            {
                sb.Append(fileMapping[i] + "\n");
            }
        }
        File.WriteAllText(assetsMappingFilePath,sb.ToString());
        AssetDatabase.Refresh();
        //设置映射文件
        AssetImporter assetImporter = AssetImporter.GetAtPath(assetsMappingFileImporterPath);
        if (assetImporter != null)
        {
            assetImporter.assetBundleName = "assetsmapping_bytes.assetbundle";
        }
        AssetDatabase.Refresh();
    }
    #endregion
}
