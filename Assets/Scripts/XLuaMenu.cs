using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;


public static class XLuaMenu
{


    public const string luaSourcePathName = "HotfixScripts";
    public const string luaTargetPathName = "Scripts";
    public const string AssetPackagePathName = "AssetPackage";
#if UNITY_EDITOR

    /// <summary>
    /// 从资源包中拷贝lua脚本到指定目录
    /// </summary>
    public static void CopyLuaScriptsToPackage() {
        string targetPath = Path.Combine(Application.dataPath, AssetPackagePathName, luaTargetPathName);
        string sourcePath = Path.Combine(Application.dataPath, luaSourcePathName);
        GameUtility.SafeDeleteDir(targetPath);
        FileUtil.CopyFileOrDirectoryFollowSymlinks(sourcePath, targetPath);
        //删除非lua文件
        var notLuaFiles = GameUtility.GetTargetFilesInFolder(targetPath, new string[] { ".lua" }, true);
        if (notLuaFiles != null && notLuaFiles.Length > 0)
        {
            for (int i = 0; i < notLuaFiles.Length; i++)
            {
                GameUtility.SafeDeleteFile(notLuaFiles[i]);
            }
        }
        //对lua文件进行重命名
        var luaFiles = GameUtility.GetTargetFilesInFolder(targetPath, new string[] { ".lua" }, false);
        if (luaFiles != null && luaFiles.Length > 0)
        {
            for (int i = 0; i < luaFiles.Length; i++)
            {
                GameUtility.SafeRenameFile(luaFiles[i], luaFiles[i] + ".bytes");
            }
        }
        AssetDatabase.Refresh();
    }

    public static void GenLuaBytesFile()
    {
        string targetPath = Path.Combine(Application.dataPath, AssetPackagePathName, luaTargetPathName);
        string sourcePath = Path.Combine(Application.dataPath, luaSourcePathName);
        GameUtility.SafeDeleteDir(targetPath);
        FileUtil.CopyFileOrDirectoryFollowSymlinks(sourcePath, targetPath);
        //删除非lua文件
        var notLuaFiles = GameUtility.GetTargetFilesInFolder(targetPath, new string[] { ".lua" }, true);
        if (notLuaFiles != null && notLuaFiles.Length > 0)
        {
            for (int i = 0; i < notLuaFiles.Length; i++)
            {
                GameUtility.SafeDeleteFile(notLuaFiles[i]);
            }
        }
        //对lua文件进行重命名
        var luaFiles = GameUtility.GetTargetFilesInFolder(targetPath, new string[] { ".lua" }, false);
        if (luaFiles != null && luaFiles.Length > 0)
        {
            for (int i = 0; i < luaFiles.Length; i++)
            {
                GameUtility.SafeRenameFile(luaFiles[i], luaFiles[i] + ".bytes");
            }
        }
        AssetDatabase.Refresh();
    }
#endif
}
