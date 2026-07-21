using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;
    private ItemInstance itemInstance;

    void Awake()
    {
        if (itemData.instanceMode == InstanceMode.CreateOnSpawn)
        {
            itemInstance = ItemFactory.Create(itemData);
        }
    }

    public void Initialize(ItemInstance instance)
    {
        itemInstance = instance;
        itemData = instance.data;
    }

    public virtual void OnInteract(GameObject _player)
    {
        Debug.Log(gameObject.name + ":" + _player.name + "がインタラクト開始");
    }
    public ItemInstance GetItemInstance()
    {
        if (itemInstance == null)
        {
            itemInstance = ItemFactory.Create(itemData);
        }

        return itemInstance;
    }
}
