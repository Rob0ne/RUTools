using System.Collections;
using UnityEngine;
using RUT.Utilities;

namespace RUT.Systems.FX
{
    /// <summary>
    /// Light blink effect.
    /// </summary>
    [RequireComponent(typeof(Light))]
    public class FXLightBlinker : FXBase
    {
        #region Private properties
        public Vector2 intensityRange = new Vector2(1,2);
        public Vector2 timeRange = new Vector2(.5f, 1);
        #endregion

        #region Private properties
        private Light _light = null;
        private Coroutine _blinkCR = null;
        private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
        #endregion

        #region API
        #endregion

        #region Unity
        protected override void Awake()
        {
            base.Awake();

            _light = GetComponent<Light>();
        }
        #endregion

        #region Private methods
        protected override void OnEffectStart()
        {
            base.OnEffectStart();

            _blinkCR = StartCoroutine(BlinkProcess());
        }

        protected override void OnEffectEnd()
        {
            base.OnEffectEnd();

            this.StopCoroutineSafe(ref _blinkCR);
        }

        private IEnumerator BlinkProcess()
        {
            UEase.EaseFunction ease = UEase.GetEaseFunction(UEase.EaseType.easeInOutSine);
            _light.intensity = intensityRange.x;

            bool reverse = false;

            while (true)
            {
                float startTime = Time.time;
                float duration = Random.Range(timeRange.x, timeRange.y);
                if (duration <= 0)
                    duration = 0.01f;

                float startIntensity = _light.intensity;

                float minIntensity = reverse ? intensityRange.x : startIntensity;
                float maxIntensity = reverse ? startIntensity : intensityRange.y;

                float endIntensity = Random.Range(minIntensity, maxIntensity);

                float t = 0;

                while(t < 1)
                {
                    t = (Time.time - startTime) / duration;
                    _light.intensity = ease(startIntensity, endIntensity, t);

                    yield return _waitForEndOfFrame;
                }

                _light.intensity = endIntensity;
                reverse = !reverse;
            }
        }
        #endregion
    }
}