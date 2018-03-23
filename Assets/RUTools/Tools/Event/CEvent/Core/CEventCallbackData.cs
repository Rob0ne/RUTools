using System.Collections.Generic;

namespace RUT.Tools.Event.CEvent
{
    public delegate void CEventCallback(ICEventDispatcher dispatcher, ICEventArgs args);

    /// <summary>
    /// Struct containing all data needed for callback processing.
    /// </summary>
    public struct CEventCallbackData
    {
        #region Public properties
        public CEventCallback callback;
        public bool calledOnce;
        #endregion

        #region API
        public CEventCallbackData(CEventCallback callback, bool calledOnce)
        {
            this.callback = callback;
            this.calledOnce = calledOnce;
        }
        #endregion
    }

    /// <summary>
    ///	Custom comparer for CEventFunctionData.
    /// </summary>
    public class CEventCallbackDataComparer : IEqualityComparer<CEventCallbackData>
    {
        public bool Equals(CEventCallbackData a, CEventCallbackData b)
        {
            return a.callback.Equals(b.callback);
        }

        public int GetHashCode(CEventCallbackData item)
        {
            return item.callback.GetHashCode ();
        }
    }
}