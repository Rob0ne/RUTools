using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RUT.Tools.Collision;
using RUT.Tools.Collision.Collector;

namespace RUT.Examples.Collision
{
    /// <summary>
    /// ExampleInitializer class.
    /// </summary>
    public class ExampleInitializer : MonoBehaviour
    {
        #region Public properties
        public Text colliderCollectorText;
        public Text targetCollectorText;
        [Space(5)]
        public ColliderCollector colliderCollector;
        public TargetCollector targetCollector;
        [Space(5)]
        public Rigidbody[] targetItems;
        public Rigidbody[] colliderItems;
        public Transform p1;
        public Transform p2;
        #endregion

        #region Private properties
        private int _colliderItems = -1;
        private int _targetItems = -1;

        private string _collectorInfoTemplate = "Items: {0}";
        private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
        #endregion

        #region Unity
        private void Start()
        {
            StartCoroutine(MoveItems(targetItems));
            StartCoroutine(MoveItems(colliderItems));
        }

        private void Update()
        {
            int collidersCount = colliderCollector.CollectedCount;
            int targetsCount = targetCollector.CollectedCount;

            if(_colliderItems != collidersCount)
            {
                _colliderItems = collidersCount;
                colliderCollectorText.text = string.Format(_collectorInfoTemplate, _colliderItems);
            }

            if (_targetItems != targetsCount)
            {
                _targetItems = targetsCount;
                targetCollectorText.text = string.Format(_collectorInfoTemplate, _targetItems);
            }
        }
        #endregion

        #region Private methods
        private IEnumerator MoveItems(Rigidbody[] items)
        {
            bool invert = false;
            while(true)
            {
                Vector3 newPos = invert ? p1.position : p2.position;

                for(int i = 0; i < items.Length; ++i)
                {
                    float time = Random.Range(0.5f, 2f);
                    Vector3 startPos = items[i].transform.position;
                    Vector3 endPos = new Vector3(startPos.x, startPos.y, newPos.z);

                    float endTime = Time.time + time;
                    while (Time.time < endTime)
                    {
                        float t = 1 - (endTime - Time.time) / time;
                        items[i].MovePosition(Vector3.Lerp(startPos, endPos, t));

                        yield return _waitForFixedUpdate;
                    }
                }

                invert = !invert;
            }
        }
        #endregion
    }
}