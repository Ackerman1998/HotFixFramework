using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class FileTools 
{
    /// <summary>
    /// 拷贝文件夹所有文件
    /// </summary>
    public static void CopyFilePackage(string fileSource,string fileDestination) {
        if (!Directory.Exists(fileSource)) {
            return;
        }
        if (!Directory.Exists(fileDestination))
        {
            Directory.CreateDirectory(fileDestination);
           
        }
        string[] allFile = Directory.GetFiles(fileSource);
        foreach (string file in allFile) {
            string fileName = Path.GetFileName(file);
            string destName = Path.Combine(fileDestination,fileName);
            File.Copy(file,destName);
        }
        string[] allFilePackage = Directory.GetDirectories(fileSource);
        foreach (string package in allFilePackage)
        {
            string packName = Path.GetFileName(package);
            string destName = Path.Combine(fileDestination, packName);
            CopyFilePackage(package, destName.Replace(@"\",@"/"));
        }
    }
    /// <summary>
    /// 清空文件夹中的所有文件
    /// </summary>
    /// <param name="fileDestination"></param>
    public static void ClearFilePackage(string fileDestination) {
        if (Directory.Exists(fileDestination))
        {
            string[] allFile = Directory.GetFiles(fileDestination);
            foreach (string file in allFile)
            {
                File.Delete(file);
            }

            string[] allFilePackage = Directory.GetDirectories(fileDestination);
            foreach (string package in allFilePackage)
            {
                ClearFilePackage(package);
            }
        }
    }
    /// <summary>
    /// 修改文件夹下所有文件的后缀名
    /// </summary>
    public static void AlterAllFileEndNameByDirectory(string destinationPath,string endName) {
        
        if (!Directory.Exists(destinationPath))
        {
         
            return;
        }
        string[] allFile = Directory.GetFiles(destinationPath);
        foreach (string file in allFile)
        {
           
            Debug.Log(endName+"!Directory.Exists(destinationPath)" + file);
            Path.ChangeExtension(file.ReplaceSpecial(),endName);
        }
        string[] allFilePackage = Directory.GetDirectories(destinationPath);
        foreach (string package in allFilePackage)
        {
            AlterAllFileEndNameByDirectory(package, endName);
        }
    }
    /// <summary>
    /// 写入文本（覆盖写入）
    /// </summary>
    /// <param name="path"></param>
    /// <param name="content"></param>
    public static void WriteText(string path,string content) {
        path=path.CheckEnvironmentPath();
        path=path.ReplaceSpecial();
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else {
            FileStream ff = File.Create(path);
            byte[] buff = System.Text.Encoding.UTF8.GetBytes(content);
            ff.Write(buff, 0, buff.Length);
            return;
        }
        FileStream fs = File.Create(path);
        byte [] buffer = System.Text.Encoding.UTF8.GetBytes(content);
        fs.Write(buffer,0, buffer.Length);
    }
    public static bool CheckContentExist(string content) {
        if (Directory.Exists(content)) {
            return true;
        }
        Debug.LogError("Content : ["+content+"] not exist...");
        return false;
    }
    /// <summary>
    /// check rootcontent all files md5 value 
    /// </summary>
    public static void GetAllFileMD5(string rootContent) {
        if (!CheckContentExist(rootContent)) {
            return;
        }

    }
}
