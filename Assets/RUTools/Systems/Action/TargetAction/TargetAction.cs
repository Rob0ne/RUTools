using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RUT.Tools.Collision;
using RUT.Tools.Collision.Collector;

namespace RUT.Systems.Action
{
    /// <summary>
    /// Targeted action base class. Used to call actions on collected targets.
    /// </summary>
    public class TargetAction : MonoBehaviour
    {
        #region Public properties
        public Settings settings = new Settings(0,1,-1,-1,-1);
        public TargetCollector collector;

        public delegate void CallbackAction (List<ITargetable> targets);
        public event CallbackAction OnStart;
        public event CallbackAction OnLoop;
        public event CallbackAction OnEnd;
        #endregion

        #region Private properties
        protected MonoBehaviour _currentUser = null;
        private Coroutine _actionCR = null;

        private YieldInstruction _yieldFrame = new WaitForEndOfFrame();
        private YieldInstruction _yieldFixed = new WaitForFixedUpdate();

        private List<ITargetable> _currentTargets = new List<ITargetable>();
        private Dictionary<ITargetable, int> _affectedTargets = new Dictionary<ITargetable, int>();
        #endregion

        #region API
        /// <summary>
        /// Starts action.
        /// </summary>
        public void Use(MonoBehaviour user)
        {
            if(!gameObject.activeSelf)
                gameObject.SetActive(true);

            _currentUser = user;
            StartAction();
        }

        /// <summary>
        /// Stops action.
        /// </summary>
        public void Stop()
        {
            StopAction();
            StopAllCoroutines();

            if (settings.autoDisable)
                gameObject.SetActive(false);
        }
        #endregion

        #region Unity
        protected virtual void Start()
        {
            if (settings.autoDisable)
                gameObject.SetActive(false);
        }
        #endregion

        #region Private methods
        protected virtual void OnStartCallback(List<ITargetable> targets) { }
        protected virtual void OnLoopCallback(List<ITargetable> targets) { }
        protected virtual void OnEndCallback(List<ITargetable> targets) { }

        /// <summary>
        /// Handle the data reset and on end events.
        /// </summary>
        protected void StopAction(bool raiseEvent = true)
        {
            if (_actionCR != null)
            {
                this.StopCoroutineSafe(ref _actionCR);

                if (raiseEvent)
                {
                    UpdateCurrentTargets(false);

                    if(OnEnd != null)
                        OnEnd(_currentTargets);

                    OnEndCallback(_currentTargets);
                }
            }

            _affectedTargets.Clear();
            _currentTargets.Clear();
        }

        /// <summary>
        /// Starts the action's process.
        /// </summary>
        protected void StartAction()
        {
            StopAction(false);
            _actionCR = StartCoroutine(ActionProcess());
        }

