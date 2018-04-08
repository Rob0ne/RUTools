namespace RUT.Tools.Pool
{
    /// <summary>
    /// Disposable interface.
    /// </summary>
    public interface IDisposable : IResetable
    {
        void Dispose();
    }
}