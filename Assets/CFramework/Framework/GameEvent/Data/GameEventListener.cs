using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class GameEventListener : MonoBehaviour
    {
        public Data.GameEventData eventData;
        public UnityEngine.Events.UnityEvent onEventTriggered;

        private void OnEnable()
        {
            eventData.AddListener(this);
        }

        private void OnDisable()
        {
            eventData.RemoveListener(this);
        }

        public void OnEventTriggered()
        {
            onEventTriggered?.Invoke();
        }
    }
}