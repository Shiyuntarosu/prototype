using UnityEngine;
using System.Collections.Generic;

// ここに入るアイテムデータをもとにオブジェクトを生成する
[CreateAssetMenu(menuName = "Game/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField]
    private List<ItemData> items = new List<ItemData>();

    public IReadOnlyList<ItemData> Items => items;

    public ItemData GetItemById(int id)
    {
        return items.Find(item => item.itemId == id);
    }

}
