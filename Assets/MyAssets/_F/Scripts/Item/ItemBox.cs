using UnityEngine;

public class ItemBox : WorldItem
{

    public bool PutInItemBox(ItemInstance item, int amount)
    {
        // アイテムインスタンス取得
        ItemInstance instance = GetItemInstance();
        // アイテムデータをリストに追加
        if (instance.GetFeature<ContainerFeature>().TryStore(item, amount))
        {
            Debug.Log(item.data.itemName + "を入れた");
            return true;
        }
        return false;
    }

    public override void OnInteract(GameObject _player)
    {
        Debug.Log(gameObject.name + ":" + _player.name + "がインタラクト開始");
        // アイテムインスタンス取得
        ItemInstance instance = GetItemInstance();
        // アイテムデータをリストに追加
        ContainerFeature containerFeature = instance.GetFeature<ContainerFeature>();
        UIManager.Instance.OpenContainer(containerFeature.Container);

        // // コンポーネント取得
        // MyCostomPlayer player = _player.GetComponent<MyCostomPlayer>();
        // // プレイヤーの持っているアイテムが受け入れ可能かチェック
        // ItemSlot itemSlot = player.GetSelectedItemSlot();
        // if (PutInItemBox(itemSlot.item, itemSlot.count))
        // {
        //     // 入れたアイテムをプレイヤーから減らす
        //     player.TakeSelectedItem();
        // }
    }
}
