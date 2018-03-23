using UnityEngine;
using UnityEngine.UI;
using RUT.Tools.Event.CEvent;

namespace RUT.Examples.Event
{
    /// <summary>
    /// ExampleInitializer class.
    /// </summary>
    public class ExampleInitializer : MonoBehaviour
    {
        #region Public properties
        public Button globalCEventButton;
        public Button localCEventButton;
        public Button observerButton;
        [Space(5)]
        public EventsExample1 example1;
        #endregion

        #region Unity
        private void Awake()
        {
            globalCEventButton.onClick.AddListener(() =>
            {
                //Dispatch global event.
                GlobalCEvent.Instance.DispatchCEvent(null, GlobalCEvent.ID_OnEventTriggered,
                    new EventsExample1.CEventArgExample(Color.red, "Global CEvent"));
            });

            localCEventButton.onClick.AddListener(example1.DispatchLocalCEvent);

            observerButton.onClick.AddListener(example1.NotifyObservable);
        }
        #endregion
    }
}