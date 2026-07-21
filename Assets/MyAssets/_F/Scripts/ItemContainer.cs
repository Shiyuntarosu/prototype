using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// アイテムを保持する
public class ItemContainer
{
    public event Action OnChanged;

    [SerializeField] private List<ItemSlot> slots; // インベントリ
    public IReadOnlyList<ItemSlot> Slots { get { return slots; } }  // 外部参照用（読み取り専用）
    public int SlotCount => slots.Count;
    private int size = 5;
    public int Size { get { return size; } }    // インベントリの大きさ

    public ItemContainer(int size)
    {
        slots = Enumerable.Range(0, size)
            .Select(_ => new ItemSlot())
            .ToList();
    }

    public void NotifyChanged()
    {
        OnChanged?.Invoke();
    }

    public bool TryAddItem(ItemInstance item, int amount)
    {
        return AddItem(new ItemSlot(item, amount)) == amount;
    }

    public int AddItem(ItemSlot sourceSlot)
    {
        int amount = sourceSlot.count;
        int originalAmount = amount;

        // 既存スタックへ追加
        foreach (ItemSlot slot in slots)
        {
            if (slot.IsEmpty)
                continue;

            if (slot.CanStackWith(sourceSlot) && slot.RemainingStack > 0)
            {
                int add = Mathf.Min(amount, slot.RemainingStack);

                slot.count += add;
                amount -= add;

                if (amount <= 0)
                {
                    OnChanged?.Invoke();
                    return originalAmount;
                }
            }
        }

        // 空スロットへ追加
        foreach (ItemSlot slot in slots)
        {
            if (!slot.IsEmpty)
                continue;

            int add = Mathf.Min(amount, sourceSlot.item.data.maxStack);

            slot.Set(sourceSlot.item, add);

            amount -= add;

            if (amount <= 0)
            {
                OnChanged?.Invoke();
                return originalAmount;
            }
        }

        if (amount != originalAmount)
        {
            OnChanged?.Invoke();
        }

        return originalAmount - amount;
    }

    public ItemSlot TryTakeItem(int index, int amount = 1)
    {
        // 取り出す個数
        int take = Mathf.Min(amount, slots[index].count);

        // 取り出すアイテム
        ItemSlot result = new ItemSlot(slots[index].item, take);
        if (result.IsEmpty)
            return result;

        // 取りだした分を減らす
        slots[index].count -= take;
        if (slots[index].count <= 0)
        {
            slots[index].Clear();
        }

        OnChanged?.Invoke();
        return result;
    }

    public ItemSlot PeekSlot(int index)
    {
        return slots[index];
    }

    public bool TryPlaceItem(int index, ItemSlot slot)
    {
        ItemSlot target = slots[index];

        if (!target.IsEmpty)
            return false;

        target.CopyFrom(slot);

        OnChanged?.Invoke();
        return true;
    }
}
