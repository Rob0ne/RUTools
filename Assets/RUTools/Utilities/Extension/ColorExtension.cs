using UnityEngine;

/// <summary>
/// ColorExtension class.
/// </summary>
public static class ColorExtension
{
    public static Vector4 ColorToVector(this Color c)
    {
        return new Vector4 (c.r, c.g, c.b, c.a);
    }
    public static Color VectorToColor(this Vector4 v)
    {
        return new Color (v.x, v.y, v.z, v.w);
    }
}
