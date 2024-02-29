using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Framework
{
    public class SceneTransition : MonoSingletons<SceneTransition>
    {
        public enum Event { OnAnimStart, OnAnimEnterEnd, OnAnimEnd }
        public enum State { Idle, Entering, HalfTime, Exiting }
        public enum TransType { Normal, SceneLoad }

        public ITransAnim defaultTransAnim => gameObject.GetComponent<DefaultTransAnim>();

        private ITransAnim transAnim;
        private event System.Action m_OnAnimStart, m_OnAnimEnterEnd, m_OnAnimEnd;
        public State state { get; set; }

        public VoidEventSO OnSceneUnload;

        #region UnityEvent

        protected override void Awake()
        {
            base.Awake();
            SetDontDestroyOnLoad();
            transAnim = gameObject.AddComponent<DefaultTransAnim>();
        }

        #endregion

        #region public Animation interface

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

        #region Settings

        public void SetTransAnim(ITransAnim transAnim)
        {
            this.transAnim = transAnim;
        }

        #endregion

        #region public Func

        public void DoTransition(IEnumerator halfTimeCoroutine = null, System.Action onAnimStart = null, System.Action onAnimEntered=null, System.Action onAnimExited=null, TransType transType=TransType.Normal)
        {
            if (state != State.Idle) return;

            if (transType == TransType.SceneLoad)
                OnSceneUnload?.Raise();

            SetEvent(Event.OnAnimStart, onAnimStart);
            SetEvent(Event.OnAnimEnterEnd, onAnimEntered);
            SetEvent(Event.OnAnimEnd, onAnimExited);
            StartCoroutine(M_BaseTransFlow(halfTimeCoroutine));
        }

        //public void LoadScene(GlobalData.SceneIndex sceneIndex)
        //{
        //    if (state != State.Idle) return;

        //    SetEvent(Event.OnAnimEnterEnd, () => SceneManager.LoadScene((int)sceneIndex));
        //    StartCoroutine(M_BaseTransFlow(null));
        //}

        //public void LoadSceneAsync(GlobalData.SceneIndex sceneIndex)
        //{
        //    if (state != State.Idle) return;

        //    StartCoroutine(M_BaseTransFlow(Coroutine_LoadSceneAsync((int)sceneIndex)));
        //}

        //public void LoadSceneAsync(string sceneName)
        //{
        //    if (state != State.Idle) return;

        //    StartCoroutine(M_BaseTransFlow(Coroutine_LoadSceneAsync(sceneName)));
        //}

        //private IEnumerator Coroutine_LoadSceneAsync(int sceneIndex)
        //{
        //    var operation = SceneManager.LoadSceneAsync(sceneIndex);
        //    while (!operation.isDone) yield return null;
        //}

        //private IEnumerator Coroutine_LoadSceneAsync(string sceneName)
        //{
        //    var operation = SceneManager.LoadSceneAsync(sceneName);
        //    while (!operation.isDone) yield return null;
        //}


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
    }
}