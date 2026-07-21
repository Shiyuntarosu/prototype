public readonly struct SlotReference
{
    public ItemContainer Container { get; }
    public int Index { get; }

    public SlotReference(ItemContainer container, int index)
    {
        Container = container;
        Index = index;
    }

    public ItemSlot Slot => Container.PeekSlot(Index);

    public bool IsValid =>
        Container != null &&
        Index >= 0 &&
        Index < Container.SlotCount;

    public bool IsFrom(ItemContainer container)
    {
        return Container == container;
    }
}