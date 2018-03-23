using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace RUT.Editor
{
    /// <summary>
    /// RUToolsPreferences class.
    /// </summary>
    [CreateAssetMenu(fileName = "RUTPreferences", menuName = "RUTools/Options/Preferences", order = 1)]
    public class RUToolsPreferences : ScriptableObject
    {
        #region Public properties
        public List<EditorSettings> editors = new List<EditorSettings>();
        #endregion

        #region Private properties
        private static string _assetFolder = "Assets/";
        private static string _resourcesFolder = "Resources/";
        private static string _editorFolder = "Editor/";

        private static string _skinName = "RUTSkin";
        private static string _preferencesName = "RUTPreferences";
        private static string _assetExt = ".asset";

        private static RUToolsPreferences _preferencesCached = null;
        private static RUToolsSkin _skinCached = null;

        private Dictionary<string, Dictionary<string, State>> _editorStateSet = null;
        #endregion

        #region API
#if UNITY_EDITOR
        /// <summary>
        /// Gets RUTools skin.
        /// </summary>
        public static RUToolsSkin GetSkin()
        {
            if (_skinCached == null)
            {
                _skinCached = Resources.Load<RUToolsSkin>(_editorFolder + _skinName);

                if (_skinCached == null)
                {
                    _skinCached = CreateAsset<RUToolsSkin>(_skinName + _assetExt);
                }
            }

            return _skinCached;
        }

        /// <summary>
        /// Gets RUTools preferences.
        /// </summary>
        public static RUToolsPreferences GetPreferences()
        {
            if (_preferencesCached == null)
            {
                _preferencesCached = Resources.Load<RUToolsPreferences>(_editorFolder + _preferencesName);

                if (_preferencesCached == null)
                {
                    _preferencesCached = CreateAsset<RUToolsPreferences>(_preferencesName + _assetExt);
                }
            }

            return _preferencesCached;
        }
#endif

        /// <summary>
        /// Gets state's value.
        /// </summary>
        public int GetEditorState(Type editor, string state)
        {
            string editorName = editor.ToString();
            string id = state;

            if (_editorStateSet == null)
                RebuildEditorSet();

            int value = 0;

            Dictionary<string, State> stateSet;
            if (_editorStateSet.TryGetValue(editorName, out stateSet))
            {
                State s;
                if(stateSet.TryGetValue(id, out s))
                {
                    value = s.value;
                }
            }

            return value;
        }

        /// <summary>
        /// Sets state's value.
        /// </summary>
        public void SetEditorState(Type editor, string state, short value)
        {
            string editorName = editor.ToString();
            string id = state;
            State stateData = new State (id, value );

            if (_editorStateSet == null)
                RebuildEditorSet();

            //Try to quick access data from dictionary
            bool quickSet = true;
            Dictionary<string, State> stateSet = null;
            if (_editorStateSet.TryGetValue(editorName, out stateSet))
            {
                State s;
                if (stateSet.TryGetValue(id, out s))
                {
                    s.value = value;
                }
                else
                    quickSet = false;
            }
            else
                quickSet = false;

            //If quick access failed, meaning that the data is new, set in the main list.
            if (!quickSet)
            {
                //If stateSet was found, meaning that the editor's data is already there, just add the new state.
                if(stateSet != null)
                {
                    for (int i = 0; i < editors.Count; ++i)
                    {
                        if (editors[i] != null && editors[i].Editor == editorName)
                        {
                            editors[i].states.Add(stateData);

                            //Update dictionary.
                            stateSet[id] = stateData;
                            return;
                        }
                    }
                }
                else
                {
                    EditorSettings settings = new EditorSettings(editorName);
                    settings.states.Add(stateData);
                    editors.Add(settings);

                    //Update dictionary.
                    _editorStateSet[editorName] = new Dictionary<string, State>();
                    _editorStateSet[editorName][id] = stateData;
                }
            }
        }
        #endregion

        #region Unity
        #endregion

        #region Private methods
        private void RebuildEditorSet()
        {
            _editorStateSet = new Dictionary<string, Dictionary<string, State>>();

            for (int i = 0; i < editors.Count; ++i)
            {
                if(editors[i] != null)
                {
                    string editorName = editors[i].Editor;

                    _editorStateSet[editorName] = new Dictionary<string, State>();

                    List<State> stateList = editors[i].states;

                    for (int j = 0; j < stateList.Count; ++j)
                    {
                        if (stateList[j] != null)
                        {
                            _editorStateSet[editorName][stateList[j].ID] = stateList[j];
                        }
                    }
                }
            }
        }

        private static T CreateAsset<T>(string assetName) where T : ScriptableObject
        {
            string folderPath = Application.dataPath + "/" + _resourcesFolder + _editorFolder;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            T asset = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(asset, _assetFolder + _resourcesFolder + _editorFolder + assetName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return asset;
        }
        #endregion

        #region SubType
        [Serializable]
        public class EditorSettings
        {
            [SerializeField][ReadOnly]
            private string editor;
            public List<State> states;

            public string Editor { get { return editor; } }

            public EditorSettings(string editor)
            {
                this.editor = editor;
                this.states = new List<State>();
            }
        }
        [Serializable]
        public class State
        {
            [SerializeField][ReadOnly]
            private string id;
            public short value;

            public string ID { get { return id; } }

            public State(string id, short value)
            {
                this.id = id;
                this.value = value;
            }
        }
        #endregion
    }
}