using UnityEngine;

namespace RUT.Tools.Localization
{
    /// <summary>
    /// LocalizationController by string ID asset class.
    /// </summary>
    [CreateAssetMenu(fileName = "LocalizationControllerByStringID", menuName = "RUTools/Localization/Controller by String ID", order = 0)]
    public class LocalizationControllerByStringIDAsset : ScriptableObject, ILocalizationController<string>
    {
        #region Public properties
        [SerializeField] LocalizationControllerByStringID controller = new LocalizationControllerByStringID();

        public event LocalizationCallback OnLanguageChanged
        {
            add { controller.OnLanguageChanged += value; }
            remove { controller.OnLanguageChanged -= value; }
        }
        #endregion

        #region API
        public bool SetLanguage(string language)
        {
            return controller.SetLanguage(language);
        }

        public string GetText(string id)
        {
            return controller.GetText(id);
        }

        public AudioClip GetAudio(string id)
        {
            return controller.GetAudio(id);
        }

        public Texture GetTexture(string id)
        {
            return controller.GetTexture(id);
        }

        public void CreateTextFileTemplate(string path)
        {
            controller.CreateTextFileTemplate(path);
        }
        #endregion
    }
}