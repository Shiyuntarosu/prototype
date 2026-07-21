using System;
using System.ComponentModel;

[Serializable]
public abstract class ItemFeatureData
{
    public abstract ItemFeature CreateFeature();
}

[Serializable]
public class ContainerFeatureData : ItemFeatureData
{
    public int maxCapacity = 16;

    public override ItemFeature CreateFeature()
    {
        return new ContainerFeature(maxCapacity);
    }
}