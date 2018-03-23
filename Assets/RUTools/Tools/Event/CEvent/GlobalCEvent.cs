namespace RUT.Tools.Event.CEvent
{
    // Example:
    //
    // GlobalCEvent.Instance.AddCEventListener(GlobalCEvent.ID_OnEventTriggered, Callback)
    // //GlobalCEvent.ID_OnEventTriggered must be a declared string and Callback signature must be of
    // //type "void Callback(CEvent e)".
    //
    // GlobalCEvent.Instance.RemoveCEventListener(GlobalCEvent.ID_OnEventTriggered);
    //
    // GlobalCEvent.Instance.DispatchCEvent(new CEvent(GlobalCEvent.ID_OnEventTriggered, new ExampleArgs()));
    // //ExampleArgs class must implement ICEventArgs.



    /// <summary>
    /// Global custom event handler.
    /// </summary>
    public sealed partial class GlobalCEvent : EventDispatcher
    {
        //Example:

        public static readonly string ID_OnEventTriggered = "GlobalCEvent_OnEventTriggered";
    }



    // Singleton part.
    public sealed partial class GlobalCEvent : EventDispatcher
    {
        #region Public properties
        public static GlobalCEvent Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GlobalCEvent();

                return _instance;
            }
        }
        #endregion

        #region Private properties
        private static GlobalCEvent _instance = null;
        #endregion

        #region Private methods
        private GlobalCEvent()
        {
            if (_instance == null)
                _instance = this;
        }
        #endregion
    }
}
