namespace RUT.Tools.Event.CEvent
{
    /// <summary>
    /// Custom event dispatcher interface.
    /// </summary>
    public interface ICEventDispatcher
    {
        void AddCEventListener(string type, CEventCallback callback, bool once = false);
        void RemoveCEventListener(string type);
        void RemoveCEventListener(string type, CEventCallback callback);
        void RemoveAllCEventListeners();
        void DispatchCEvent(ICEventDispatcher dispatcher, string type, object arg);
        bool HasCEventListener(string type);
    }
}