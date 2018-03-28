using System.Collections;
using UnityEngine;

namespace RUT.Systems.FX
{
    /// <summary>
    /// Particle effect.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class FXParticle : FXBase
    {
        #region Private properties
        private ParticleSystem _particleSystem = null;
        private Coroutine _lifeTimeCR = null;

        private WaitForSecondsRealtime _waitForSecondsRealtime;
        #endregion

        #region Unity
        protected override void Awake()
        {
            base.Awake();

            _particleSystem = GetComponent<ParticleSystem>();
            _waitForSecondsRealtime = new WaitForSecondsRealtime(0.5f);
        }
        #endregion

        #region Private methods
        protected override IEnumerator OnEffectEndDelayedProcess()
        {
            this.StopCoroutineSafe(ref _lifeTimeCR);
            _particleSystem.Stop();

            while (_particleSystem.IsAlive())
            {
                yield return _waitForSecondsRealtime;
            }

            OnEffectEnd();
        }

        protected override void OnEffectStart()
        {
            base.OnEffectStart();
            _lifeTimeCR = StartCoroutine(LifeTimeProcess());
        }

        protected override void ResetEffect()
        {
            base.ResetEffect();
            this.StopCoroutineSafe(ref _lifeTimeCR);
        }

        private IEnumerator LifeTimeProcess()
        {
            _particleSystem.Play();

            //If particle system is not in loop mode, wait for system's duration before adding
            //it to finished effects stack.
            if (!_particleSystem.main.loop)
            {
                if (_particleSystem.main.useUnscaledTime)
                    yield return new WaitForSecondsRealtime(_particleSystem.main.duration + _particleSystem.main.startLifetimeMultiplier);
                else
                    yield return new WaitForSeconds(_particleSystem.main.duration + _particleSystem.main.startLifetimeMultiplier);

                while (_particleSystem.IsAlive())
                {
                    yield return _waitForSecondsRealtime;
                }

                base.Stop();
            }
        }
        #endregion
    }
}