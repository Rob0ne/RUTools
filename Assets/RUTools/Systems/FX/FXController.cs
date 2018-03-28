using UnityEngine;
using System.Collections;
using RUT.Tools.Pool;

namespace RUT.Systems.FX
{
    /// <summary>
    /// Visual effects controller. Used to activate simultaneously multiple effects and handle the ending.
    /// </summary>
    public class FXController : PoolableMBObject
    {
        #region Public properties
        public EndAction endType = EndAction.Disable;
        public float endDelay = 0; //Delay in seconds to call the endType ending.
        [Space(5)]
        [SerializeField] FXSettings[] fxSettings;
        #endregion

        #region Private properties
        private Transform _transform = null;
        private Coroutine _followCR = null;

        private int _totalEffects = 0;
        private int _finishedEffects = 0;
        private int _activeEffects = 0;
        private int _inactiveEffects = 0;

        private bool _isActive = false;

        private WaitForEndOfFrame _yieldFrame = new WaitForEndOfFrame();
        #endregion

        #region API
        /// <summary>
        /// Starts the fx.
        /// </summary>
        public void Play()
        { Play(-1); }
        /// <summary>
        /// Starts the fx with a overriden "lifetime".
        /// </summary>
        public void Play(float lifeTime)
        {
            if (!gameObject.activeInHierarchy)
                return;

            _isActive = true;

            ResetController();

            PlayEffects();

            if (lifeTime >= 0)
                this.DelayedAction(Stop, lifeTime);
        }

        /// <summary>
        /// Stops the fx.
        /// </summary>
        public void Stop()
        {
            if (!gameObject.activeInHierarchy)
                return;

            StopEffects();
        }

        /// <summary>
        /// Starts following a target.
        /// </summary>
        public void Follow(Transform target, bool followRotation = false)
        {
            if (!gameObject.activeInHierarchy)
                return;

            if (target == null)
                return;

            if(_followCR != null)
                StopFollowing();

            _followCR = StartCoroutine(FollowProcess(target, followRotation));
        }
        /// <summary>
        /// Stops following process.
        /// </summary>
        public void StopFollowing()
        {
            this.StopCoroutineSafe(ref _followCR);
        }
        #endregion

        #region Unity
        protected virtual void OnDisable()
        {
            ForcedStop(0);
        }

        protected virtual void Awake()
        {
            _transform = GetComponent<Transform>();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Instant stop of fx.
        /// </summary>
        private void ForcedStop(float endDelay)
        {
            if (!_isActive)
                return;

            CancelInvoke();
            StopAllCoroutines();
            _followCR = null;

            this.DelayedAction(DisableController, endDelay);
        }

        /// <summary>
        /// Resets all controller's data.
        /// </summary>
        private void ResetController()
        {
            CancelInvoke();
            StopAllCoroutines();
        }

        /// <summary>
        /// End of life handling.
        /// </summary>
        private void DisableController()
        {
            _isActive = false;

            switch (endType)
            {
                case EndAction.Nothing:
                    break;
                case EndAction.Disable:
                    gameObject.SetActive(false);
                    break;
                case EndAction.Destroy:
                    Destroy(gameObject);
                    break;
                case EndAction.Dispose:
                    Dispose();
                    break;
            }
        }


        private void PlayEffects()
        {
            _totalEffects = 0;
            _finishedEffects = 0;
            _activeEffects = 0;
            _inactiveEffects = 0;

            if (fxSettings != null)
            {
                for (int i = 0; i < fxSettings.Length; ++i)
                {
                    for (int j = 0; j < fxSettings[i].effects.Length; j++)
                    {
                        FXBase effect = fxSettings[i].effects[j];

                        _totalEffects++;

                        if(effect.Play(fxSettings[i].delay))
                        {
                            effect.OnEndEvent += OnEffectEndCallback;
                            _activeEffects++;
                        }
                        else
                        {
                            _inactiveEffects++;
                        }
                    }
                }
            }
        }

        private void StopEffects()
        {
            if (fxSettings != null)
            {
                for (int i = 0; i < fxSettings.Length; ++i)
                {
                    for (int j = 0; j < fxSettings[i].effects.Length; ++j)
                    {
                        fxSettings[i].effects[j].Stop();
                    }
                }
            }
        }

        private void AddFinishedEffect()
        {
            _finishedEffects++;

            //If total effects reached, end the fx.
            if (_finishedEffects + _inactiveEffects >= _totalEffects)
            {
                ForcedStop(endDelay);
            }
        }

        private IEnumerator FollowProcess(Transform target, bool followRotation)
        {
            if (followRotation)
            {
                while (target != null)
                {
                    _transform.SetPositionAndRotation(target.position, target.rotation);
                    yield return _yieldFrame;
                }
            }
            else
            {
                while (target != null)
                {
                    _transform.position = target.position;
                    yield return _yieldFrame;
                }
            }
        }

        //Events

        private void OnEffectEndCallback(FXBase caller)
        {
            caller.OnEndEvent -= OnEffectEndCallback;
            AddFinishedEffect();
        }
        #endregion

        #region SubType
        public enum EndAction
        {
            Nothing,
            Disable,
            Destroy,
            Dispose,
        }

        [System.Serializable]
        struct FXSettings
        {
            public FXBase[] effects;
            public float delay;
        }
        #endregion
    }
}