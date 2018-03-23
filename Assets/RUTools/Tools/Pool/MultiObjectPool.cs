using UnityEngine;
using System.Collections.Generic;
using RUT.Tools.Utilities;

namespace RUT.Tools.Pool
{
    /// <summary>
    /// Multi pool class. Used to handle multiple pools by id.
    /// </summary>
    [System.Serializable]
    public class MultiObjectPool
    {
        #region Public properties
        [SerializeField] PoolByID[] pools;

        public int TotalItems
        {
            get
            {
                int count = 0;
                for (int i = 0; i < pools.Length; ++i)
                    count += pools[i].pool.TotalItems;
                return count;
            }
        }

        public int FreeItems
        {
            get
            {
                int count = 0;
                for (int i = 0; i < pools.Length; ++i)
                    count += pools[i].pool.FreeItems;
                return count;
            }
        }

        public int UsedItems
        {
            get
            {
                int count = 0;
                for (int i = 0; i < pools.Length; ++i)
                    count += pools[i].pool.UsedItems;
                return count;
            }
        }
        #endregion

        #region Private properties
        private Dictionary<string, ObjectPool> _poolByIDSet = new Dictionary<string, ObjectPool>();
        private bool _initialised = false;
        #endregion

        #region Constructor
        #endregion

        #region API
        /// <summary>
        /// Initialises all pools.
        /// </summary>
        public void Initialise()
        {
            for (int i = 0; i < pools.Length; ++i)
            {
                if (!pools[i].pool.IsInitialised)
                {
                    pools[i].pool.Initialise();

                    if (!_poolByIDSet.ContainsKey(pools[i].id))
                        _poolByIDSet.Add(pools[i].id, pools[i].pool);
                    else
                        ULog.LogDebug("MultiObjectPool: duplicated id \"" + pools[i].id + "\" detected.", ULog.Type.Warning);
                }
            }

            _initialised = true;
        }

        /// <summary>
        /// Clears all pools.
        /// </summary>
        public void Clear()
        {
            if (!_initialised)
                return;

            for (int i = 0; i < pools.Length; ++i)
                pools[i].pool.Clear();
        }

        /// <summary>
        /// Resets all pools.
        /// </summary>
        public void DisposeAll()
        {
            if (!_initialised)
                return;

            for (int i = 0; i < pools.Length; ++i)
                pools[i].pool.DisposeAll();
        }

        /// <summary>
        /// Takes the first free object from the right pool. If none are available, new ones will
        /// be instantiated.
        /// </summary>
        public IPoolable Take(string id)
        {
            if (!_initialised)
                return null;

            IPoolable instance = null;
            ObjectPool pool;

            if (_poolByIDSet.TryGetValue(id, out pool))
                instance = pool.Take();
            else
                ULog.LogDebug("MultiObjectPool: no pool associated to id \"" + id + "\" has been found.");

            return instance;
        }

        /// <summary>
        /// Recovers a taken object.
        /// </summary>
        public bool Recover(IPoolable instance)
        {
            if (!_initialised || instance == null)
                return false;

            for (int i = 0; i < pools.Length; ++i)
            {
                if(pools[i].pool.Recover(instance))
                    return true;
            }

            ULog.LogDebug("MultiObjectPool: " + instance.ToString() + " doesn't belong to any pool or is already recovered");

            return false;
        }
        #endregion

        #region Private methods
        #endregion

        #region SubType
        [System.Serializable]
        public struct PoolByID
        {
            [ReadOnlyRuntime] public string id;
            public ObjectPool pool;
        }
        #endregion
    }
}