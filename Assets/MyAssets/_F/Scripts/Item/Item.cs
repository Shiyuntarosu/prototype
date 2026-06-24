using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField]
    private ItemData itemData;

    public void OnInteract(GameObject _player)
    {
        Debug.Log(gameObject.name + ":" + _player.name + "がインタラクト開始");
    }

    public ItemData GetItemData()
    {
        return itemData;
    }
}
