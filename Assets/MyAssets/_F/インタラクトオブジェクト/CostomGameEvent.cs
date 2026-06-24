using UnityEngine;

public abstract class CostomGameEvent : MonoBehaviour
{
    public bool isDone;

    public abstract void Initialize();

    public abstract void OnInteract();

    public abstract void RunningUpdate();

    public abstract void OnComplete();
}
