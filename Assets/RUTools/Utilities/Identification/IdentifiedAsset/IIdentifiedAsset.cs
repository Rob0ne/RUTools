namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// IIdentifiedAsset interface.
    /// </summary>
    public interface IIdentifiedAsset<T, U> where U : class
    {
        T ID { get; }
        U Asset { get; }
    }
}