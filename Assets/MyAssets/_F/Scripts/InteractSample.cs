using UnityEngine;

public class InteractSample : MonoBehaviour, IInteractable
{
    public GameEvent interactEvent;

    public void ReceiveInteract(GameObject _player)
    {
        interactEvent.Raise();
        Debug.Log(gameObject.name + ":" + _player.name + "がインタラクト");
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
