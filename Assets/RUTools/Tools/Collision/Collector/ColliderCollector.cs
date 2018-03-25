using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace RUT.Tools.Collision.Collector
{
    /// <summary>
    /// Collider collector. Get all colliders inside a collider.
    /// </summary>
    public class ColliderCollector : Collector
    {
        #region Public properties
        public Collider[] Collection
        {
            get
            {
                //Update pending data only when asked.
                if (_checkRequired)
                {
                    UpdatePending();
                    CleanLists();

                    _checkRequired = false;
                }
                return _collectionSet.ToArray();
            }
        }

        public int CollectedCount
        {
            get
            {
                if (_checkRequired)
                {
                    UpdatePending();
                    CleanLists();

                    _checkRequired = false;
                }
                return _collectionSet.Count;
            }
        }
        #endregion

        #region Private properties
        private bool _checkRequired = true;

        //Contains every collected colliders.
        private HashSet<Collider> _collectionSet = new HashSet<Collider>();
        //Contains every collected colliders marked as volatile.
        private HashSet<Collider> _volatileSet = new HashSet<Collider>();
        //Contains every colliders awaiting for a place in main collection.
        private List<Collider> _pendingList = new List<Collider>();
        #endregion

        #region Unity
        protected override void Update()
        {
            //Override default behaviour to cancel data updates on every frame.
        }
        #endregion

        #region Private methods
        protected override void AddObject(Collider collider, bool isVolatile)
        {
            //If max capacity has been reached add collider to pending collection.
            if (IsAtMaxCapacity(_collectionSet.Count))
                _pendingList.Add(collider);
            //If max capacity has not been reached add collider to main collection.
            else
                _collectionSet.Add(collider);

            //If collider is volatile add it to volatile collection.
            if (isVolatile)
                _volatileSet.Add(collider);
        }
        protected override void RemoveObject(Collider collider)
        {
            _collectionSet.Remove(collider);
            _volatileSet.Remove(collider);
            _pendingList.Remove(collider);
        }
        protected override void UpdatePending()
        {
            if (_pendingList.Count > 0)
            {
                //While there is space in main collection, transfer elements from pending to main.
                //FIFO context.
                while (!IsAtMaxCapacity(_collectionSet.Count) && _pendingList.Count > 0)
                {
                    Collider collider = _pendingList[0];

                    _collectionSet.Add(collider);
                    _pendingList.RemoveAt(0);
                }
            }
        }
        protected override void CleanLists()
        {
            if (_volatileSet.Count > 0)
            {
                //Remove every deprecated volatile colliders.
                _volatileSet.RemoveWhere(VolatileCheckAndRemove);
            }
        }
        protected override void ClearLists()
        {
            _collectionSet.Clear();
            _volatileSet.Clear();
            _pendingList.Clear();
        }

        /// <summary>
        /// Checks if volatile colliders should be removed from collections.
        /// (Destroyed, deactivated or layer changed).
        /// </summary>
        private bool VolatileCheckAndRemove(Collider collider)
        {
            bool remove = false;
            if (collider != null)
            {
                int collectable = (1 << collider.gameObject.layer) & settings.collectableLayer.value;
                remove = collider.gameObject.activeInHierarchy ? collectable <= 0 : true;
            }
            else
                remove = true;

            if (remove)
            {
                _collectionSet.Remove(collider);
                _pendingList.Remove(collider);
            }
            return remove;
        }
        #endregion
    }
}