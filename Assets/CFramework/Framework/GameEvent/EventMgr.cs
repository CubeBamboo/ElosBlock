using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// a global event system
    /// </summary>
    public class EventMgr : Framework.MonoSingletons<EventMgr>
    {
        protected override void Awake()
        {
            base.Awake();
            SetDontDestroyOnLoad();
        }

        #region CustomClass

        //TODO: how to maintain it
        /// <summary>
        /// Record name of events
        /// </summary>
        public enum EventName
        {  }

        #endregion

        #region EventManager

        private Dictionary<EventName, System.Action> eventDict = new Dictionary<EventName, System.Action>();

        public void AddEvent(EventName eventName, System.Action handler)
        {
            if (eventDict.ContainsKey(eventName))
                eventDict[eventName] += handler;
            else
                eventDict.Add(eventName, handler);
        }
        public void RemoveEvent(EventName eventName, System.Action handler)
        {
            if (eventDict.ContainsKey(eventName))
                eventDict[eventName] -= handler;
        }
        public void TriggerEvent(EventName eventName)
        {
            if (eventDict.ContainsKey(eventName))
                eventDict[eventName]?.Invoke();
        }

        public void Clear()
        {
            eventDict.Clear();
        }

        #endregion

    }
}