using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class Timer : MonoSingletons<Timer>
    {
        public void SetTimer(System.Action call, float delayTime)
        {
            StartCoroutine(Corou_SetTimer(call, delayTime));
        }

        private IEnumerator Corou_SetTimer(System.Action call, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            call?.Invoke();
        }
    }
}