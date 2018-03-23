using UnityEngine;
using UnityEngine.UI;

namespace RUT.Utilities.Debug
{
    [RequireComponent(typeof(Text))]
    public class FPSDisplayUIText : MonoBehaviour
    {
        #region Public properties
        [Range(0,10)] public float updateTime = 0.1f;
        #endregion

        #region Private properties
        private float _updateTimer = 0;
        private int _lastFPS = 0;

        private float _lastUpdateTime = 0;

        private Text _text = null;
        #endregion

        #region Unity
        private void Awake()
        {
            _text = GetComponent<Text>();
        }

        private void Update()
        {
            float deltaTime = Time.realtimeSinceStartup - _lastUpdateTime;
            _lastUpdateTime = Time.realtimeSinceStartup;

            if (_updateTimer <= 0)
            {
                _lastFPS = (int)(1.0f / deltaTime);
                _lastFPS = Mathf.Clamp(_lastFPS, 0, UDebug.CachedFPSValue.Length - 1);
                _text.text = UDebug.CachedFPSValue[_lastFPS];

                _updateTimer = updateTime;
            }
            else
                _updateTimer -= deltaTime;
        }
        #endregion
    }
}