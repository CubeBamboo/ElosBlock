using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.AI;

namespace Framework.GameFlow
{
    public class AnimatorTransAnim : MonoBehaviour, ITransAnim //TODO: abstrct class
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

        public string enterClipName = "TransEnter";
        public string exitClipName = "TransExit";

        private Animator _animator;
        private AnimationClip enterClip, exitClip;

        private void Awake()
        {
            _transition = GetComponent<SceneTransition>();
            _animator = GetComponent<Animator>();

            Debug.Assert(_animator);
        }

        private void Start()
        {
            InitClip();
        }

        private void InitClip()
        {
            var animClips = _animator.runtimeAnimatorController.animationClips;
            foreach (var animClip in animClips)
            {
                if (animClip.name == enterClipName) { enterClip = animClip; }
                if (animClip.name == exitClipName) { exitClip = animClip; }
            }
            Debug.Assert(enterClip && exitClip);
        }

        public void DoEnterAnim()
        {
            _animator.Play(enterClipName);

            //add event
            var evt = new AnimationEvent
            {
                functionName = "Evt_OnAnimEnterEnd",
                time = enterClip.length
            };
            enterClip.AddEvent(evt);
        }

        public void DoExitAnim()
        {
            _animator.Play(exitClipName);

            var evt = new AnimationEvent
            {
                functionName = "Evt_OnAnimExitEnd",
                time = exitClip.length
            };
            exitClip.AddEvent(evt);
        }

        public void DoHalfTimeAnim()
        {
            Evt_OnAnimHalfTimeEnd();
        }
    }
}