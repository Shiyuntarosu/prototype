using UnityEngine;

public class HandController : MonoBehaviour
{
    [Header("Item Setting")]
    [Tooltip("アイテム")]
    [SerializeField] private Transform ItemRoot; // アイテムを持つ位置

    [SerializeField] private GameObject itemView; // 手に持っているアイテム

    private InventoryController inventory;
    void Start()
    {
        inventory = GetComponent<InventoryController>();
        inventory.OnSelectedItemChanged += OnItemChanged;
    }

    void OnDestroy()
    {
        inventory.OnSelectedItemChanged -= OnItemChanged;
    }

    private void OnItemChanged()
    {
        SetItemView(inventory.PeekSelectedItemSlot.item);
    }

    // アイテムを手に持つ
    private void SetItemView(ItemInstance item)
    {
        if (itemView != null)
        {
            Destroy(itemView);
        }
        if (item == null)
            return;

        // プレハブ生成
        itemView = Instantiate(item.data.prefab, ItemRoot);
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
        ItemSlot takeItem = inventory.TakeSelectedItem(1);
        if (takeItem.item == null) return;

        ItemInstance instance = takeItem.item;

        // 物理計算用のゲームオブジェクトをアイテムを持つ位置に作成
        GameObject obj = Instantiate(instance.data.prefab, ItemRoot.transform.position, ItemRoot.transform.rotation);
        // 親子関係リセット
        obj.transform.SetParent(null);

        // コンポーネント設定
        obj.TryGetComponent(out WorldItem worldItem);
        if (worldItem == null)
        {
            worldItem = obj.AddComponent<WorldItem>();
        }
        worldItem.Initialize(instance);

        obj.TryGetComponent(out Rigidbody rb);
        if (rb == null)
        {
            rb = obj.AddComponent<Rigidbody>();
        }
        rb.linearVelocity = Vector3.zero;// 速度リセット

        // アイテムを投げる
        Vector3 force = GameManager.Instance.mainCamera.transform.forward;
        int power = GameManager.Instance.player.throwPower;
        rb.AddForce(force * power, ForceMode.Impulse);

        Debug.Log("throw");
    }

}
