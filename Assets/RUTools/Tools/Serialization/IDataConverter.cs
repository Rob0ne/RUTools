namespace RUT.Tools.Serialization
{
    /// <summary>
    /// Data converter interface.
    /// </summary>
    public interface IDataConverter
    {
        string FileExtension { get; }

        T StringToData<T>(string content, string encoding);
        string DataToString<T>(T data, string encoding);
    }
}