using UnityEngine;

namespace RUT.Utilities.Debug
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
        private Texture2D _backgroundTex = null;

        private Color _backgroundColor = new Color(0, 0, 0, .4f);
        private Color _textColor = new Color(.5f, 1, .5f, 1);

        private GUIStyle _backgroundStyle = new GUIStyle();
        private GUIStyle _textStyle = new GUIStyle();

        private Rect _box = new Rect(5, 5, 0, 0);
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
            float deltaTime = Time.realtimeSinceStartup - _lastUpdateTime;
            _lastUpdateTime = Time.realtimeSinceStartup;

            if (_updateTimer <= 0)
            {
                _lastFPS = (int)(1.0f / deltaTime);
                _lastFPS = Mathf.Clamp(_lastFPS, 0, UDebug.CachedFPSValue.Length - 1);

                _updateTimer = updateTime;
            }
            else
                _updateTimer -= deltaTime;
        }

        private void OnGUI()
        {
            int w = Screen.width;
            int	h = Screen.height;

            _box.width = (int)(h / 55)*5;
            _box.height = h / 40;

            _textStyle.fontSize = h / 55;

            GUI.Box(_box, GUIContent.none, _backgroundStyle);
            GUI.Label(_box, UDebug.CachedFPSValue[_lastFPS], _textStyle);
        }
        #endregion
    }
}