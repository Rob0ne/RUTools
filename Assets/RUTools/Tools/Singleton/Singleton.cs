namespace RUT.Tools.Singleton
{
    /// <summary>
    /// Singleton class.
    /// </summary>
    public abstract class Singleton<T>
        where T : class, new()
    {
        #region Public properties
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new T();

                return _instance;
            }
        }

        public static bool IsInstanceNull
        {
            get { return _instance == null; }
        }
        #endregion

        #region Private properties
        private static T _instance = null;
        #endregion

        #region Private methods
        protected Singleton()
        {
            if (_instance == null)
                _instance = this as T;
        }
        #endregion
    }
}