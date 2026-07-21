public class ItemSlot
{
    public ItemInstance item;
    public int count;

    public bool IsEmpty => item == null;

    public ItemSlot(ItemInstance _item = null, int _count = 0)
    {
        item = _item;
        count = _count;
    }

    public void Set(ItemInstance item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public void Clear()
    {
        item = null;
        count = 0;
    }

    public void CopyFrom(ItemSlot other)
    {
        item = other.item;
        count = other.count;
    }

    public void Swap(ItemSlot other)
    {
        ItemInstance tempItem = item;
        int tempCount = count;

        item = other.item;
        count = other.count;

        other.item = tempItem;
        other.count = tempCount;
    }

    public bool CanStackWith(ItemSlot other)
    {
        if (IsEmpty || other.IsEmpty)
            return false;

        return item.CanStackWith(other.item);
    }
    
    public int RemainingStack
    {
        get
        {
            if (IsEmpty)
                return 0;

            return item.data.maxStack - count;
        }
    }
}