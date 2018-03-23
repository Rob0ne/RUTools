namespace RUT.Tools.Event.Observer
{
    /// <summary>
    /// Observable class.
    /// </summary>
    public class Observable<T> : IObservable<T>
    {
        #region Private properties
        private event ObserverAction<T> _event;
        #endregion

        #region API
        public void Subscribe(IObserver<T> observer)
        {
            _event += observer.OnObserverNotified;
        }
        public void Unsubscribe(IObserver<T> observer)
        {
            _event -= observer.OnObserverNotified;
        }

        public void Notify(ref T arg)
        {
            if (_event != null)
                _event(ref arg);
        }
        #endregion
    }

    /// <summary>
    /// Observable class.
    /// </summary>
    public class Observable : IObservable
    {
        #region Private properties
        private event ObserverAction _event;
        #endregion

        #region API
        public void Subscribe(IObserver observer)
        {
            _event += observer.OnObserverNotified;
        }
        public void Unsubscribe(IObserver observer)
        {
            _event -= observer.OnObserverNotified;
        }

        public void Notify()
        {
            if (_event != null)
                _event();
        }
        #endregion
    }
}