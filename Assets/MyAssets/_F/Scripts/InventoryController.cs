using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public event Action OnInventoryChanged;     // インベントリ変更検知
    public event Action<ItemSlot> OnSelectedItemChanged; // 選択中アイテム変更検知

    [SerializeField] private List<ItemSlot> inventory; // インベントリ
    public IReadOnlyList<ItemSlot> Inventory { get { return inventory; } }  // 外部参照用（読み取り専用）
    [SerializeField] private GameObject ui_inventory; // インベントリのＵＩ
    public const int inventorySize = 5; // インベントリの大きさ
    public int selectedIndex { get; private set; }

    [SerializeField]
    private ItemSlot selectedItemSlot    // 選択中のスロット
    {
        get
        {
            return inventory[selectedIndex];
        }
    }

    void Start()
    {
        OnInitialize();
    }

    // インベントリスロットをすべて空にする
    public void OnInitialize()
    {
        inventory.Clear();
        for (int i = 0; i < inventorySize; i++)
        {
            inventory.Add(new ItemSlot(null, 0));
        }
        OnInventoryChanged?.Invoke();
        OnSelectedItemChanged?.Invoke(selectedItemSlot);
    }

    // アイテムスロット切り替え
    public void ChangeItemSlot(int value)
    {
        selectedIndex += value;
        if (selectedIndex < 0)
        {
            selectedIndex = inventorySize - 1;
        }
        if (selectedIndex >= inventorySize)
        {
            selectedIndex = 0;
        }
        Debug.Log(selectedIndex);

        OnSelectedItemChanged?.Invoke(selectedItemSlot);    // 選択中のアイテムが更新された
    }


    // インベントリにアイテムを追加する
    public bool AddItem(ItemData item, int amount)
    {
        // 既存スタックがあれば足す
        foreach (ItemSlot slot in inventory)
        {
            if (slot.item == item && slot.count < item.maxStack)
            {
                // 拾った数か1スタックまでの数
                int add = Mathf.Min(amount, item.maxStack - slot.count);

                slot.count += add;
                amount -= add;

                if (amount <= 0)
                {
                    OnInventoryChanged?.Invoke(); // インベントリが更新された
                    OnSelectedItemChanged?.Invoke(selectedItemSlot);    // 選択中のアイテムが更新された
                    return true;
                }
            }
        }

        // 空スロットを探す
        foreach (ItemSlot slot in inventory)
        {
            if (slot.IsEmpty)
            {
                slot.item = item;

                // 拾った数か最大スタック数までの数
                int add = Mathf.Min(amount, item.maxStack);

                slot.count = add;
                amount -= add;

                if (amount <= 0)
                {
                    OnInventoryChanged?.Invoke(); // インベントリが更新された
                    OnSelectedItemChanged?.Invoke(selectedItemSlot);    // 選択中のアイテムが更新された
                    return true;
                }
            }
        }

        Debug.Log("アイテムいっぱい");
        return false;
    }

    // インベントリからアイテムを取り出す
    public ItemSlot TakeSelectedItem(int amount = 1)
    {
        // 取り出す個数
        int take = Mathf.Min(amount, selectedItemSlot.count);

        // 取り出すアイテム
        ItemSlot result = new ItemSlot(selectedItemSlot.item, take);

        // 取りだした分を減らす
        selectedItemSlot.count -= take;
        if (selectedItemSlot.count <= 0)
        {
            selectedItemSlot.item = null;
        }

        OnInventoryChanged?.Invoke(); // インベントリが更新された
        OnSelectedItemChanged?.Invoke(selectedItemSlot);    // 選択中のアイテムが更新された

        return result;
    }

    // アイテムを拾う
    public bool PickUpItem(ItemData item)
    {
        // インベントリにアイテムを追加
        if (!AddItem(item, 1))
        {
            Debug.Log("アイテムいっぱい");
            return false;
        }
        Debug.Log(item + "を拾った");
        return true;
    }
}