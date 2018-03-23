using System.Collections.Generic;
using System.Linq;

namespace RUT.Tools.Event.CEvent
{
    /// <summary>
    /// Class containing all the reusable code for event dispatching.
    /// </summary>
    public class CEDComponent : ICEventDispatcher
    {
        #region Private properties
        private Dictionary<string, HashSet<CEventCallbackData>> _ceventSet = new Dictionary<string, HashSet<CEventCallbackData>> ();
        private List<CEventCallbackData> _removableList = new List<CEventCallbackData>();

        private static readonly CEventCallbackDataComparer _ceventCallbackdataComparer = new CEventCallbackDataComparer();
        #endregion

        #region API
        /// <summary>
        /// Adds an event listener of given "type".
        /// </summary>
        public void AddCEventListener(string type, CEventCallback callback, bool once = false)
        {
            if (callback == null)
                return;

            HashSet<CEventCallbackData> callbackSet;
            CEventCallbackData callbackData = new CEventCallbackData (callback, once);

            if (_ceventSet.TryGetValue (type, out callbackSet))
            {
                if (!callbackSet.Contains (callbackData))
                {
                    callbackSet.Add (callbackData);
                }
            }
            else
            {
                callbackSet = new HashSet<CEventCallbackData> (_ceventCallbackdataComparer);
                callbackSet.Add (callbackData);

                _ceventSet.Add (type, callbackSet);
            }
        }

        /// <summary>
        /// Removes the whole event listener of given "type".
        /// </summary>
        public void RemoveCEventListener(string type)
        {
            if (_ceventSet.ContainsKey (type))
            {
                _ceventSet.Remove (type);
            }
        }

        /// <summary>
        /// Removes the "callback" from the event listener "type". If the event listener is empty
        /// afterwards, removes it entirely.
        /// </summary>
        public void RemoveCEventListener(string type, CEventCallback callback)
        {
            HashSet<CEventCallbackData> callbackSet;

            if (_ceventSet.TryGetValue (type, out callbackSet))
            {
                CEventCallbackData callbackData = new CEventCallbackData (callback, false);
                callbackSet.Remove (callbackData);

                if (callbackSet.Count == 0)
                {
                    _ceventSet.Remove (type);
                }
            }
        }

        /// <summary>
        /// Removes all the event listeners.
        /// </summary>
        public void RemoveAllCEventListeners()
        {
            _ceventSet.Clear();
        }

        /// <summary>
        /// Dispatches an event. If there is no callbacks associated to the event, nothing happens.
        /// </summary>
        public void DispatchCEvent(ICEventDispatcher dispatcher, string type, ICEventArgs args)
        {
            HashSet<CEventCallbackData> callbackSet;

            if (_ceventSet.TryGetValue (type, out callbackSet))
            {
                foreach (CEventCallbackData data in callbackSet)
                {
                    data.callback.Invoke (dispatcher, args);

                    if (data.calledOnce)
                    {
                        _removableList.Add (data);
                    }
                }

                for (int i = 0; i < _removableList.Count; ++i)
                {
                    callbackSet.Remove (_removableList[i]);
                }

                _removableList.Clear();
            }
        }

        /// <summary>
        /// Returns true if an event listener of "type" exists.
        /// </summary>
        public bool HasCEventListener(string type)
        {
            return _ceventSet.ContainsKey(type);
        }
        #endregion
    }
}
