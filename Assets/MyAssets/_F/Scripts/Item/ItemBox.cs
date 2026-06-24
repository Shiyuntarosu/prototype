using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string itemName;
    [SerializeField]
    private int maxSize;
    [SerializeField]
    private int size;
    [SerializeField]
    private List<ItemData> itemList;

    public int GetItemListCount()
    {
        if (itemList == null) return 0;
        return itemList.Count;
    }

    public void PutInItemBox(ItemData itemData)
    {
        // アイテムデータをリストに追加
        itemList.Add(itemData);
        Debug.Log(itemData.name + "を入れた");
    }

    public void OnInteract(GameObject _player)
    {
        Debug.Log(gameObject.name + ":" + _player.name + "がインタラクト開始");
    }
}
