using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void OnLevelWa()
    {
        GameObject obj = AssetBundleManager.Instance.LoadAssets<GameObject>("Picture");
   
        Transform parent = GameObject.Find("Canvas").transform;
        Instantiate(obj, parent);
        GameObject obj2 = AssetBundleManager.Instance.LoadAssets<GameObject>("Bag");
        Instantiate(obj2, parent);
        GameObject obj3 = AssetBundleManager.Instance.LoadGameObject("Ball");

        Instantiate(obj3);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
