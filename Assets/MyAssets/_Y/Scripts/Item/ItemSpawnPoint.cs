using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
    /// <summary>
    /// ItemDatabaseを元に生成したいアイテムIDを設定する
    /// </summary>
    [HideInInspector]
    [SerializeField]
    private int itemId;

    public int ItemId => itemId;

    public void SetItemId(int id)
    {
        itemId = id;
    }

}
