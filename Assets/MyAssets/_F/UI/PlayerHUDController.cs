using UnityEngine;

// プレイヤーの子オブジェクトに設定する
public class PlayerHUDController : MonoBehaviour
{
    [SerializeField] private GameObject ui_Crosshair;
    [SerializeField] private ItemContainerView itemContainerView;

    private InventoryController inventory;

    void Start()
    {
        SetInventory(transform.parent.GetComponent<InventoryController>());
    }

    public void SetInventory(InventoryController inventory)
    {
        Debug.Log("setContaier");
        if (this.inventory != null)
        {
            this.inventory.OnSelectedItemChanged -= UpdateSelection;

            itemContainerView.OnSlotLeftClicked -= UIManager.Instance.OnSlotLeftClicked;
            itemContainerView.OnSlotRightClicked -= UIManager.Instance.OnSlotRightClicked;
            itemContainerView.OnSlotShiftLeftClicked -= OnShiftLeftClicked;
        }

        this.inventory = inventory;

        // ViewにインベントリのItemContaierを渡す
        itemContainerView.SetContainer(inventory.ItemContainer);

        itemContainerView.OnSlotLeftClicked += UIManager.Instance.OnSlotLeftClicked;
        itemContainerView.OnSlotRightClicked += UIManager.Instance.OnSlotRightClicked;
        itemContainerView.OnSlotShiftLeftClicked += OnShiftLeftClicked;

        // 選択イベント購読
        inventory.OnSelectedItemChanged += UpdateSelection;
        UpdateSelection();

        UIManager.Instance.SetPlayerContainer(inventory.ItemContainer);
    }

    // 選択アイテム更新処理
    public void UpdateSelection()
    {
        for (int i = 0; i < itemContainerView.SlotViews.Count; i++)
        {
            itemContainerView.SlotViews[i].SetSelected(i == inventory.SelectedIndex);
        }
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.OnSelectedItemChanged -= UpdateSelection;

            itemContainerView.OnSlotLeftClicked -= UIManager.Instance.OnSlotLeftClicked;
            itemContainerView.OnSlotRightClicked -= UIManager.Instance.OnSlotRightClicked;
            itemContainerView.OnSlotShiftLeftClicked -= OnShiftLeftClicked;
        }
    }
    
    private void OnShiftLeftClicked(SlotReference slot)
    {
        UIManager.Instance.ShiftFromPlayer(slot);
    }
}
