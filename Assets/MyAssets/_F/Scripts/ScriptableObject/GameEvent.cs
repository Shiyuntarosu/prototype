using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent")]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void Subscribe(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void Unsubscribe(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Raise()
    {
        foreach (var listener in listeners)
        {
            listener?.OnEventRaised();
        }
    }
}
