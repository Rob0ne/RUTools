using System.Collections.Generic;
using UnityEngine;
using RUT.Utilities;
using RUT.Utilities.Identification.Asset;
using System.Text;
using System.IO;

namespace RUT.Tools.Localization
{
    /// <summary>
    /// LocalizationControllerByStringID class.
    /// </summary>
    [System.Serializable]
    public class LocalizationControllerByStringID : ILocalizationController<string>
    {
        #region Public properties
        public LocalizedResources[] localizedResources;
        [Space(5)]
        public string defaultText;
        public AudioClip defaultAudio;
        public Texture defaultTexture;

        public event LocalizationCallback OnLanguageChanged
        {
            add { _onLanguageChanged += value; }
            remove { _onLanguageChanged -= value; }
        }
        #endregion

        #region Private properties
        private event LocalizationCallback _onLanguageChanged;

        private Dictionary<string, string> _textSet = new Dictionary<string, string>();
        private Dictionary<string, AudioClip> _audioSet = new Dictionary<string, AudioClip>();
        private Dictionary<string, Texture> _textureSet = new Dictionary<string, Texture>();

        private LocalizedResources _emptyData = new LocalizedResources();
        #endregion

        #region API
        /// <summary>
        /// Reloads data associated to given "language".
        /// </summary>
        public bool SetLanguage(string language)
        {
            for (int i = 0; i < localizedResources.Length; ++i)
            {
                if (language.Equals(localizedResources[i].language))
                {
                    RebuildSets(ref localizedResources[i]);
                    return true;
                }
            }

            RebuildSets(ref _emptyData);
            return false;
        }

        /// <summary>
        /// Retrieves text associated to "id".
        /// </summary>
        public string GetText(string id)
        {
            string value;
            if (!_textSet.TryGetValue(id, out value))
                value = defaultText;

            return value;
        }

        /// <summary>
        /// Retrieves audio clip associated to "id".
        /// </summary>
        public AudioClip GetAudio(string id)
        {
            AudioClip value;
            if (!_audioSet.TryGetValue(id, out value))
                value = defaultAudio;

            return value;
        }

        /// <summary>
        /// Retrieves texture associated to "id".
        /// </summary>
        public Texture GetTexture(string id)
        {
            Texture value;
            if (!_textureSet.TryGetValue(id, out value))
                value = defaultTexture;

            return value;
        }

        /// <summary>
        /// Creates a text file template.
        /// </summary>
        public void CreateTextFileTemplate(string path)
        {
            TextsByStringIDStruct data = new TextsByStringIDStruct
            {
                texts = new TextsByStringIDStruct.Item[2]
            };

            for (int i = 0; i < data.texts.Length; ++i)
            {
                data.texts[i].id = "id"+i;
                data.texts[i].text = "text"+i;
            }

            string content = JsonUtility.ToJson(data, true);

            Encoding encoding = Encoding.GetEncoding("UTF-8");

            path += ".json";

            FileStream fileStream = new FileStream(path, FileMode.Create);
            StreamWriter fileWriter = new StreamWriter(fileStream, encoding);

            fileWriter.WriteLine(content);

            fileWriter.Close();
            fileStream.Close();

            ULog.LogDebug("LocalizationController: Text file template created at "+ path, ULog.Type.Log);
        }
        #endregion

        #region Private methods
        private bool RebuildSets(ref LocalizedResources data)
        {
            _textSet.Clear();
            _audioSet.Clear();
            _textureSet.Clear();

            for (int i = 0; i < data.resources.Length; ++i)
            {
                switch (data.resources[i].type)
                {
                    case ResourceType.Text:
                        {
                            TextAsset[] fileAssets = Resources.LoadAll<TextAsset>(data.resources[i].directoryPath);
                            TextByStringID[] textAssets = Resources.LoadAll<TextByStringID>(data.resources[i].directoryPath);

                            if (fileAssets != null)
                            {
                                for (int j = 0; j < fileAssets.Length; ++j)
                                {
                                    TextsByStringIDStruct fileText = JsonUtility.FromJson<TextsByStringIDStruct>(fileAssets[j].text);

                                    for (int k = 0; k < fileText.texts.Length; ++k)
                                    {
                                        _textSet[fileText.texts[k].id] = fileText.texts[k].text;
                                    }
                                }
                            }

                            if (textAssets != null)
                            {
                                for (int j = 0; j < textAssets.Length; ++j)
                                {
                                    _textSet[textAssets[j].ID] = textAssets[j].Asset;
                                }
                            }
                        }
                        break;
                    case ResourceType.Sound:
                        {
                            AudioByStringID[] assets = Resources.LoadAll<AudioByStringID>(data.resources[i].directoryPath);

                            if (assets != null)
                            {
                                for (int j = 0; j < assets.Length; ++j)
                                {
                                    AudioClip clip = assets[j].Asset;
                                    if (clip != null)
                                    {
                                        _audioSet[assets[j].ID] = clip;
                                    }
                                }
                            }
                        }
                        break;
                    case ResourceType.Texture:
                        {
                            TextureByStringID[] assets = Resources.LoadAll<TextureByStringID>(data.resources[i].directoryPath);

                            if (assets != null)
                            {
                                for (int j = 0; j < assets.Length; ++j)
                                {
                                    Texture texture = assets[j].Asset;
                                    if (texture != null)
                                    {
                                        _textureSet[assets[j].ID] = texture;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        ULog.LogDebug("LocalizationController: Invalid type of resources.", ULog.Type.Warning);
                        break;
                }
            }

            ULog.LogDebug("LocalizationController: Language changed to " + data.language, ULog.Type.Log);

            if (_onLanguageChanged != null)
                _onLanguageChanged();

            return true;
        }
        #endregion

        #region SubType
        public enum ResourceType
        {
            Text = 0,
            Sound,
            Texture
        }

        [System.Serializable]
        public struct LocalizedResources
        {
            public string language;
            public Item[] resources;

            [System.Serializable]
            public struct Item
            {
                public string directoryPath;
                public ResourceType type;
            }
        }

        [System.Serializable]
        private struct TextsByStringIDStruct
        {
            public Item[] texts;

            [System.Serializable]
            public struct Item
            {
                public string id;
                public string text;
            }
        }
        #endregion
    }
}