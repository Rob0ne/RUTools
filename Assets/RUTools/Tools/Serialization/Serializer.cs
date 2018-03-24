using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System;
using System.Threading;
using RUT.Utilities;

namespace RUT.Tools.Serialization
{
    /// <summary>
    /// File serializer class.
    /// </summary>
    public sealed class Serializer : ISerializer
    {
        #region Public properties
        #endregion

        #region Private properties
        private static readonly string _tempFileExt = ".temp";
        private static readonly string _corruptFileExt = ".corrupt";

        private static readonly string _defaultEncryptionKey = "54x68jh3aloi87w2vg6f13vcdr7u9n25";
        private static readonly string _encryptedIdentifier = "_r42n9c_";

        private IDataConverter _converter = null;
        private string _encoding = "UTF-8";
        #endregion

        #region Constructor
        public Serializer (IDataConverter converter)
        {
            _converter = converter;
        }
        #endregion

        #region API
        /// <summary>
        /// Serializes "data" into a file at given destination "filePath".
        /// </summary>
        public Thread SerializeDataAsync<T>(T data, string filePath, Action onEnd, bool encrypted = false)
        {
            Thread thread = new Thread(() =>
            {
                SerializeData<T>(data, filePath, encrypted);

                if (onEnd != null)
                    onEnd();
            });

            thread.Start();
            return thread;
        }

        /// <summary>
        /// Serializes "data" into a file at given destination "filePath".
        /// </summary>
        public void SerializeData<T>(T data, string filePath, bool encrypted = false)
        {
            if(data == null)
            {
                ULog.LogDebug("Serializer: Cannot serialize null data.", ULog.Type.Warning);
                return;
            }
            if(_converter == null)
            {
                ULog.LogDebug("Serializer: Converter is null.", ULog.Type.Warning);
                return;
            }

            //Get directory path and verify file extension.
            string dirPath = Path.GetDirectoryName(filePath);
            if (string.IsNullOrEmpty(Path.GetExtension(filePath)))
            {
                filePath += _converter.FileExtension;
            }

            bool overridingFile = false;
            string savingFilePath = filePath + _tempFileExt;

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            else
            {
                if (File.Exists(filePath))
                    overridingFile = true;

                if (File.Exists(savingFilePath))
                    File.Delete(savingFilePath);
            }

            //Get file content.
            string content = "";
            try
            {
                content = _converter.DataToString<T>(data, _encoding);
            }
            catch
            {
                ULog.LogDebug("Serializer: Serialization failed.", ULog.Type.Error);
                return;
            }

            Encoding encoding = Encoding.GetEncoding(_encoding);

            FileStream fileStream = new FileStream(savingFilePath, FileMode.Create);
            StreamWriter fileWriter = new StreamWriter(fileStream, encoding);

            if (encrypted)
                content = _encryptedIdentifier + Encrypt(content);

            fileWriter.WriteLine(content);

            fileWriter.Close();
            fileStream.Close();

            //If same file already exits, delete it before moving.
            if (overridingFile)
            {
                File.Delete(filePath);
            }

            File.Move(savingFilePath, filePath);
        }

        /// <summary>
        /// Serializes "data" into a file at given destination "filePath".
        /// </summary>
        public Thread DeserializeDataAsync<T>(string filePath, Action<T> onEnd)
        {
            Thread thread = new Thread(() =>
            {
                T data = DeserializeData<T>(filePath);

                if (onEnd != null)
                    onEnd(data);
            });

            thread.Start();
            return thread;
        }

        /// <summary>
        /// Deserializes file into data.
        /// </summary>
        public T DeserializeData<T> (string filePath)
        {
            T data = default(T);

            if (_converter == null)
            {
                ULog.LogDebug("Serializer: Converter is null.", ULog.Type.Warning);
                return data;
            }

            //Verify file extension.
            if (string.IsNullOrEmpty(Path.GetExtension(filePath)))
            {
                filePath += _converter.FileExtension;
            }

            //If a temp file exists, try to load it first.
            List<string> pathList = new List<string>();
            string filePathTemp = filePath + _tempFileExt;

            if (File.Exists(filePathTemp))
                pathList.Add(filePathTemp);

            if (File.Exists(filePath))
                pathList.Add(filePath);
            else if (pathList.Count == 0)
            {
                ULog.LogDebug("Serializer: File " + filePath + " does not exist.", ULog.Type.Warning);

                return data;
            }

            Encoding encoding = Encoding.GetEncoding(_encoding);

            //Try loading all existing files until success.
            string correctPath = "";
            for (int i = 0; i < pathList.Count; ++i)
            {
                string content;
                using (StreamReader fileReader = new StreamReader(pathList[i], encoding))
                {
                    content = fileReader.ReadToEnd();

                    bool decrypt = false;
                    if(content.StartsWith(_encryptedIdentifier))
                    {
                        decrypt = true;
                        content = content.Remove(0, _encryptedIdentifier.Length);
                    }

                    if (decrypt)
                    {
                        try
                        {
                            content = Decrypt(content);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }

                try
                {
                    data = _converter.StringToData<T>(content, _encoding);

                    if (data != null)
                    {
                        correctPath = pathList[i];
                        break;
                    }
                    else
                        continue;
                }
                catch(Exception)
                {
                    continue;
                }
            }

            //If no file was successful and true file exists, mark it as corrupt
            if (string.IsNullOrEmpty(correctPath))
            {
                if (pathList.Contains(filePath))
                {
                    FlagFileAsCorrupted(filePath);
                }
            }
            //If loaded file is temp file, change it to true file
            else if (correctPath == filePathTemp)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                File.Move(filePathTemp, filePath);
            }

            return data;
        }
        #endregion

        #region Unity
        #endregion

        #region Private methods
        private void FlagFileAsCorrupted(string filePath)
        {
            ULog.LogDebug("Serializer: File " + filePath + " is corrupted.", ULog.Type.Error);

            int corruptCount = 0;
            while (File.Exists(filePath + _corruptFileExt + (++corruptCount))) { }

            File.Move(filePath, filePath + _corruptFileExt + corruptCount);
        }

        private string Encrypt(string msg)
        {
            if (msg == null || msg.Length <= 0)
                return "";

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(_defaultEncryptionKey);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(msg);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        private string Decrypt(string msg)
        {
            if (msg == null || msg.Length <= 0)
                return "";

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(_defaultEncryptionKey);
            byte[] toEncryptArray = Convert.FromBase64String(msg);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        #endregion

        #region SubType
        #endregion
    }

}
