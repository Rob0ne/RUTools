using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using RUT.Utilities;

namespace RUT.Tools.Serialization
{
    /// <summary>
    /// Binary format converter class.
    /// </summary>
    public class BinaryConverter : IDataConverter
    {
        #region Public properties
        public string FileExtension { get { return ".bin"; } }
        #endregion

        #region Private properties
        #endregion

        #region API
        public T StringToData<T>(string content, string encoding)
        {
            T data = default(T);

            try
            {
                using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(content)))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    data = (T)formatter.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                ULog.LogDebug("BinaryConverter: Failed to convert string to data.\n" + e, ULog.Type.Error);
                throw e;
            }

            return data;
        }

        public string DataToString<T>(T data, string encoding)
        {
            string content = "";

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(stream, data);

                    stream.Flush();
                    stream.Position = 0;

                    content = Convert.ToBase64String(stream.ToArray());
                }
            }
            catch (Exception e)
            {
                ULog.LogDebug("BinaryConverter: Failed to convert data to string.\n" + e, ULog.Type.Error);
                throw e;
            }

            return content;
        }
        #endregion

        #region Unity
        #endregion

        #region Private methods
        #endregion

        #region SubType
        #endregion
    }
}