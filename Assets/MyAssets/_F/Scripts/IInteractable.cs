using UnityEngine;

public interface IInteractable
{
    // ボタンを押したとき
    public virtual void OnInteract() { }

    public virtual void OnInteract(GameObject _player) { }

    // ホールド中
    public virtual void OnInteractHold() { }
    public virtual void OnInteractHold(GameObject _player) { }
    public virtual void OnInteractHold(float holdTime) { }
    public virtual void OnInteractHold(GameObject _player, float holdTime) { }

    // ボタンを離した時

    public virtual void OnInteractRelease() { }
    public virtual void OnInteractRelease(GameObject _player) { }
    public virtual void OnInteractRelease(float holdTime) { }
    public virtual void OnInteractRelease(GameObject _player, float holdTime) { }
}
