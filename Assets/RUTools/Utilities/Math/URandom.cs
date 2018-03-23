using UnityEngine;

namespace RUT.Utilities
{
    /// <summary>
    /// Random utilities.
    /// </summary>
    public static class URandom
    {
        public static int[] IntArray(int count, int min = 0, int max = int.MaxValue)
        {
            int[] result = new int[count];

            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = Random.Range(min, max);
            }

            return result;
        }

        public static Vector3 Range(Vector3 min, Vector3 max)
        {
            Vector3 result = new Vector3();
            result.x = Random.Range(min.x, max.x);
            result.y = Random.Range(min.y, max.y);
            result.z = Random.Range(min.z, max.z);

            return result;
        }
    }
}