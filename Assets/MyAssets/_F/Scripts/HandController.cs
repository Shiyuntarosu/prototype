using UnityEngine;

public class HandController : MonoBehaviour
{
    [Header("Item Setting")]
    [Tooltip("アイテム")]
    [SerializeField] private Transform ItemRoot; // アイテムを持つ位置

    [SerializeField] private GameObject itemView; // 手に持っているアイテム

    private InventoryController inventoryController;
    void Start()
    {
        inventoryController = GetComponent<InventoryController>();
        inventoryController.OnSelectedItemChanged += OnItemChanged;
    }

    void OnDestroy()
    {
        inventoryController.OnSelectedItemChanged -= OnItemChanged;
    }

    private void OnItemChanged(ItemSlot itemSlot)
    {
        SetItemView(itemSlot.item);
    }

    // アイテムを手に持つ
    private void SetItemView(ItemData item)
    {
        if (itemView != null)
        {
            Destroy(itemView);
        }
        if (item == null)
            return;

        // プレハブ生成
        itemView = Instantiate(item.prefab, ItemRoot);
        // 親子関係設定
        itemView.transform.parent = ItemRoot.transform;
        // コンポーネント設定
        itemView.TryGetComponent(out Collider col);
        if (col != null) col.enabled = false;
        itemView.TryGetComponent(out Rigidbody rb);
        if (rb != null) rb.isKinematic = true;
    }

    public void ThrowSelectedItem()
    {
        // 手に持っているアイテムから１つ取り出す
        ItemSlot takeItem = inventoryController.TakeSelectedItem(1);
        if (takeItem.item == null) return;
        // 物理計算用のゲームオブジェクトをアイテムを持つ位置に作成
        GameObject item = Instantiate(takeItem.item.prefab, ItemRoot.transform.position, ItemRoot.transform.rotation);
        // 親子関係リセット
        item.transform.SetParent(null);
        // コンポーネント設定
        item.TryGetComponent(out Rigidbody rb);
        if (rb == null)
        {
            rb = item.AddComponent<Rigidbody>();
        }
        rb.linearVelocity = Vector3.zero;// 速度リセット

        // アイテムを投げる
        Vector3 force = GameManager.Instance.mainCamera.transform.forward;
        int power = GameManager.Instance.player.throwPower;
        rb.AddForce(force * power, ForceMode.Impulse);

        Debug.Log("throw");
    }

}
