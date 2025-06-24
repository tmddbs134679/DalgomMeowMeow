using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEvent { }

public static class EventBus
{
    private static readonly Dictionary<Type, Action<BaseEvent>> listeners = new();

    public static Action<BaseEvent> Subscribe<T>(Action<T> callback) where T : BaseEvent
    {
        Type eventType = typeof(T);
        if (!listeners.ContainsKey(eventType))
            listeners[eventType] = _ => { };

        Action<BaseEvent> wrapper = (e) => callback((T)e);
        listeners[eventType] += wrapper;
        return wrapper;
    }

    public static void Unsubscribe<T>(Action<BaseEvent> wrapper) where T : BaseEvent
    {
        Type eventType = typeof(T);
        if (listeners.ContainsKey(eventType))
            listeners[eventType] -= wrapper;
    }

    public static void Publish(BaseEvent e)
    {
        Type eventType = e.GetType();
        if (listeners.TryGetValue(eventType, out var callback))
        {
            callback.Invoke(e);
        }
    }
}