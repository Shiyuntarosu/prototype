using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

public class OpenWithInteractHold : MonoBehaviour, IInteractable
{
    public PlayableDirector timeline;
    public float moveSpeed;
    private float value;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnInteractHold()
    {
        value += moveSpeed * Time.deltaTime;
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        if (math.abs(value) >= 1.0f)
        {
            timeline.Play();
        }
    }
}
