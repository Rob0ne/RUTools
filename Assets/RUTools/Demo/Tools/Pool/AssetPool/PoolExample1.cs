using System.Collections.Generic;
using UnityEngine;
using RUT.Tools.Pool;
using RUT.Utilities.Identification;

namespace RUT.Examples.Pool.Ex2
{
    /// <summary>
    /// PoolExample1 class.
    /// </summary>
    public class PoolExample1 : MonoBehaviour
    {
        #region Public properties
        public AssetPool pool;
        public MultiAssetPoolByStringIDAsset multiPool;
        [Space(5)]
        public StringID multiPoolItem1ID;
        public StringID multiPoolItem2ID;
        [Space(5)]
        public Transform spawn1Pos;
        public Transform spawn2Pos;
        public Transform spawn3Pos;
        #endregion

        #region Private properties
        private Queue<IPoolable> _poolQueue = new Queue<IPoolable>();
        private Dictionary<string, Queue<IPoolable>> _multiPoolQueues = new Dictionary<string, Queue<IPoolable>>();
        #endregion

        #region API
        public void SpawnCube1()
        { SpawnFromPool(pool, spawn1Pos.position); }

        public void SpawnCube2()
        { SpawnFromMultiPool(multiPool, multiPoolItem1ID.ID, spawn2Pos.position); }

        public void SpawnCube3()
        { SpawnFromMultiPool(multiPool, multiPoolItem2ID.ID, spawn3Pos.position); }

        public void DisposeCube1()
        { DisposeOneFromPool(); }

        public void DisposeCube2()
        { DisposeOneFromMultiPool(multiPoolItem1ID.ID); }

        public void DisposeCube3()
        { DisposeOneFromMultiPool(multiPoolItem2ID.ID); }

        public void DisposeAllFromPool()
        { DisposeAllFromPool(pool); }

        public void DisposeAllFromMultiPool()
        { DisposeAllFromMultiPool(multiPool); }
        #endregion

        #region Unity
        private void Awake()
        {
            pool.Initialise();
            multiPool.Initialise();
        }

        private void OnDestroy()
        {
            //The multipool being an scriptable object needs to be cleared manually. If you don't
            //do this, the object will keep all the current data. It might be a behaviour you want,
            //aka persistant pool, but if you don't, clear it manually or use an instance of the
            //scriptable object instead.
            multiPool.Clear();
        }
        #endregion

        #region Private methods
        private void SpawnFromPool(IObjectPool pool, Vector3 position)
        {
            //Take an item from the pool. The return item can be null if all pool's items are
            //already used and the configuration doesn't allow any expansion.
            IPoolable poolable = pool.Take();
            //Cast the IPoolable into a Component, this will work because I am only using
            //PoolableMBObject and PoolableComponent scripts in the pool and both derive from
            //MonoBehaviour.
            Component component = poolable as Component;
            if (component != null)
            {
                component.gameObject.SetActive(true); ;
                component.transform.position = position;

                _poolQueue.Enqueue(poolable);
            }
        }

        private void SpawnFromMultiPool(IMultiObjectPool<string> multiPool, string id, Vector3 position)
        {
            IPoolable poolable = multiPool.Take(id);
            Component component = poolable as Component;
            if (component != null)
            {
                component.gameObject.SetActive(true); ;
                component.transform.position = position;

                Queue<IPoolable> queue;
                if (!_multiPoolQueues.TryGetValue(id, out queue))
                {
                    queue = new Queue<IPoolable>();
                    _multiPoolQueues[id] = queue;
                }

                _multiPoolQueues[id].Enqueue(poolable);
            }
        }

        private void DisposeOneFromPool()
        {
            if (_poolQueue.Count > 0)
            {
                _poolQueue.Dequeue().Dispose();
            }
        }

        private void DisposeOneFromMultiPool(string id)
        {
            Queue<IPoolable> queue;
            if (_multiPoolQueues.TryGetValue(id, out queue))
            {
                if (queue.Count > 0)
                {
                    queue.Dequeue().Dispose();
                }
            }
        }

        private void DisposeAllFromPool(AssetPool pool)
        {
            pool.DisposeAll();
            _poolQueue.Clear();
        }

        private void DisposeAllFromMultiPool(IMultiObjectPool<string> multiPool)
        {
            multiPool.DisposeAll();
            _multiPoolQueues.Clear();
        }
        #endregion
    }
}