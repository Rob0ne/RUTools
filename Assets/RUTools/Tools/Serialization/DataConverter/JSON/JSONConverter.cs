using UnityEngine;
using RUT.Tools.Utilities;
using System;

namespace RUT.Tools.Serialization
{
    /// <summary>
    /// JSON format converter class.
    /// </summary>
    public class JSONConverter : IDataConverter
    {
        #region Public properties
        public string FileExtension { get { return ".json"; } }
        public bool prettyPrint;
        #endregion

        #region Constructor
        public JSONConverter()
        {
            prettyPrint = true;
        }
        public JSONConverter(bool prettyPrint)
        {
            this.prettyPrint = prettyPrint;
        }
        #endregion

        #region API
        public T StringToData<T>(string content, string encoding)
        {
            T data = default(T);

            try
            {
                data = JsonUtility.FromJson<T>(content);
            }
            catch(Exception e)
            {
                ULog.LogDebug("JSONConverter: Failed to convert string to data.\n" + e, ULog.Type.Error);
                throw e;
            }

            return data;
        }

        public string DataToString<T>(T data, string encoding)
        {
            string content = "";

            try
            {
                content = JsonUtility.ToJson(data, true);
            }
            catch (Exception e)
            {
                ULog.LogDebug("JSONConverter: Failed to convert data to string.\n" + e, ULog.Type.Error);
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