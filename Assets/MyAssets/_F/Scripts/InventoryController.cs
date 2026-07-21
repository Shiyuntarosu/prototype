using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public event Action OnInventoryChanged;     // インベントリ変更検知
    public event Action OnSelectedItemChanged; // 選択中アイテム変更検知

    [SerializeField] public ItemContainer ItemContainer { get; private set; }

    [SerializeField] public int inventorySize { get; private set; } = 5;

    public int SelectedIndex { get; private set; } = 0;

    public ItemSlot PeekSelectedItemSlot => ItemContainer.Slots[SelectedIndex];

    void Awake()
    {
        ItemContainer = new ItemContainer(inventorySize);
        ItemContainer.OnChanged += NotifyChanged;
    }

    public void NotifyChanged()
    {
        OnInventoryChanged?.Invoke();
        OnSelectedItemChanged?.Invoke();
    }

    // アイテムスロット切り替え
    public void ChangeItemSlot(int value)
    {
        SelectedIndex += value;
        if (SelectedIndex < 0)
        {
            SelectedIndex = ItemContainer.Size - 1;
        }
        if (SelectedIndex >= ItemContainer.Size)
        {
            SelectedIndex = 0;
        }

        OnSelectedItemChanged?.Invoke();    // 選択中のアイテムが更新された
    }


    // インベントリにアイテムを追加する
    public bool TryAddItem(ItemInstance item, int amount)
    {
        if (item == null) return false;
        if (ItemContainer.TryAddItem(item, amount))
        {
            NotifyChanged();
            return true;
        }
        return false;
    }

    // インベントリからアイテムを取り出す
    public ItemSlot TakeSelectedItem(int amount = 1)
    {
        ItemSlot result = ItemContainer.TryTakeItem(SelectedIndex, amount);
        if (!result.IsEmpty)
        {
            NotifyChanged();
        }
        return result;
    }

    // アイテムを拾う
    public bool PickUpItem(ItemInstance item)
    {
        // インベントリにアイテムを追加
        if (!TryAddItem(item, 1))
        {
            Debug.Log("アイテムいっぱい");
            return false;
        }
        Debug.Log(item.data.itemName + "を拾った");
        return true;
    }
}