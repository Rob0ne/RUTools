namespace RUT.Utilities
{
    /// <summary>
    /// Log Utilities.
    /// </summary>
    public static class ULog
    {
        public static void Log(object msg)
        { LogInternal(msg, Type.Log); }
        public static void Log(object msg, Type type)
        { LogInternal(msg, type); }

        [System.Diagnostics.Conditional("DEBUG"), System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogDebug(object msg)
        { LogInternal(msg, Type.Log); }
        [System.Diagnostics.Conditional("DEBUG"), System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogDebug(object msg, Type type)
        { LogInternal(msg, type); }

        private static void LogInternal(object msg, Type type)
        {
            switch (type)
            {
                case Type.Warning:
                    UnityEngine.Debug.LogWarning(msg);
                    break;
                case Type.Error:
                    UnityEngine.Debug.LogError(msg);
                    break;

                default:
                    UnityEngine.Debug.Log(msg);
                    break;
            }
        }

        public enum Type
        {
            Log,
            Warning,
            Error,
        }
    }
}