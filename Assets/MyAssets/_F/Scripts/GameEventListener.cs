using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent eventToListen;
    [SerializeField] private UnityEvent response;

    private void OnEnable()
    {
        eventToListen?.Subscribe(this);
    }

    private void OnDisable()
    {
        eventToListen?.Unsubscribe(this);
    }

    public void OnEventRaised()
    {
        response?.Invoke();
    }
}