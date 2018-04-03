using System.Collections.Generic;
using UnityEngine;

namespace RUT.Editor
{
    /// <summary>
    /// RUToolsSkin class.
    /// </summary>
    //[CreateAssetMenu(fileName = "RUTSkin", menuName = "RUTools/Options/Skin", order = 2)]
    public class RUToolsSkin : ScriptableObject
    {
        #region Public properties
        public GUIStyle[] styles = new GUIStyle[1];
        #endregion

        #region Private properties
        private Dictionary<string, GUIStyle> _styleSet = null;
        private GUIStyle _defaultStyle = new GUIStyle();
        #endregion

        #region API
        public GUIStyle GetStyle(string id)
        {
            if (_styleSet == null)
                RebuildStyleSet();

            GUIStyle style = null;
            _styleSet.TryGetValue(id, out style);

            return style != null ? style : _defaultStyle;
        }
        #endregion

        #region Unity
        #endregion

        #region Private methods
        private void RebuildStyleSet()
        {
            _styleSet = new Dictionary<string, GUIStyle>();

            for (int i = 0; i < styles.Length; ++i)
            {
                if(styles[i] != null)
                {
                    _styleSet[styles[i].name] = styles[i];
                }
            }
        }
        #endregion

        #region SubType
        #endregion
    }
}