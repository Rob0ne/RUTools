using System.Collections.Generic;
using UnityEngine;
using RUT.Tools.Collision;

namespace RUT.Examples.TargetAction
{
    /// <summary>
    /// TargetActionExample1 class.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class TargetActionExample1 : Systems.Action.TargetAction
    {
        #region Public properties
        public ParticleSystem loopFX;
        public ParticleSystem endFX;

        public int LastTargetCount
        { get { return _lastTargetCount; } }
        #endregion

        #region Private properties
        private Rigidbody _rigidbody;
        private int _lastTargetCount;
        #endregion

        #region API
        public void MovePosition(Vector3 position)
        {
            _rigidbody.MovePosition(position);
        }
        #endregion

        #region Unity
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        #endregion

        #region Private methods
        protected override void OnStartCallback(List<ITargetable> targets)
        {
            _lastTargetCount = targets.Count;
            loopFX.Play();
        }
        protected override void OnLoopCallback(List<ITargetable> targets)
        {
            _lastTargetCount = targets.Count;
            for (int i = 0; i < targets.Count; ++i)
            {
                TargetExample1 targetExample = targets[i] as TargetExample1;

                if(targetExample != null)
                {
                    targetExample.GetHit(0);
                }
            }
        }
        protected override void OnEndCallback(List<ITargetable> targets)
        {
            _lastTargetCount = targets.Count;
            for (int i = 0; i < targets.Count; ++i)
            {
                TargetExample1 targetExample = targets[i] as TargetExample1;

                if (targetExample != null)
                {
                    targetExample.GetHit(1);
                }
            }

            loopFX.Stop();
            endFX.Play();
        }
        #endregion
    }
}