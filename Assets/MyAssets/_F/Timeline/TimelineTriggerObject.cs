using UnityEngine;
using UnityEngine.Playables;

public class TimelineTriggerObject : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayableDirector m_PlayableDirector;
    private GameObject _player;

    // インタラクト時
    public void OnInteract(GameObject player)
    {
        if (m_PlayableDirector.state != PlayState.Playing)
        {
            _player = player;
            // タイムライン再生
            m_PlayableDirector.Play();
        }
    }

    public void SetActionMap_FixedCamera()
    {
        Debug.Log("スタート");
        if (_player != null)
        {
            _player.TryGetComponent(out MyCustomPlayer playerComponent);
            //playerComponent.SetActionMap_FixedCamera();
        }
    }

    public void EndTimeline()
    {
        Debug.Log("エンド");
        if (_player != null)
        {
            _player.TryGetComponent(out MyCustomPlayer playerComponent);
            playerComponent.SetActionMap_Player();
        }
    }
}
