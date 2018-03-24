using System.Collections.Generic;
using System;
using UnityEngine;

namespace RUT.Tools.Collision.Collector
{
    /// <summary>
    /// Collector class.
    /// </summary>
    public abstract class Collector : MonoBehaviour
    {
        #region Public properties
        public Settings settings = new Settings(-1,0,0);
        #endregion

        #region Unity
        protected virtual void Update()
        {
            UpdatePending();
            CleanLists();
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            Transform targetTransform = collider.gameObject.transform;

            //Check if target is collectable.
            int targetLayer = collider.gameObject.layer;
            int collision = (1 << targetLayer) & settings.collectableLayer.value;

            if (collision > 0)
            {
                //Check if target is on the ignore list.
                for (int i = 0; i < settings.ignoredTargets.Count; ++i)
                {
                    if (settings.ignoredTargets[i] == null)
                        continue;

                    if (collider.gameObject == settings.ignoredTargets[i])
                        return;

                    if (settings.ignoreChildren && targetTransform.IsChildOf(settings.ignoredTargets[i]))
                        return;
                }

                //Check if target is volatile.
                bool isVolatile = Convert.ToBoolean((1 << targetLayer) & settings.volatileLayer.value);

                AddObject(collider, isVolatile);
            }

        }

        protected virtual void OnTriggerExit(Collider collider)
        {
            //Check if target is collectable
            int collision = (1 << collider.gameObject.layer) & settings.collectableLayer.value;
            if (collision > 0)
            {
                RemoveObject(collider);
            }
        }

        protected virtual void OnDisable()
        {
            ClearLists();
        }
        #endregion

        #region Private methods
        protected bool IsAtMaxCapacity(int count)
        {
            if (settings.maxCapacity >= 0)
            {
                if (count >= settings.maxCapacity)
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual void AddObject(Collider collider, bool isVolatile)
        {
        }
        protected virtual void RemoveObject(Collider collider)
        {
        }
        protected virtual void UpdatePending()
        {
        }
        protected virtual void CleanLists()
        {
        }
        protected virtual void ClearLists()
        {
        }
        #endregion

        #region SubType
        [System.Serializable]
        public struct Settings
        {
            //Maximum number of collected targets. If there are more targets than maxCapacity
            //inside the collector, they are queued in a pending list and are swapped to the
            //target list whenever there is space.
            public int maxCapacity;
            //Layers of collected objects.
            public LayerMask collectableLayer;
            //Layers of volatile objects. Objects that could be destroyed or have their layer
            //changed while inside the collector.
            public LayerMask volatileLayer;

            [Space(5)]

            //List of ignored targets.
            public List<Transform> ignoredTargets;
            //Set to True if all children of every ignoredTargets should also be ignored.
            public bool ignoreChildren;

            public Settings(int mc, int tl, int vl)
            {
                maxCapacity = mc;
                collectableLayer = tl;
                volatileLayer = vl;
                ignoredTargets = new List<Transform>();
                ignoreChildren = false;
            }
        }
        #endregion
    }
}