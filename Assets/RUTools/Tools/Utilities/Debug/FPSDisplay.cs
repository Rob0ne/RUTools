using UnityEngine;
using System.Text;

namespace RUT.Tools.Utilities.Debug
{
    public class FPSDisplay : MonoBehaviour
    {
        #region Public properties
        [Range(0,10)] public float updateTime = 0.1f;
        #endregion

        #region Private properties
        private float _updateTimer = 0;
        private int _lastFPS = 0;

        private float _lastUpdateTime = 0;
        private float _deltaTime = 0;
        private Texture2D _backgroundTex = null;

        private Color _backgroundColor = new Color(0, 0, 0, .4f);
        private Color _textColor = new Color(.5f, 1, .5f, 1);

        private GUIStyle _backgroundStyle = new GUIStyle();
        private GUIStyle _textStyle = new GUIStyle();

        //private StringBuilder _text = new StringBuilder(20, 100);
        private Rect _box = new Rect(5, 5, 0, 0);

        private static readonly string[] _cachedFPSValue = new string[]
        {
            "FPS: 000", "FPS: 001", "FPS: 002", "FPS: 003", "FPS: 004", "FPS: 005", "FPS: 006", "FPS: 007", "FPS: 008", "FPS: 009",
            "FPS: 010", "FPS: 011", "FPS: 012", "FPS: 013", "FPS: 014", "FPS: 015", "FPS: 016", "FPS: 017", "FPS: 018", "FPS: 019",
            "FPS: 020", "FPS: 021", "FPS: 022", "FPS: 023", "FPS: 024", "FPS: 025", "FPS: 026", "FPS: 027", "FPS: 028", "FPS: 029",
            "FPS: 030", "FPS: 031", "FPS: 032", "FPS: 033", "FPS: 034", "FPS: 035", "FPS: 036", "FPS: 037", "FPS: 038", "FPS: 039",
            "FPS: 040", "FPS: 041", "FPS: 042", "FPS: 043", "FPS: 044", "FPS: 045", "FPS: 046", "FPS: 047", "FPS: 048", "FPS: 049",
            "FPS: 050", "FPS: 051", "FPS: 052", "FPS: 053", "FPS: 054", "FPS: 055", "FPS: 056", "FPS: 057", "FPS: 058", "FPS: 059",
            "FPS: 060", "FPS: 061", "FPS: 062", "FPS: 063", "FPS: 064", "FPS: 065", "FPS: 066", "FPS: 067", "FPS: 068", "FPS: 069",
            "FPS: 070", "FPS: 071", "FPS: 072", "FPS: 073", "FPS: 074", "FPS: 075", "FPS: 076", "FPS: 077", "FPS: 078", "FPS: 079",
            "FPS: 080", "FPS: 081", "FPS: 082", "FPS: 083", "FPS: 084", "FPS: 085", "FPS: 086", "FPS: 087", "FPS: 088", "FPS: 089",
            "FPS: 090", "FPS: 091", "FPS: 092", "FPS: 093", "FPS: 094", "FPS: 095", "FPS: 096", "FPS: 097", "FPS: 098", "FPS: 099",
            "FPS: 100", "FPS: 101", "FPS: 102", "FPS: 103", "FPS: 104", "FPS: 105", "FPS: 106", "FPS: 107", "FPS: 108", "FPS: 109",
            "FPS: 110", "FPS: 111", "FPS: 112", "FPS: 113", "FPS: 114", "FPS: 115", "FPS: 116", "FPS: 117", "FPS: 118", "FPS: 119",
            "FPS: 120", "FPS: 121", "FPS: 122", "FPS: 123", "FPS: 124", "FPS: 125", "FPS: 126", "FPS: 127", "FPS: 128", "FPS: 129",
            "FPS: 130", "FPS: 131", "FPS: 132", "FPS: 133", "FPS: 134", "FPS: 135", "FPS: 136", "FPS: 137", "FPS: 138", "FPS: 139",
            "FPS: 140", "FPS: 141", "FPS: 142", "FPS: 143", "FPS: 144", "FPS: 145", "FPS: 146", "FPS: 147", "FPS: 148", "FPS: 149",
            "FPS: 150", "FPS: 151", "FPS: 152", "FPS: 153", "FPS: 154", "FPS: 155", "FPS: 156", "FPS: 157", "FPS: 158", "FPS: 159",
            "FPS: 160", "FPS: 161", "FPS: 162", "FPS: 163", "FPS: 164", "FPS: 165", "FPS: 166", "FPS: 167", "FPS: 168", "FPS: 169",
            "FPS: 170", "FPS: 171", "FPS: 172", "FPS: 173", "FPS: 174", "FPS: 175", "FPS: 176", "FPS: 177", "FPS: 178", "FPS: 179",
            "FPS: 180", "FPS: 181", "FPS: 182", "FPS: 183", "FPS: 184", "FPS: 185", "FPS: 186", "FPS: 187", "FPS: 188", "FPS: 189",
            "FPS: 190", "FPS: 191", "FPS: 192", "FPS: 193", "FPS: 194", "FPS: 195", "FPS: 196", "FPS: 197", "FPS: 198", "FPS: 199",
            "FPS: 200"
        };
        #endregion

        #region Unity
        private void Awake()
        {
            _backgroundTex = new Texture2D(1, 1);

            _backgroundTex.SetPixel(0, 0, _backgroundColor);

            _backgroundTex.wrapMode = TextureWrapMode.Repeat;
            _backgroundTex.Apply();

            _backgroundStyle.normal.background = _backgroundTex;

            _textStyle.padding = new RectOffset(4, 4, 2, 2);
            _textStyle.alignment = TextAnchor.MiddleLeft;
            _textStyle.normal.textColor = _textColor;
            _textStyle.fontStyle = FontStyle.Bold;

        }

        private void Update()
        {
            _deltaTime = Time.realtimeSinceStartup - _lastUpdateTime;
            _lastUpdateTime = Time.realtimeSinceStartup;
        }

        private void OnGUI()
        {
            int w = Screen.width;
            int	h = Screen.height;

            _box.width = (int)(h / 55)*5;
            _box.height = h / 40;

            _textStyle.fontSize = h / 55;

            if (_updateTimer <= 0)
            {
                _lastFPS = (int)(1.0f / _deltaTime);
                _lastFPS = Mathf.Clamp(_lastFPS, 0, _cachedFPSValue.Length - 1);

                /*_text.Length = 0;
                _text.Append("FPS: ").Append(_lastFPS);*/

                _updateTimer = updateTime;
            }
            else
                _updateTimer -= _deltaTime;

            GUI.Box(_box, GUIContent.none, _backgroundStyle);
            GUI.Label(_box, _cachedFPSValue[_lastFPS], _textStyle);
        }
        #endregion
    }
}