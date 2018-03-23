using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RUT.Tools.Event.CEvent;
using RUT.Tools.Event.Observer;

namespace RUT.Examples.Event
{
    /// <summary>
    /// EventsExample1 class.
    /// </summary>
    public class EventsExample1 : MBEventDispatcher
    {
        #region Public properties
        public Text outputText;
        public Renderer subject;
        #endregion

        #region Private properties
        private Color _baseColor;
        private Coroutine _colorChangeCR;

        private Observable<ObserverArg> _observable = new Observable<ObserverArg>();

        //Local event id. It's up to you how you want to organize the id storage. Personally,
        //I declare local event ids inside the concerned object only and global event ids inside
        //the GlobalCEvent singleton class. That way, anyone can access the global event ids but
        //only the concerned object can have access to the local ones.
        private static readonly string ID_OnEventTriggered = "EventsExample1_OnEventTriggered";
        #endregion

        #region API
        public void DispatchLocalCEvent()
        {
            this.DispatchCEvent(this, ID_OnEventTriggered, new CEventArgExample(Color.blue, "Local CEvent"));
        }

        public void NotifyObservable()
        {
            ObserverArg args = new ObserverArg(Color.green, "Observer event");
            _observable.Notify(ref args);
        }
        #endregion

        #region Unity
        private void Awake()
        {
            _baseColor = subject.material.color;

            //Technically speaking, this "global" event is in fact a local one on the singleton
            //GlobalCEvent. The reason I call it "global" is because that singleton's purpose
            //is to act like a global object that any script can access and add, remove or
            //dispatch events. So it is more like, the instance of that object is global.
            //
            //If you don't like the idea of having a singleton and still want an object to act
            //like a global event dispatcher. You can declare your own non-singleton class that
            //inherit from EventDispatcher and get its reference by injection whenever needed.
            GlobalCEvent.Instance.AddCEventListener(GlobalCEvent.ID_OnEventTriggered, OnEventTriggeredCE);

            //Local events need to have the object's reference to add, remove or dispatch.
            //For good practive, local events should only be dispatched by the object owning
            //them. In other words, EventsExample1 has 1 event "ID_OnEventTriggered", you should
            //not get EventsExample1's reference in any other script in order to call its
            //DispatchCEvent() function. Only EventsExample1 should call it.
            this.AddCEventListener(ID_OnEventTriggered, OnEventTriggeredCE);

            //Observers can be used with or without arguments. Use Observer or Observer<T>.
            Observer <ObserverArg> observer = new Observer<ObserverArg>();
            //Observers must be subscribed to a observable.
            _observable.Subscribe(observer);
            //You can add/remove actions to an observer whenever you want.
            observer.AddAction(OnObservableNotified);
        }
        #endregion

        #region Private methods
        //CEvent function
        private void OnEventTriggeredCE(ICEventDispatcher dispatcher, ICEventArgs args)
        {
            CEventArgExample argExample = args as CEventArgExample;
            if (argExample == null)
                return;

            if (_colorChangeCR != null)
                StopCoroutine(_colorChangeCR);

            _colorChangeCR = StartCoroutine(ChangeSubjectColorProcess(argExample.color, argExample.text, 2));
        }

        //Observer function
        private void OnObservableNotified(ref ObserverArg args)
        {
            if (_colorChangeCR != null)
                StopCoroutine(_colorChangeCR);

            _colorChangeCR = StartCoroutine(ChangeSubjectColorProcess(args.color, args.text, 2));
        }

        //Coroutine
        private IEnumerator ChangeSubjectColorProcess(Color col, string output, float time)
        {
            outputText.text = output;
            subject.material.color = col;
            yield return new WaitForSeconds(time);
            subject.material.color = _baseColor;
            outputText.text = "";
        }
        #endregion

        #region SubType
        //You can create your own argument classes for CEvents by implementing the ICEventArgs interface.
        public class CEventArgExample : ICEventArgs
        {
            public Color color;
            public string text;

            public CEventArgExample(Color color, string text)
            {
                this.color = color;
                this.text = text;
            }
        }

        public struct ObserverArg
        {
            public Color color;
            public string text;

            public ObserverArg(Color color, string text)
            {
                this.color = color;
                this.text = text;
            }
        }
        #endregion
    }
}