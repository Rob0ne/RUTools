using UnityEngine;

namespace RUT.Examples.Localization
{
    /// <summary>
    /// TextureLocAgentExample1 class.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class TextureLocAgentExample1 : LocAgentExample1
    {
        #region Private properties
        private Renderer _renderer;
        #endregion

        #region API
        public override void ReloadContent()
        {
            _renderer.material.SetTexture("_MainTex", _controller.GetTexture(ID));
        }
        #endregion

        #region Unity
        protected override void Awake()
        {
            base.Awake();
            _renderer = GetComponent<Renderer>();
        }
        #endregion
    }
}