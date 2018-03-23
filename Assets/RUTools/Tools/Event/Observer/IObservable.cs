namespace RUT.Tools.Event.Observer
{
    /// <summary>
    /// IObservable interface.
    /// </summary>
    public interface IObservable<T>
    {
        void Subscribe(IObserver<T> observer);
        void Unsubscribe(IObserver<T> observer);

        void Notify(ref T arg);
    }

    /// <summary>
    /// IObservable interface.
    /// </summary>
    public interface IObservable
    {
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);

        void Notify();
    }
}