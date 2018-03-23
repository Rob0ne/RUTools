using UnityEngine;

namespace RUT.Tools.Singleton
{
    /// <summary>
    /// Monobehaviour singleton class.
    /// </summary>
    public abstract class MBSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        #region Public properties
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("_" + typeof(T).ToString() + "_DEFAULT_");
                        _instance = obj.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        public static bool IsInstanceNull
        {
            get { return _instance == null; }
        }

        /// <summary>
        /// False by default, object will be destroyed automatically on scene change. Once set to true, cannot be undone.
        /// </summary>
        public bool IsPersistent
        {
            get { return _isPersistent; }
            set
            {
                if (_isPersistent)
                    return;

                if (value)
                {
                    DontDestroyOnLoad(gameObject);
                    _isPersistent = true;
                }
            }
        }
        #endregion

        #region Private properties
        private static T _instance = null;
        private bool _isPersistent = false;
        #endregion

        #region Unity
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if(_instance != this)
            {
                Destroy(gameObject);	//Destroy the gameobject
                //Destroy(this);		//Destroy the component
            }
        }
        #endregion
    }
}