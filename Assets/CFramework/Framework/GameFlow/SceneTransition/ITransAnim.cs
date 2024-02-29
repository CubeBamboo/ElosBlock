using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public interface ITransAnim
    {
        /// <summary>
        /// play anim and do oncomplete callback ()=>_transition.OnAnimEnterEnd();
        /// </summary>
        public abstract void DoEnterAnim();

        /// <summary>
        /// play anim sync and do oncomplete callback ()=>_transition.OnAnimHalfTimeEnd();
        /// </summary>
        public abstract void DoHalfTimeAnim();

        /// <summary>
        /// play anim and do oncomplete callback ()=>_transition.OnAnimExitEnd();
        /// </summary>
        public abstract void DoExitAnim();
    }

    public abstract class AbstractTransAnim : ITransAnim
    {
        public SceneTransition _transition => SceneTransition.Instance;

        public abstract void DoEnterAnim();
        public abstract void DoHalfTimeAnim();
        public abstract void DoExitAnim();
    }
}