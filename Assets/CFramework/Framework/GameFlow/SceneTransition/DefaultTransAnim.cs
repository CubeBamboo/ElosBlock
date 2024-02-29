using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    //[RequireComponent(typeof(Image))] //TODO: maybe image in canvas?
    public class DefaultTransAnim : MonoBehaviour, ITransAnim //TODO: abstrct class
    {
        #region Common

        private SceneTransition _transition;

        public void Evt_OnAnimEnterEnd()
        {
            _transition.OnAnimEnterEnd();
        }
        public void Evt_OnAnimHalfTimeEnd()
        {
            _transition.OnAnimHalfTimeEnd();
        }
        public void Evt_OnAnimExitEnd()
        {
            _transition.OnAnimExitEnd();
        }

        #endregion

        [SerializeField] private Color targetColor = Color.black;
        private Canvas _canvas;
        private Image _panel;
        
        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _transition = SceneTransition.Instance;
            _panel = gameObject.AddComponent<Image>();
            //if (!_panel) Debug.LogWarning("SceneTransition don't have a target panel");

            Init();
        }

        private void Init()
        {
            _canvas.sortingOrder = 10;

            _panel.color = targetColor;
            var color = _panel.color;
            color.a = 0;
            _panel.color = color;

            _panel.raycastTarget = false;
        }

        public void DoEnterAnim()
        {
            StartCoroutine(PlayEnterAnim());
        }

        public void DoExitAnim()
        {
            StartCoroutine(PlayExitAnim());
        }

        public void DoHalfTimeAnim()
        {
            Evt_OnAnimHalfTimeEnd();
        }

        #region Coroutine

        private IEnumerator PlayEnterAnim()
        {
            float endTime = Time.time + 1f;
            var animCurve = AnimationCurve.EaseInOut(Time.time, 0f, endTime, 1f);
            while (Time.time < endTime)
            {
                _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, animCurve.Evaluate(Time.time));
                yield return null;
            }
            Evt_OnAnimEnterEnd();
        }

        private IEnumerator PlayExitAnim()
        {
            float endTime = Time.time + 1f;
            var animCurve = AnimationCurve.EaseInOut(Time.time, 1f, endTime, 0f);
            while (Time.time < endTime)
            {
                _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, animCurve.Evaluate(Time.time));
                yield return null;
            }
            Evt_OnAnimExitEnd();
        }

        #endregion
    }
}