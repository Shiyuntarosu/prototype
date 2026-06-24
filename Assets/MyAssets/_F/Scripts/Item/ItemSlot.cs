[System.Serializable]
public class ItemSlot
{
    public ItemData item;
    public int count;

    public bool IsEmpty => item == null;

    public ItemSlot(ItemData _item = null, int _count = 0)
    {
        item = _item;
        count = _count;
    }
}