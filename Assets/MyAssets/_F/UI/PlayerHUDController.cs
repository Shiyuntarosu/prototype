using UnityEngine;

public class PlayerHUDController : MonoBehaviour
{
    [SerializeField] private GameObject ui_Crosshair;
    [SerializeField] private UI_Inventory ui_Inventory;

    private InventoryController inventoryContorller;

    void Start()
    {
        ui_Inventory.OnInitialize();
        inventoryContorller = transform.parent.GetComponent<InventoryController>();
        inventoryContorller.OnInventoryChanged += RefreshInventoryUI;
        inventoryContorller.OnSelectedItemChanged += OnSelectedItemChanged;
    }

    void OnDestroy()
    {
        inventoryContorller.OnInventoryChanged -= RefreshInventoryUI;
    }

    void OnSelectedItemChanged(ItemSlot item)
    {
        for (int i = 0; i < InventoryController.inventorySize; i++)
        {
            ui_Inventory.Slots[i].SetSelected(false);
            if (i == inventoryContorller.selectedIndex)
                ui_Inventory.Slots[i].SetSelected(true);
        }
    }

    // ＵＩを再読み込みする
    public void RefreshInventoryUI()
    {
        for (int i = 0; i < InventoryController.inventorySize; i++)
        {
            ui_Inventory.Slots[i].SetItem(inventoryContorller.Inventory[i]);
        }
    }
}
