using UnityEngine;

namespace RUT.Tools.Utilities
{
    /// <summary>
    /// Math utilities.
    /// </summary>
    public static class UMath
    {
        /// <summary>
        /// Safe division. "a" divided by "b"
        /// </summary>
        public static float SafeDivide(float a, float b, float safeReturn)
        {
            return b == 0 ? safeReturn : a / b;
        }
        public static Vector2 SafeDivide(Vector2 a, float b, Vector2 safeReturn)
        {
            return b == 0 ? safeReturn : a / b;
        }
        public static Vector3 SafeDivide(Vector3 a, float b, Vector3 safeReturn)
        {
            return b == 0 ? safeReturn : a / b;
        }
    }
}