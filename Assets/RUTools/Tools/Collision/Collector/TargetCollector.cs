using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RUT.Tools.Collision.Collector
{
    /// <summary>
    /// ITargetable collector. Get all ITargetable inside a collider.
    /// </summary>
    public class TargetCollector : Collector
    {
        #region Public properties
        public ITargetable[] Collection
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
                return _collectionSet.Keys.ToArray();
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

        //Contains final targets as key and a collection of all parts pointing toward it as value.
        private Dictionary<ITargetable, HashSet<ITargetable>> _collectionSet = new Dictionary<ITargetable, HashSet<ITargetable>>();
        //Contains all volatile parts and their own gameobject as value.
        private Dictionary<ITargetable, GameObject> _volatileSet = new Dictionary<ITargetable, GameObject>();
        //Contains final targets, as key, awaiting for a place in the main collection and their parts as value.
        private Dictionary<ITargetable, HashSet<ITargetable>> _pendingSet = new Dictionary<ITargetable, HashSet<ITargetable>>();
        #endregion

        #region API
        /// <summary>
        /// Get every parts of a collected target.
        /// </summary>
        public List<ITargetable> GetPartsOf(ITargetable target)
        {
            HashSet<ITargetable> partsSet;
            _collectionSet.TryGetValue(target, out partsSet);

            return partsSet.ToList();
        }
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
            //Get part's ITargetable component.
            ITargetable part = collider.GetComponent<ITargetable>();
            if (part == null)
                return;

            //Get part's target.
            ITargetable target = part.Target;
            if (target == null)
                return;

            HashSet<ITargetable> partsSet;

            //If target already in main collection, add its part.
            if (_collectionSet.TryGetValue(target, out partsSet))
            {
                partsSet.Add(part);
            }
            //If final target is not in main collection and max capacity has been reached,
            //add part and target to pending collection.
            else if (IsAtMaxCapacity(_collectionSet.Count))
            {
                if (_pendingSet.TryGetValue(target, out partsSet))
                {
                    partsSet.Add(part);
                }
                else
                {
                    partsSet = new HashSet<ITargetable>();
                    partsSet.Add(part);

                    _pendingSet.Add(target, partsSet);
                }
            }
            //If final target is not in main collection and max capacity has not been reached,
            //add part and target to main collection.
            else
            {
                partsSet = new HashSet<ITargetable>();
                partsSet.Add(part);

                _collectionSet.Add(target, partsSet);
            }

            //If part is volatile, add it to volatile collection.
            if (isVolatile)
            {
                _volatileSet.Add(part, collider.gameObject);
            }
        }
        protected override void RemoveObject(Collider collider)
        {
            //Get part's ITargetable component.
            ITargetable part = collider.GetComponent<ITargetable>();
            if (part == null)
                return;

            //Get part's target.
            ITargetable target = part.Target;
            if (target == null)
                return;

            HashSet<ITargetable> partsSet;

            //Try remove part from main collection.
            if (_collectionSet.TryGetValue(target, out partsSet))
            {
                partsSet.Remove(part);
                if (partsSet.Count == 0)
                    _collectionSet.Remove(target);
            }
            //If part is not in main collection, it is in pending.
            else if (_pendingSet.TryGetValue(target, out partsSet))
            {
                partsSet.Remove(part);
                if (partsSet.Count == 0)
                    _pendingSet.Remove(target);
            }

            //Try remove part from volatile collection.
            _volatileSet.Remove(part);
        }
        protected override void UpdatePending()
        {
            if (_pendingSet.Count > 0)
            {
                //While there is space in main collection, transfer elements from pending to main.
                //FIFO context.
                while (!IsAtMaxCapacity(_collectionSet.Count) && _pendingSet.Count > 0)
                {
                    KeyValuePair<ITargetable, HashSet<ITargetable>> element = _pendingSet.ElementAt(0);

                    _collectionSet.Add(element.Key, element.Value);
                    _pendingSet.Remove(element.Key);
                }
            }
        }
        protected override void CleanLists()
        {
            if (_volatileSet.Count > 0)
            {
                //Get all volatile parts that shouldn't be collected anymore.
                List<KeyValuePair<ITargetable, GameObject>> deprecatedList = _volatileSet.Where(VolatileCheck).ToList();

                if (deprecatedList.Count() > 0)
                {
                    //Remove all deprecated volatile parts from every collections.
                    for (int i = 0; i < deprecatedList.Count; ++i)
                    {
                        HashSet<ITargetable> partsSet;

                        if (_collectionSet.TryGetValue(deprecatedList[i].Key.Target, out partsSet))
                        {
                            partsSet.Remove(deprecatedList[i].Key);
                            if (partsSet.Count == 0)
                                _collectionSet.Remove(deprecatedList[i].Key.Target);
                        }
                        else if (_pendingSet.TryGetValue(deprecatedList[i].Key.Target, out partsSet))
                        {
                            partsSet.Remove(deprecatedList[i].Key);
                            if (partsSet.Count == 0)
                                _pendingSet.Remove(deprecatedList[i].Key.Target);
                        }

                        _volatileSet.Remove(deprecatedList[i].Key);
                    }
                }
            }
        }
        protected override void ClearLists()
        {
            _collectionSet.Clear();
            _volatileSet.Clear();
            _pendingSet.Clear();
        }

        /// <summary>
        /// Checks if volatile parts should be removed from collections.
        /// (Destroyed, deactivated or layer changed).
        /// </summary>
        private bool VolatileCheck(KeyValuePair<ITargetable, GameObject> keyValue)
        {
            bool remove = false;
            if (keyValue.Value != null)
            {
                int collectable = (1 << keyValue.Value.layer) & settings.collectableLayer.value;
                remove = keyValue.Value.activeInHierarchy ? collectable <= 0 : true;
            }
            else
                remove = true;

            return remove;
        }
        #endregion
    }
}