using System;
using System.Collections;
using System.Collections.Generic;

namespace chchch.Events
{
    [Serializable]
    public abstract class Ch3Event { };

    public static class Ch3EventManager
    {
        public delegate void EventDelegate<T>(T p_event) where T : Ch3Event;
        private static Dictionary<Type, Delegate> _eventHandlers = new Dictionary<Type, Delegate>();
        private static EventDelegate<Ch3Event> _globalEventHandler = delegate { };

        public static void DispatchEvent<T>(T p_event) where T : Ch3Event
        {
            Delegate eventHandler;

            _eventHandlers.TryGetValue(typeof(T), out eventHandler);

            if (eventHandler != null)
            {
                (eventHandler as EventDelegate<T>)(p_event);
            }

            _globalEventHandler(p_event);
        }

        public static void HookEvent<T>(EventDelegate<T> p_eventHandler) where T : Ch3Event
        {
            Type eventType = typeof(T);
            Delegate eventHandler;

            _eventHandlers.TryGetValue(eventType, out eventHandler);

            if (eventHandler != null)
            {
                eventHandler = (eventHandler as EventDelegate<T>) + p_eventHandler;
            }
            else
            {
                _eventHandlers.Add(eventType, eventHandler);
            }
        }

        public static void UnhookEvent<T>(EventDelegate<T> p_eventHandler) where T : Ch3Event
        {
            Type eventType = typeof(T);
            Delegate eventHandler;

            _eventHandlers.TryGetValue(eventType, out eventHandler);

            if (eventHandler != null)
            {
                eventHandler = (eventHandler as EventDelegate<T>) - p_eventHandler;

                if (eventHandler == null)
                {
                    _eventHandlers.Remove(eventType);
                }
            }
        }

        public static void HookGlobal(EventDelegate<Ch3Event> p_eventHandler)
        {
            _globalEventHandler += p_eventHandler;
        }

        public static void UnhookGlobal(EventDelegate<Ch3Event> p_eventHandler)
        {
            _globalEventHandler -= p_eventHandler;
        }
    }
}