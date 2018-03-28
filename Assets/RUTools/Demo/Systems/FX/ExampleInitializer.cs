using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RUT.Utilities;

namespace RUT.Examples.FXController
{
    /// <summary>
    /// ExampleInitializer class.
    /// </summary>
    public class ExampleInitializer : MonoBehaviour
    {
        #region Public properties
        public Button playButton;
        public Button stopButton;
        [Space(5)]
        public Systems.FX.FXController lightFX;
        public Systems.FX.FXController anomalyFX;
        [Space(5)]
        public Transform p1;
        public Transform p2;
        #endregion

        #region Private properties
        private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
        #endregion

        #region Unity
        private void Awake()
        {
            playButton.onClick.AddListener(lightFX.Play);
            stopButton.onClick.AddListener(lightFX.Stop);
        }

        private void Start()
        {
            anomalyFX.Play();
            StartCoroutine(MoveAnomalyProcess());
        }

        #endregion

        #region Private methods
        private IEnumerator MoveAnomalyProcess()
        {
            UEase.EaseFunction ease = UEase.GetEaseFunction(UEase.EaseType.easeInOutSine);
            anomalyFX.transform.position = p1.position;

            bool reverse = false;

            while (true)
            {
                float startTime = Time.time;
                float duration = Random.Range(4, 5);

                Vector3 startPos = anomalyFX.transform.position;

                Vector3 endPos = reverse ? p1.position : p2.position;

                float t = 0;

                while (t < 1)
                {
                    t = (Time.time - startTime) / duration;

                    Vector3 newPos;

                    newPos.x = ease(startPos.x, endPos.x, t);
                    newPos.y = ease(startPos.y, endPos.y, t);
                    newPos.z = ease(startPos.z, endPos.z, t);

                    anomalyFX.transform.position = newPos;

                    yield return _waitForFixedUpdate;
                }

                anomalyFX.transform.position = endPos;

                reverse = !reverse;
            }
        }

        #endregion
    }
}