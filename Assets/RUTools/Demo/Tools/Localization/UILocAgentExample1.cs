using UnityEngine;
using UnityEngine.UI;

namespace RUT.Examples.Localization
{
    /// <summary>
    /// UILocAgentExample1 class.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class UILocAgentExample1 : LocAgentExample1
    {
        #region Private properties
        private Text _text;
        #endregion

        #region API
        public override void ReloadContent()
        {
            _text.text = _controller.GetText(ID);
        }
        #endregion

        #region Unity
        protected override void Awake()
        {
            base.Awake();
            _text = GetComponent<Text>();
        }
        #endregion
    }
}