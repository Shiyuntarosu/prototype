using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class InteractTimeLine : MonoBehaviour, IInteractable, ITimeControl
{
    public GameObject _timeline;
    public CinemachineCamera _camera;

    public void ReceiveInteract(GameObject _player)
    {
        Debug.Log(gameObject.name + ":" + _player.name + "がインタラクト");
        _timeline.TryGetComponent(out PlayableDirector component);
        if (component != null)
        {
            component.Play();
        }
    }

    public void OnControlTimeStart()
    {
        Debug.Log("タイムラインスタート");
        _camera.Priority = 50;
    }

    public void OnControlTimeStop()
    {
        Debug.Log("タイムラインストップ");
        _camera.Priority = 0;
    }

    public void SetTime(double time)
    {
    }
}
