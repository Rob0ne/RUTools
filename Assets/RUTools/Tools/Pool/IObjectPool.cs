namespace RUT.Tools.Pool
{
    /// <summary>
    /// IObjectPool interface.
    /// </summary>
    public interface IObjectPool
    {
        bool IsInitialised { get; }

        IPoolable Item { get; }
        int TotalItems { get; }
        int FreeItems { get; }
        int UsedItems { get; }

        void Initialise();
        void Clear();
        void DisposeAll();

        IPoolable Take();
        bool Recover(IPoolable instance);
    }
}