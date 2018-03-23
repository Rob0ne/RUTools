using UnityEngine;

namespace RUT.Tools.Event.CEvent
{
    /// <summary>
    /// MBCEventDispatcher base class inherited from MonoBehaviour.
    /// </summary>
    public abstract class MBEventDispatcher : MonoBehaviour, ICEventDispatcher
    {
        #region Private properties
        private CEDComponent _ced = new CEDComponent();
        #endregion

        #region API
        public void AddCEventListener(string type, CEventCallback callback, bool once = false)
        {
            _ced.AddCEventListener (type, callback, once);
        }
        public void RemoveCEventListener(string type)
        {
            _ced.RemoveCEventListener (type);
        }
        public void RemoveCEventListener(string type, CEventCallback callback)
        {
            _ced.RemoveCEventListener (type, callback);
        }
        public void RemoveAllCEventListeners()
        {
            _ced.RemoveAllCEventListeners();
        }
        public void DispatchCEvent(ICEventDispatcher dispatcher, string type, ICEventArgs args)
        {
            _ced.DispatchCEvent (dispatcher, type, args);
        }
        public bool HasCEventListener(string type)
        {
            return _ced.HasCEventListener (type);
        }
        #endregion
    }
}