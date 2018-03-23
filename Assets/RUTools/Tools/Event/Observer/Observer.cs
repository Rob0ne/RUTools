namespace RUT.Tools.Event.Observer
{
    /// <summary>
    /// Observer class.
    /// </summary>
    public class Observer<T> : IObserver<T>
    {
        #region Private properties
        private ObserverAction<T> _actions;
        #endregion

        #region API
        public void OnObserverNotified(ref T arg)
        {
            if(_actions != null)
                _actions(ref arg);
        }

        public void AddAction(ObserverAction<T> action)
        {
            _actions += action;
        }
        public void RemoveAction(ObserverAction<T> action)
        {
            _actions -= action;
        }
        #endregion
    }

    /// <summary>
    /// Observer class.
    /// </summary>
    public class Observer : IObserver
    {
        #region Private properties
        private ObserverAction _actions;
        #endregion

        #region API
        public void OnObserverNotified()
        {
            if (_actions != null)
                _actions();
        }

        public void AddAction(ObserverAction action)
        {
            _actions += action;
        }
        public void RemoveAction(ObserverAction action)
        {
            _actions -= action;
        }
        #endregion
    }
}