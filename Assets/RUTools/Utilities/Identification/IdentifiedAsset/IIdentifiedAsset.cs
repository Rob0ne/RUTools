namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// IIdentifiedAsset interface.
    /// </summary>
    public interface IIdentifiedAsset<T, U> where T : class
    {
        U ID { get; }
        T Asset { get; }
    }
}