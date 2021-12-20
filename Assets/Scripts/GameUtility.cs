using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// 工具类
/// </summary>
public class GameUtility
{
    /// <summary>
    /// 写入Text
    /// </summary>
    /// <param name="outputPath"></param>
    /// <param name="content"></param>
    public static void SafeWriteAllText(string outputPath, string content)
    {
        if (string.IsNullOrEmpty(outputPath))
        {
            Debug.LogError("outputPath is null:" + outputPath);
            return;
        }
        CheckFileAndDirectory(outputPath);
        File.WriteAllText(outputPath, content);
    }

    public static bool SafeWriteAllLines(string outFile, string[] outLines)
    {
        try
        {
            if (string.IsNullOrEmpty(outFile))
            {
                return false;
            }

            CheckFileAndDirectory(outFile);
            if (File.Exists(outFile))
            {
                File.SetAttributes(outFile, FileAttributes.Normal);
            }
            File.WriteAllLines(outFile, outLines);
            return true;
        }
        catch (System.Exception ex)
        {
            return false;
        }
    }

    public static void SafeDeleteDir(string targetPath)
    {
        if (string.IsNullOrEmpty(targetPath))
        {
            return ;
        }
        if (Directory.Exists(targetPath))
        {
            DeleteDirectory(targetPath);
        }
    }

    public static void DeleteDirectory(string dirPath)
    {
        string[] files = Directory.GetFiles(dirPath);
        string[] dirs = Directory.GetDirectories(dirPath);

        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        foreach (string dir in dirs)
        {
            DeleteDirectory(dir);
        }

        Directory.Delete(dirPath, false);
    }

    public static void SafeRenameFile(string sourceFileName, string destFileName)
    {
        if (string.IsNullOrEmpty(sourceFileName))
        {
            return;
        }

        if (!File.Exists(sourceFileName))
        {
            return;
        }
        SafeDeleteFile(destFileName);
        File.SetAttributes(sourceFileName, FileAttributes.Normal);
        File.Move(sourceFileName, destFileName);
    }

    public static void SafeDeleteFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }
        if (!File.Exists(filePath))
        {
            return;
        }
        File.SetAttributes(filePath, FileAttributes.Normal);
        File.Delete(filePath);
    }

    public static void CheckFileAndDirectory(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }
        FileInfo file_info = new FileInfo(filePath);
        DirectoryInfo dir_info = file_info.Directory;
        if (!dir_info.Exists)
        {
            Directory.CreateDirectory(dir_info.FullName);
        }
    }

    public static string[] GetTargetFilesInFolder(string path, string[] extensions = null, bool exclude = false)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        if (extensions == null)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        }
        else if (exclude)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(f => !extensions.Contains(GetFileExtension(f))).ToArray();
        }
        else
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(f => extensions.Contains(GetFileExtension(f))).ToArray();
        }
    }
    public static string GetFileExtension(string path)
    {
        return Path.GetExtension(path).ToLower();
    }

    public static string FormatToUnityPath(string path)
    {
        return path.Replace("\\", "/");
    }
}


