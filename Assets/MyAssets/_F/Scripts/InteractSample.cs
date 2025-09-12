using UnityEngine;

public class InteractSample : MonoBehaviour, IInteractable
{
    public void ReceiveInteract(GameObject _player)
    {
        Debug.Log(gameObject.name + ":" + _player.name + "がインタラクト");
    }
}
