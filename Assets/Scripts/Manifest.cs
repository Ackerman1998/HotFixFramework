using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 存放AssetbundleManifest
/// </summary>
public class Manifest 
{
    const string assetName = "AssetBundleManifest";
    AssetBundleManifest manifest = null;
    byte[] manifestBytes = null;
    string[] emptyStringArray = new string[] { };

    public Manifest()
    {
        AssetbundleName = AssetBundleManager.Instance.StreamingRootFolderName;
    }
    public AssetBundleManifest assetbundleManifest
    {
        get
        {
            return manifest;
        }
    }

    public string AssetbundleName
    {
        get;
        protected set;
    }

    public int Length
    {
        get
        {
            return manifest == null ? 0 : manifest.GetAllAssetBundles().Length;
        }
    }

    public void LoadFromAssetbundle(AssetBundle assetbundle)
    {
        if (assetbundle == null)
        {
            return;
        }
        manifest = assetbundle.LoadAsset<AssetBundleManifest>(assetName);
        //foreach (string nn in manifest.GetAllAssetBundles()) {
        //    Debug.Log(nn);
        //}
        //foreach (string nn in manifest.GetAllAssetBundles())
        //{
        //    Debug.Log("ab::::::::::"+nn);
        //    foreach (string xx in manifest.GetAllDependencies(nn))
        //    {
        //        Debug.Log(xx);
        //    }
        //}
       
    }
    public string[] GetAllAssetBundleNames() {
        return manifest.GetAllAssetBundles();
    }  
    public string[] GetAllDependencies(string assetbundleName) {
        return manifest.GetAllDependencies(assetbundleName);
    }
    
}
