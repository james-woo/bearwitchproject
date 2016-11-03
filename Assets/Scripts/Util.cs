using UnityEngine;
using System.Collections;

public class Util {

	public static void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;
        obj.layer = newLayer;

        foreach(Transform child in obj.transform)
        {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public static void SetTagRecursively(GameObject obj, string tag)
    {
        if (obj == null) return;
        obj.tag = tag;

        foreach(Transform child in obj.transform)
        {
            if (child == null) continue;
            SetTagRecursively(child.gameObject, tag);
        }
    }
}
