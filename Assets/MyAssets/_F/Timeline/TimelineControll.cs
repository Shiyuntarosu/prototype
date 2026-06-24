using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineControll : MonoBehaviour, ITimeControl
{
    [SerializeField] private PlayableDirector m_playableDirector;
    [SerializeField] private bool m_resumeFlag;

    [Header("options")]
    [SerializeField] private bool m_interactable;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (m_resumeFlag && m_playableDirector.state == PlayState.Paused)
        {
            Debug.Log("再開");
            m_playableDirector.Resume();
        }
    }

#if m_interactable
    public void OnInteract(){
        
    }
#endif

    public void Initialize()
    {
        m_resumeFlag = false;
    }

    public void Play()
    {
        if (m_playableDirector.state != PlayState.Playing)
        {
            // タイムライン再生
            m_playableDirector.Play();
        }
    }

    public void OnControlTimeStart()
    {
    }

    public void OnControlTimeStop()
    {
    }

    public void SetTime(double time)
    {
        if (time > 3 && !m_resumeFlag)
        {
            Debug.Log("ポーズ");
            m_playableDirector.Pause();
        }
    }
}
