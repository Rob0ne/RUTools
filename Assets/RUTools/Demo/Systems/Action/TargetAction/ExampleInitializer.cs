using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RUT.Examples.TargetAction
{
    /// <summary>
    /// ExampleInitializer class.
    /// </summary>
    public class ExampleInitializer : MonoBehaviour
    {
        #region Public properties
        public Text targetText;
        [Space(5)]
        public TargetActionExample1 targetAction;
        public Transform p1;
        public Transform p2;
        #endregion

        #region Private properties
        private float _repeatTime = 3;
        private float _travelTime = 1f;

        private int _targetCount = -1;
        private string _collectorInfoTemplate = "Current Targets: {0}";
        #endregion

        #region Unity
        private void Start()
        {
            StartCoroutine(LaunchActionProcess());
        }

        private void Update()
        {
            if(targetAction.LastTargetCount != _targetCount)
            {
                _targetCount = targetAction.LastTargetCount;
                targetText.text = string.Format(_collectorInfoTemplate, _targetCount);
            }
        }
        #endregion

        #region Private methods
        private IEnumerator LaunchActionProcess()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(_repeatTime);
            WaitForFixedUpdate waitforFixedUpdate = new WaitForFixedUpdate();

            while (true)
            {
                targetAction.transform.position = p1.position;

                yield return waitforFixedUpdate;

                targetAction.Use(this);

                float endTime = Time.time + _travelTime;
                while (Time.time < endTime)
                {
                    float t = 1 - (endTime - Time.time) / _travelTime;
                    targetAction.MovePosition(Vector3.Lerp(p1.position, p2.position, t));

                    yield return waitforFixedUpdate;
                }

                targetAction.transform.position = p2.position;

                targetAction.Stop();

                yield return waitForSeconds;
            }
        }
        #endregion
    }
}