using System;
using System.Collections.Generic;

namespace Events
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> EventTable = new();

        public static void Subscribe<T>(Action<T> listener)
        {
            Type eventType = typeof(T);

            if (EventTable.TryGetValue(eventType, out Delegate existingDelegate))
            {
                EventTable[eventType] = Delegate.Combine(existingDelegate, listener);
            }
            else
            {
                EventTable[eventType] = listener;
            }
        }

        public static void Unsubscribe<T>(Action<T> listener)
        {
            Type eventType = typeof(T);

            if (!EventTable.TryGetValue(eventType, out Delegate existingDelegate))
            {
                return;
            }

            Delegate newDelegate = Delegate.Remove(existingDelegate, listener);

            if (newDelegate == null)
            {
                EventTable.Remove(eventType);
            }
            else
            {
                EventTable[eventType] = newDelegate;
            }
        }

        public static void Publish<T>(T eventData)
        {
            Type eventType = typeof(T);

            if (EventTable.TryGetValue(eventType, out Delegate existingDelegate) &&
                existingDelegate is Action<T> callback)
            {
                callback.Invoke(eventData);
            }
        }

        public static void Clear()
        {
            EventTable.Clear();
        }
    }
}