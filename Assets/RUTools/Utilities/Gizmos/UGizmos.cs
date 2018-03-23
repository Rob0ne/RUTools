using UnityEngine;

namespace RUT.Utilities
{
    /// <summary>
    /// Gizmos utilities.
    /// </summary>
    public static class UGizmos
    {
        public static void DrawCicle(Vector3 center, Quaternion rotation, int segments, float radius)
        {
            float x = 0;
            float y = 0;
            float z = radius;

            Vector3 fromPos = rotation * new Vector3(x, y, z) + center;

            float addAngle = (360f / segments);
            float angle = addAngle;

            for (int i = 1; i < (segments + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
                z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

                Vector3 toPos = rotation * new Vector3(x, y, z) + center;

                Gizmos.DrawLine(fromPos, toPos);
                fromPos = toPos;

                angle += (360f / segments);
            }
        }
    }
}