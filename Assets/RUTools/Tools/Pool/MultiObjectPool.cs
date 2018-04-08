using UnityEngine;
using System.Collections.Generic;
using RUT.Utilities;

namespace RUT.Tools.Pool
{
    /// <summary>
    /// Multi pool class. Used to handle multiple pools by id.
    /// </summary>
    [System.Serializable]
    public abstract class MultiObjectPool<T, U, V, W> where T : PoolByID<U, V, W> where W : IObjectPool
    {
        #region Public properties
        [SerializeField]
        protected T[] pools;

        public int TotalItems
        {
            get
            {
                int count = 0;
                for (int i = 0; i < pools.Length; ++i)
                    count += pools[i].Pool.TotalItems;
                return count;
            }
        }

        public int FreeItems
        {
            get
            {
                int count = 0;
                for (int i = 0; i < pools.Length; ++i)
                    count += pools[i].Pool.FreeItems;
                return count;
            }
        }

        public int UsedItems
        {
            get
            {
                int count = 0;
                for (int i = 0; i < pools.Length; ++i)
                    count += pools[i].Pool.UsedItems;
                return count;
            }
        }
        #endregion

        #region Private properties
        private Dictionary<V, IObjectPool> _poolByIDSet = new Dictionary<V, IObjectPool>();
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
            if (_initialised)
                return;

            for (int i = 0; i < pools.Length; ++i)
            {
                if (!pools[i].Pool.IsInitialised)
                {
                    pools[i].Pool.Initialise();

                    if (!_poolByIDSet.ContainsKey(pools[i].ID))
                        _poolByIDSet.Add(pools[i].ID, pools[i].Pool);
                    else
                        ULog.LogDebug(GetType().Name + ": duplicated id \"" + pools[i].ID + "\" detected.", ULog.Type.Warning);
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
                pools[i].Pool.Clear();

            _poolByIDSet.Clear();

            _initialised = false;
        }

        /// <summary>
        /// Resets all pools.
        /// </summary>
        public void DisposeAll()
        {
            if (!_initialised)
                return;

            for (int i = 0; i < pools.Length; ++i)
                pools[i].Pool.DisposeAll();
        }

        /// <summary>
        /// Takes the first free object from the right pool. If none are available, new ones will
        /// be instantiated.
        /// </summary>
        public IPoolable Take(V id)
        {
            if (!_initialised)
                return null;

            IPoolable instance = null;
            IObjectPool pool;

            if (_poolByIDSet.TryGetValue(id, out pool))
                instance = pool.Take();
            else
                ULog.LogDebug(GetType().Name + ": no pool associated to id \"" + id + "\" has been found.");

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
                if(pools[i].Pool.Recover(instance))
                    return true;
            }

            ULog.LogDebug(GetType().Name + ": " + instance.ToString() + " doesn't belong to any pool or is already recovered");

            return false;
        }
        #endregion

        #region Private methods
        #endregion

        #region SubType

        #endregion
    }

    [System.Serializable]
    public abstract class PoolByID<T, U, V> where V : IObjectPool
    {
        public abstract U ID { get; }
        public abstract V Pool { get; }
    }
}