using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RUT.Tools.Localization
{
    /// <summary>
    /// ILocalizationAgent interface.
    /// </summary>
    public interface ILocalizationAgent<T>
    {
        T ID { get; }

        void ReloadContent();
    }
}