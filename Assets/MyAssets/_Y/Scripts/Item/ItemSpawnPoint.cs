using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
    /// <summary>
    /// ItemDatabaseを元に生成したいアイテムIDを設定する
    /// </summary>
    private int itemId = 0;

    public int ItemId => itemId;

    public void SetItemId(int id)
    {
        itemId = id;
    }

}
