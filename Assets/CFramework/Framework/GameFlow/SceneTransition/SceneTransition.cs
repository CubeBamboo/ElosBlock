using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Framework.GameFlow
{
    [RequireComponent(typeof(ITransAnim))]
    public class SceneTransition : MonoSingletons<SceneTransition>
    {
        public enum Event { OnAnimStart, OnAnimEnterEnd, OnAnimEnd }
        public enum State { Idle, Entering, HalfTime, Exiting }

        private ITransAnim transAnim;
        private event System.Action m_OnAnimStart, m_OnAnimEnterEnd, m_OnAnimEnd;
        public State state { get; set; }

        #region UnityEvent

        protected override void Awake()
        {
            base.Awake();
            SetDontDestroyOnLoad();
        }

        private void Start()
        {
            transAnim = GetComponent<ITransAnim>();
        }

        #endregion

        #region public interface

        public void OnAnimEnterEnd()
        {
            state = State.HalfTime;
        }
        public void OnAnimHalfTimeEnd()
        {
            state = State.Exiting;
        }
        public void OnAnimExitEnd()
        {
            state = State.Idle;
        }

        #endregion

        #region publicFunc

        public void DoTransition(System.Action onAnimStart = null, System.Action onAnimEntered=null, System.Action onAnimExited=null)
        {
            if (state != State.Idle) return;

            SetEvent(Event.OnAnimStart, onAnimStart);
            SetEvent(Event.OnAnimEnterEnd, onAnimEntered);
            SetEvent(Event.OnAnimEnd, onAnimExited);
            StartCoroutine(M_BaseTransFlow(null));
        }

        /// <summary>
        /// load scene async with transition
        /// </summary>
        public void LoadScene(GlobalData.SceneIndex sceneIndex)
        {
            if (state != State.Idle) return;

            SetEvent(Event.OnAnimEnterEnd, () => SceneManager.LoadScene((int)sceneIndex));
            StartCoroutine(M_BaseTransFlow(null));
        }

        /// <summary>
        /// load scene async with transition
        /// </summary>
        public void LoadSceneAsync(GlobalData.SceneIndex sceneIndex)
        {
            if (state != State.Idle) return;

            StartCoroutine(M_BaseTransFlow(Coroutine_LoadSceneAsync((int)sceneIndex)));
        }

        public void SetEvent(Event eventEnum, System.Action call)
        {
            switch (eventEnum)
            {
                case Event.OnAnimEnterEnd:
                    m_OnAnimEnterEnd = null;
                    m_OnAnimEnterEnd += call;
                    break;
                case Event.OnAnimEnd:
                    m_OnAnimEnd = null;
                    m_OnAnimEnd += call;
                    break;
                case Event.OnAnimStart:
                    m_OnAnimStart = null;
                    m_OnAnimStart += call;
                    break;
                default:
                    Debug.LogError("eventEnum para error");
                    break;
            }
        }

        public void ResetEvent()
        {
            m_OnAnimEnterEnd = null;
            m_OnAnimEnd = null;
            m_OnAnimStart = null;
        }

        #endregion

        #region CoreFunc

        private IEnumerator M_BaseTransFlow(IEnumerator HalfTimeCoroutine)
        {
            m_OnAnimStart?.Invoke();  //event

            //Entering
            state = State.Entering;
            transAnim.DoEnterAnim();
            while (state != State.HalfTime) yield return null;

            //HalfTime
            m_OnAnimEnterEnd?.Invoke();    //event
            transAnim.DoHalfTimeAnim();
            if (HalfTimeCoroutine != null) yield return StartCoroutine(HalfTimeCoroutine); //event
            while (state != State.Exiting) yield return null;

            //Exiting
            transAnim.DoExitAnim();
            while (state != State.Idle) yield return null;

            //Trans End , And do some destruct work...
            m_OnAnimEnd?.Invoke(); //event
            ResetEvent();
        }

        private IEnumerator Coroutine_LoadSceneAsync(int sceneIndex)
        {
            var operation = SceneManager.LoadSceneAsync(sceneIndex);
            while (!operation.isDone) yield return null;
        }

        #endregion

        //private void M_LoadSceneAsync(int sceneIndex)
        //{
        //    StartCoroutine(M_BaseTransFlow(Coroutine_LoadSceneAsync(sceneIndex)));
        //}

        //private void M_DoTransition()
        //{
        //    StartCoroutine(M_BaseTransFlow(null));
        //}

        //public void DoTransition(IEnumerator onAnimEnterEnd)
        //{
        //    if (state != State.Idle) return;

        //    StartCoroutine(M_DoTransition(onAnimEnterEnd));
        //}

        //private IEnumerator M_DoTransition(IEnumerator onAnimEnterEnd)
        //{
        //    state = State.Entering;

        //    transAnim.DoEnterAnim();
        //    while (state != State.HalfTime) yield return null;

        //    OnAnimEntered?.Invoke();
        //    yield return StartCoroutine(onAnimEnterEnd);

        //    state = State.Exiting;

        //    transAnim.DoExitAnim();
        //    while (state != State.Idle) yield return null;
        //    OnAnimExited?.Invoke();

        //    ResetEvent();
        //}

        //private void M_DoTransition()
        //{

        //    //state = State.Entering;

        //    //transAnim.DoEnterAnim();

        //    //while(state != State.HalfTime) yield return null;
        //    //OnAnimEntered?.Invoke();
        //    //state = State.Exiting;

        //    //transAnim.DoExitAnim();
        //    //while (state != State.Idle) yield return null;
        //    //OnAnimExited?.Invoke();

        //    //ResetEvent();

        //}

        //private void M_LoadSceneAsync(int sceneIndex)
        //{
        //    //state = State.Entering;

        //    //transAnim.DoEnterAnim();
        //    //while (state != State.HalfTime) yield return null;

        //    //OnAnimEntered?.Invoke();


        //    //state = State.Exiting;

        //    //transAnim.DoExitAnim();
        //    //while (state != State.Idle) yield return null;
        //    //OnAnimExited?.Invoke();

        //    //ResetEvent();
        //}
    }
}