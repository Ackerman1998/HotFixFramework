using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class TestAssetBundle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Load());
    }
    IEnumerator Load() {
        string path = Path.Combine(Application.streamingAssetsPath, "Test", "AssetBundleTest");
        string writepath = Path.Combine(Application.streamingAssetsPath, "Test", "test.txt");
        WWW www = new WWW(path);
        yield return www;
        if (www.isDone)
        {
            AssetBundle ab = www.assetBundle;
            FileStream fs = File.Create(writepath);
            StringBuilder sb = new StringBuilder();
            AssetBundleManifest assetBundleManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            Debug.Log(assetBundleManifest.GetAllAssetBundles().Length);
            foreach (string str in assetBundleManifest.GetAllAssetBundles()) {
                sb.Append("**********************************************"+str +"\n");
                //print(str+"**********************************************");
                foreach (string depency in assetBundleManifest.GetAllDependencies(str)) {
                    sb.Append(str + " Depency:" + depency + "\n");
                    //print(str+" Depency:" + depency);
                }
            }
            fs.Write(Encoding.UTF8.GetBytes(sb.ToString()),0, sb.ToString().Length);
            fs.Flush();
            fs.Close();
        }
        else {
            Debug.LogError(www.error);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
