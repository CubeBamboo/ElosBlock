using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class TimeAction : MonoSingletons<TimeAction>
    {
        private Dictionary<string, Sequence> loopDict = new Dictionary<string, Sequence>();
        
        public Sequence LoopCall(string name, System.Action call, float interval = 0.1f)
        {
            var seq = DOTween.Sequence();
            seq.AppendCallback(() => call())
               .AppendInterval(interval)
               .SetLoops(-1);
            loopDict.Add(name, seq);
            return seq;
        }

        public bool StopLoopCall(string name)
        {
            if (!loopDict.ContainsKey(name)) return false;
            
            loopDict[name].Kill();
            loopDict.Remove(name);
            return true;
        }

        public void ClearAllLoopCall()
        {
            foreach (var item in loopDict)
            {
                item.Value.Kill();
            }

            loopDict.Clear();
        }

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