        private IEnumerator ActionProcess()
        {
            if (settings.delay > 0)
                yield return new WaitForSeconds(settings.delay);

            UpdateCurrentTargets(false);

            if(OnStart != null)
                OnStart(_currentTargets);

            OnStartCallback(_currentTargets);

            float lastUpdateTime = Time.time;
            float lifeTime = settings.lifetime;
            int repeatCount = settings.repeatLimit;

            //Callback is called only if repeat limit is > 0 or negative,
            bool callbackActive = repeatCount != 0;

            YieldInstruction yieldOnUpdate;
            switch (settings.repeatType)
            {
                case UpdateType.Update:
                    yieldOnUpdate = _yieldFrame;
                    break;
                case UpdateType.FixedUpdate:
                    yieldOnUpdate = _yieldFixed;
                    break;

                default:
                    yieldOnUpdate = new WaitForSeconds(settings.repeatInterval);
                    break;
            }

            while (true)
            {
                yield return yieldOnUpdate;

                bool endRequest = false;
                bool enoughTargets = false;

                do
                {
                    //If lifetime is not infinite, check for ending.
                    if (lifeTime >= 0)
                    {
                        lifeTime -= Time.time - lastUpdateTime;
                        lastUpdateTime = Time.time;

                        if (lifeTime < 0)
                        {
                            endRequest = true;
                            break;
                        }
                    }

                    UpdateCurrentTargets(true);

                    enoughTargets = _currentTargets.Count >= settings.minTargetsForRepeat;

                    //if action doesn't need to wait or there is enough targets, break.
                    if (!settings.waitIfNotEnoughTargets || enoughTargets)
                        break;

                    yield return _yieldFixed;
                } while (!enoughTargets);

                if (enoughTargets)
                {
                    if (callbackActive)
                    {
                        if (OnLoop != null)
                            OnLoop(_currentTargets);

                        OnLoopCallback(_currentTargets);
                    }

                    //If repeat limit is not infinite check count.
                    if (repeatCount >= 0)
                    {
                        repeatCount--;

                        //If repeat limit has been reached, disable callback.
                        if (repeatCount <= 0)
                        {
                            repeatCount = 0;
                            callbackActive = false;
                            if (settings.endOnNoRepeatLeft)
                                endRequest = true;
                        }
                    }
                }

                if (endRequest)
                    break;
            }

            StopAction();
        }

        private void UpdateCurrentTargets(bool notifyAffection)
        {
            _currentTargets.Clear();

            if (collector != null)
            {
                ITargetable[] potentialTargets = collector.Collection;

                if (settings.affectedLimit > 0)
                {
                    for (int i = 0; i < potentialTargets.Length; ++i)
                    {
                        int affectedCount;

                        if (_affectedTargets.TryGetValue(potentialTargets[i], out affectedCount))
                        {
                            if (affectedCount < settings.affectedLimit)
                            {
                                if(notifyAffection)
                                    _affectedTargets[potentialTargets[i]]++;

                                _currentTargets.Add(potentialTargets[i]);
                            }
                        }
                        else
                        {
                            if(notifyAffection)
                            {
                                affectedCount = 1;
                                _affectedTargets.Add(potentialTargets[i], affectedCount);
                            }

                            _currentTargets.Add(potentialTargets[i]);
                        }
                    }
                }
                //If no affected limit, use on everyone.
                else if (settings.affectedLimit < 0)
                {
                    _currentTargets.AddRange(potentialTargets);
                }
            }
        }
        #endregion

        #region SubType
        [System.Serializable]
        public struct Settings
        {
            //Delay before starting action.
            public float delay;
            //Time during which the action will be active. Negative value = unlimited.
            public float lifetime;
            //Action's repeat interval time.
            public float repeatInterval;

            //Type of repeat synchronisation.
            public UpdateType repeatType;
            //Number of times that action can be used before ending. Negative value = unlimited.
            public int repeatLimit;
            //Number of times that a target can be affected by the action. Negative value = unlimited.
            public int affectedLimit;

            //Min targets count for calling repeat callback.
            public int minTargetsForRepeat;

            //If action should stop when repeat limit has been reached.
            public bool endOnNoRepeatLeft;
            //If action should wait until it can call the repeat callback before looping.
            public bool waitIfNotEnoughTargets;
            //If object should be automatically disabled at the end.
            public bool autoDisable;

            public Settings(float delay, float lifetime, float repeatInterval,
                int repeatLimit, int affectedLimit)
            {
                this.delay = delay;
                this.lifetime = lifetime;
                this.repeatInterval = repeatInterval;
                this.repeatLimit = repeatLimit;
                this.affectedLimit = affectedLimit;
                this.minTargetsForRepeat = 0;
                this.endOnNoRepeatLeft = false;
                this.waitIfNotEnoughTargets = false;
                this.autoDisable = false;
                this.repeatType = 0;
            }
        }

        public enum UpdateType : int
        {
            Time = 0,
            Update,
            FixedUpdate,
        }
        #endregion
    }
}