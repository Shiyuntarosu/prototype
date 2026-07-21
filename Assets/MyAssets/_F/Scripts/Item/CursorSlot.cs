using System;

public class CursorSlot
{
    private readonly ItemSlot slot = new ItemSlot();

    public ItemSlot Slot => slot;
    public bool IsEmpty => slot.IsEmpty;

    public event Action Changed;

    public void Set(ItemInstance item, int count)
    {
        slot.Set(item, count);
        Changed?.Invoke();
    }

    public void Set(ItemSlot itemSlot)
    {
        slot.Set(itemSlot.item, itemSlot.count);
        Changed?.Invoke();
    }

    public void Clear()
    {
        slot.Clear();
        Changed?.Invoke();
    }

    public void CopyFrom(ItemSlot other)
    {
        slot.CopyFrom(other);
        Changed?.Invoke();
    }

    public void Swap(ItemSlot other)
    {
        slot.Swap(other);
        Changed?.Invoke();
    }

    public void Remove(int amount)
    {
        slot.count -= amount;

        if (slot.count <= 0)
        {
            slot.Clear();
        }

        Changed?.Invoke();
    }

    public void Add(int amount)
    {
        slot.count += amount;
        Changed?.Invoke();
    }
}