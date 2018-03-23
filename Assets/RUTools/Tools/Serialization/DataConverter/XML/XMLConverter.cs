using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System;
using RUT.Tools.Utilities;

namespace RUT.Tools.Serialization
{
    /// <summary>
    /// XMl format converter class.
    /// </summary>
    public class XMLConverter : IDataConverter
    {
        #region Public properties
        public string FileExtension { get { return ".xml"; } }
        public XmlWriterSettings XMLSettings { get { return _xmlSettings; } }
        #endregion

        #region Private properties
        private XmlWriterSettings _xmlSettings;
        #endregion

        #region Constructor
        public XMLConverter()
        {
            _xmlSettings = new XmlWriterSettings
            {
                Indent = true,
            };
        }
        public XMLConverter(XmlWriterSettings xmlSettings)
        {
            _xmlSettings = xmlSettings;
        }
        #endregion

        #region API
        public T StringToData<T>(string content, string encoding)
        {
            T data = default(T);

            try
            {
                using (MemoryStream stream = new MemoryStream(Encoding.GetEncoding(encoding).GetBytes(content)))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    data = (T)serializer.Deserialize(stream);
                }
            }
            catch(Exception e)
            {
                ULog.LogDebug("XMLConverter: Failed to convert string to data.\n"+e, ULog.Type.Error);
                throw e;
            }

            return data;
        }

        public string DataToString<T>(T data, string encoding)
        {
            string content = "";

            try
            {
                _xmlSettings.Encoding = Encoding.GetEncoding(encoding);

                using (MemoryStream stream = new MemoryStream())
                using (XmlWriter writer = XmlWriter.Create(stream, _xmlSettings))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    serializer.Serialize(writer, data);

                    stream.Flush();
                    stream.Position = 0;

                    using (StreamReader memReader = new StreamReader(stream))
                    {
                        content = memReader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                ULog.LogDebug("XMLConverter: Failed to convert data to string.\n"+e, ULog.Type.Error);
                throw e;
            }

            return content;
        }
        #endregion
    }

}