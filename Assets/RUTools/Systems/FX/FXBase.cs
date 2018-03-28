using UnityEngine;
using System.Collections;

namespace RUT.Systems.FX
{
    /// <summary>
    /// Base class for all custom effects.
    /// </summary>
    public abstract class FXBase : MonoBehaviour
    {
        #region Public properties
        [SerializeField]
        protected Settings settings = new Settings(true, true, true);

        public delegate void EffectCallback(FXBase caller);
        public event EffectCallback OnStartEvent;
        public event EffectCallback OnEndEvent;
        #endregion

        #region Private properties
        protected bool _isActive = false;
        protected Coroutine _delayedActionCR = null;
        #endregion

        #region API
        public bool Play()
        { return Play(0); }
        /// <summary>
        /// Starts effect with delay.
        /// </summary>
        public bool Play(float delay)
        {
            if (settings.enableOnPlay)
                gameObject.SetActive(true);

            if (gameObject.activeInHierarchy)
            {
                _isActive = true;

                ResetEffect();

                this.StopCoroutineSafe(ref _delayedActionCR);
                _delayedActionCR = StartCoroutine(OnEffectStartDelayedProcess(delay));

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Stops effect.
        /// </summary>
        public void Stop()
        {
            if (!_isActive)
                return;

            _isActive = false;

            this.StopCoroutineSafe(ref _delayedActionCR);
            _delayedActionCR = StartCoroutine(OnEffectEndDelayedProcess());
        }
        #endregion

        #region Unity
        protected virtual void Awake()
        {
            if (settings.hideOnStart)
                gameObject.SetActive(false);
        }

        protected virtual void Start()
        {
        }

        protected void OnDisable()
        {
            //If object is disabled manually
            _isActive = false;
            OnEffectEnd();

        }
        #endregion

        #region Private methods
        //Allows to delay the effect start.
        protected virtual IEnumerator OnEffectStartDelayedProcess(float delay)
        {
            yield return new WaitForSeconds(delay);
            OnEffectStart();
        }

        //When the effect trully starts.
        protected virtual void OnEffectStart()
        {
            if (OnStartEvent != null)
                OnStartEvent(this);
        }

        //Allows to delay the effect end.
        protected virtual IEnumerator OnEffectEndDelayedProcess()
        {
            yield return null;
            OnEffectEnd();
        }

        //When the effect trully ends.
        protected virtual void OnEffectEnd()
        {
            if (settings.disableOnStop)
                gameObject.SetActive(false);

            if (OnEndEvent != null)
                OnEndEvent(this);
        }

        //Resets all data.
        protected virtual void ResetEffect()
        {
        }
        #endregion

        #region SubType
        [System.Serializable]
        protected struct Settings
        {
            public bool hideOnStart;
            public bool enableOnPlay;
            public bool disableOnStop;

            public Settings(bool hideOnStart, bool enableOnPlay, bool disableOnStop)
            {
                this.hideOnStart = hideOnStart;
                this.enableOnPlay = enableOnPlay;
                this.disableOnStop = disableOnStop;
            }
        }
        #endregion
    }
}