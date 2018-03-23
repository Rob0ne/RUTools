namespace RUT.Tools.Serialization
{
    /// <summary>
    /// Serializer interface.
    /// </summary>
    public interface ISerializer
    {
        void SerializeData<T>(T data, string filePath, bool encrypted = false);
        T DeserializeData<T>(string filePath);
    }
}

