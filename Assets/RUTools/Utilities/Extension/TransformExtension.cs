using UnityEngine;
using System.Collections;

/// <summary>
/// TransformExtension class.
/// </summary>
public static class TransformExtension
{
    public static Transform FindDeepChild(this Transform parent, string name)
    {
        var result = parent.Find(name);
        if (result != null)
            return result;
        foreach(Transform child in parent)
        {
            result = child.FindDeepChild(name);
            if (result != null)
                return result;
        }
        return null;
    }


    public static void DrawHierarchy(this Transform t)
    { DrawHierarchyInternal(t, Color.white);}
    public static void DrawHierarchy(this Transform t, Color col)
    { DrawHierarchyInternal(t, col);}

    private static void DrawHierarchyInternal(Transform t, Color col)
    {
        if (t == null)
            return;

        for (int i = 0; i < t.childCount; ++i)
        {
            Transform child = t.GetChild (i);
            Debug.DrawLine (t.position, child.position, col);
            DrawHierarchyInternal(child, col);
        }
    }
}
