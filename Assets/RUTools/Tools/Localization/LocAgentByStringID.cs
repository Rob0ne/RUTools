using UnityEngine;
using RUT.Utilities.Identification;

namespace RUT.Tools.Localization
{
    /// <summary>
    /// StringLocAgent class.
    /// </summary>
    public abstract class LocAgentByStringID : MonoBehaviour, ILocalizationAgent<string>
    {
        #region Public properties
        [SerializeField] string localID;
        [SerializeField] StringID objectID;
        [SerializeField] IDType idType = IDType.Local;

        public string ID
        {
            get
            {
                switch (idType)
                {
                    case IDType.Object:
                        return objectID.ID;
                    default:
                        return localID;
                }
            }
        }
        #endregion

        #region API
        public abstract void ReloadContent();
        #endregion

        #region SubType
        public enum IDType : uint
        {
            Local = 0,
            Object
        }
        #endregion
    }
}