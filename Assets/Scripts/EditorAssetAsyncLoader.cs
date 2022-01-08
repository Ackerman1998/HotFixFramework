using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorAssetAsyncLoader : BaseAssetAsyncLoader
{
    public EditorAssetAsyncLoader(UnityEngine.Object obj) {
        asset = obj;
    }
    public override bool IsDone()
    {
        return true;
    }

    public override float Progress()
    {
        return 1;
    }

    public override void Update()
    {

    }
}
