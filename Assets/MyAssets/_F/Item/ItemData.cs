using System.Collections.Generic;
using UnityEngine;

public enum InstanceMode
{
    CreateOnPickup,
    CreateOnSpawn
}

[CreateAssetMenu(menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;

    public int id;

    public int maxStack = 1;

    public GameObject prefab;

    public Sprite icon;

    public Vector3 onHandOffset;   // 手に持った時の位置調整

    [SerializeReference, SubclassSelector] public List<ItemFeatureData> features = new();

    public InstanceMode instanceMode = InstanceMode.CreateOnPickup;
}
