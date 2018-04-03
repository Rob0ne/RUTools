namespace RUT.Utilities.Identification
{
    /// <summary>
    /// IObjectID interface.
    /// </summary>
    public interface IObjectID<T>
    {
        T ID { get; }
    }
}