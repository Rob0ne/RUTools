using UnityEngine;
using RUT.Tools.Serialization;

namespace RUT.Examples.Serialization
{
    /// <summary>
    /// SerializationExample1 class.
    /// </summary>
    public class SerializationExample1 : MonoBehaviour
    {
        #region Public properties
        public string filePath = "RUTools/Demo/Tools/Serialization/";
        public SerializableStruct data = new SerializableStruct();
        #endregion

        #region Private properties
        private Serializer _xmlSerializer = new Serializer(new XMLConverter());
        private Serializer _jsonSerializer = new Serializer(new JSONConverter());
        private Serializer _binarySerializer = new Serializer(new BinaryConverter());
        #endregion

        #region API
        public void XmlSerialize()
        {
            _xmlSerializer.SerializeData<SerializableStruct>(data, Application.dataPath + "/" + filePath + "fileExample.xml");
        }
        public void XmlDeserialize()
        {
            data = _xmlSerializer.DeserializeData<SerializableStruct>(Application.dataPath + "/" + filePath + "fileExample.xml");
        }

        public void JsonSerialize()
        {
            _jsonSerializer.SerializeData<SerializableStruct>(data, Application.dataPath + "/" + filePath + "fileExample.json");
        }

        public void JsonDeserialize()
        {
            data = _jsonSerializer.DeserializeData<SerializableStruct>(Application.dataPath + "/" + filePath + "fileExample.json");
        }

        public void BinarySerialize()
        {
            _binarySerializer.SerializeData<SerializableStruct>(data, Application.dataPath + "/" + filePath + "fileExample.bin");
        }

        public void BinaryDeserialize()
        {
            data = _binarySerializer.DeserializeData<SerializableStruct>(Application.dataPath + "/" + filePath + "fileExample.bin");
        }
        #endregion

        #region SubType
        [System.Serializable]
        public struct SerializableStruct
        {
            public int intValue;
            public float floatValue;
            public string stringValue;
        }
        #endregion
    }
}