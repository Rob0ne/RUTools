namespace RUT.Tools.Pool
{
    /// <summary>
    /// Poolable interface.
    /// </summary>
    public interface IPoolable : IDisposable
    {
        void LinkToPool(IObjectPool pool);
    }
}