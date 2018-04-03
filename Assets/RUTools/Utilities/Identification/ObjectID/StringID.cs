using UnityEngine;

namespace RUT.Utilities.Identification
{
    /// <summary>
    /// StringID class.
    /// </summary>
    [CreateAssetMenu(fileName = "StringID", menuName = "RUTools/Identification/Object ID/String ID", order = 0)]
    public class StringID : ScriptableObject, IObjectID<string>
    {
        #region Public properties
        [SerializeField] string id = "EMPTY";

        public string ID
        { get { return id; } }
        #endregion
    }
}