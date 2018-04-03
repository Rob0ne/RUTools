using UnityEngine;

namespace RUT.Examples.Localization
{
    /// <summary>
    /// TextLocAgentExample1 class.
    /// </summary>
    [RequireComponent(typeof(TextMesh))]
    public class TextLocAgentExample1 : LocAgentExample1
    {
        #region Private properties
        private TextMesh _textMesh;
        #endregion

        #region API
        public override void ReloadContent()
        {
            _textMesh.text = _controller.GetText(ID);
        }
        #endregion

        #region Unity
        protected override void Awake()
        {
            base.Awake();
            _textMesh = GetComponent<TextMesh>();
        }
        #endregion
    }
}