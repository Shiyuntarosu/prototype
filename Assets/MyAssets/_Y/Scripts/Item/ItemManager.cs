using UnityEngine;

/// <summary>
/// 実際の生成自体はこのクラスで行う
/// </summary>
public class ItemManager : MonoBehaviour
{
    [SerializeField] private ItemFactory itemFactory;

    void Start()
    {
        itemFactory.GenerateAllItems();
    }
}

