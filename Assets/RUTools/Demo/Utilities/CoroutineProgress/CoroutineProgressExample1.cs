using UnityEngine;
using RUT.Utilities;
using System;

namespace RUT.Examples.Utilities
{
    /// <summary>
    /// CoroutineProgressExample1 class.
    /// </summary>
    public class CoroutineProgressExample1 : MonoBehaviour
    {
        #region Public properties
        public SubjectData[] subjects;
        #endregion

        #region Unity
        private void Start()
        {
            for(int i = 0; i < subjects.Length; ++i)
            {
                MoveSubject(subjects[i]);
            }
        }
        #endregion

        #region Private methods
        private void MoveSubject(SubjectData data)
        {
            GameObject subject = data.subject;
            float time = data.timePointToPoint;
            UEase.EaseType ease = data.ease;
            Vector3 startPos = data.reverse ? data.p2.transform.position : data.p1.transform.position;
            Vector3 endPos = data.reverse ? data.p1.transform.position : data.p2.transform.position;

            Action<float> loop = (t) =>
            {
                Vector3 newPos;

                newPos.x = UEase.GetEaseFunction(ease)(startPos.x, endPos.x, t);
                newPos.y = UEase.GetEaseFunction(ease)(startPos.y, endPos.y, t);
                newPos.z = UEase.GetEaseFunction(ease)(startPos.z, endPos.z, t);

                subject.transform.position = newPos;
            };

            Action end = () =>
            {
                subject.transform.position = endPos;

                //Normally CoroutineProgress should only be used to do one action progression and
                //not loop around another CoroutineProgress. Here I am creating an infinite loop
                //just for the sake of this example. It creates a new coroutine every time it
                //loops, making it not a very good approach for this kind of situations. You should
                //probably create your own coroutine with an infinite loop for better performance.
                data.reverse = !data.reverse;
                MoveSubject(data);
            };

            MonoBehaviourExtension.CoroutineProgressData progressData =
                   new MonoBehaviourExtension.CoroutineProgressData
                   (
                       MonoBehaviourExtension.CoroutineProgressType.WaitForFixedUpdate
                   );

            this.CoroutineProgress(time, loop, end, progressData);
        }
        #endregion

        #region SubType
        [System.Serializable]
        public class SubjectData
        {
            public GameObject subject;
            public Transform p1;
            public Transform p2;
            public UEase.EaseType ease;
            public float timePointToPoint;
            [HideInInspector] public bool reverse;
        }
        #endregion
    }
}