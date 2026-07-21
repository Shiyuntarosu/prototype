public abstract class ItemFeature
{
}

public class ContainerFeature : ItemFeature
{
    public ItemContainer Container { get; private set; }

    public ContainerFeature(int maxCapacity = 5)
    {
        Container = new ItemContainer(maxCapacity);
    }

    public bool TryStore(ItemInstance item, int amount)
    {
        return Container.TryAddItem(item, amount);
    }
}
