namespace RUT.Tools.Event.Observer
{
    public delegate void ObserverAction<T>(ref T arg);
    public delegate void ObserverAction();

    /// <summary>
    /// IObserver interface.
    /// </summary>
    public interface IObserver<T>
    {
        void OnObserverNotified(ref T arg);

        void AddAction(ObserverAction<T> action);
        void RemoveAction(ObserverAction<T> action);
    }

    /// <summary>
    /// IObserver interface.
    /// </summary>
    public interface IObserver
    {
        void OnObserverNotified();

        void AddAction(ObserverAction action);
        void RemoveAction(ObserverAction action);
    }
}