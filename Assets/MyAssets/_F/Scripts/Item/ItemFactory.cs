
public static class ItemFactory
{
    public static ItemInstance Create(ItemData data)
    {
        ItemInstance item = new ItemInstance(data);

        // featureを生成
        foreach (var feature in data.features)
        {
            item.features.Add(feature.CreateFeature());
        }

        return item;
    }
}
