using UnityEngine;

namespace RUT.Tools.Localization
{
    public delegate void LocalizationCallback();

    /// <summary>
    /// ILocalizationController interface.
    /// </summary>
    public interface ILocalizationController<T>
    {
        event LocalizationCallback OnLanguageChanged;

        bool SetLanguage(T language);

        string GetText(T id);
        AudioClip GetAudio(T id);
        Texture GetTexture(T id);
    }
}
