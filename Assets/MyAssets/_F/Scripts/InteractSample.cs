using UnityEngine;

public class InteractSample : MonoBehaviour, IInteractable
{
    public GameEvent interactEvent;

    public void OnInteract(GameObject _player)
    {
        interactEvent.Raise();
        Debug.Log(gameObject.name + ":" + _player.name + "がインタラクト開始");
    }

    public void OnInteractHold(GameObject _player)
    {
        Debug.Log(gameObject.name + ":" + _player.name + "がインタラクト中");
    }

    public void OnInteractRelease(GameObject _player, float time)
    {
        Debug.Log(gameObject.name + ":" + _player.name + "が" + time + "秒インタラクト");
    }

    public void SayHello()
    {
        Debug.Log("Hello");
    }

    public void SayGoodbye()
    {
        Debug.Log("GoodBye");
    }
}
