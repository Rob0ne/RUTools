namespace RUT.Tools.Pool
{
    /// <summary>
    /// IMultiObjectPool interface.
    /// </summary>
    public interface IMultiObjectPool<T>
    {
        int TotalItems { get; }
        int FreeItems { get; }
        int UsedItems { get; }

        void Initialise();
        void Clear();
        void DisposeAll();

        IPoolable Take(T id);
        bool Recover(IPoolable instance);
    }
}