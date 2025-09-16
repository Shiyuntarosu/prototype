using UnityEngine;

/// <summary>
/// どのように生成するかを管理する
/// </summary>
public class ItemFactory : MonoBehaviour
{
    public static ItemFactory Instance { get; private set; }

    [SerializeField]
    private ItemDatabase itemDatabase;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // 必要なら
    }

    /// <summary>
    /// ItemDatabaseを元に一括でアイテムを生成する 
    /// </summary>
    public void GenerateAllItems()
    {
        /// MEMO:FindObjectsOfType<T>() がUnity2023から非推奨になった(後継：FindObjectsByType<T>())
        /// ソートやフィルタリングができるようになったらしい
        /// itemIdの設定ミス注意！！
        /// もし下記コードで取得したくない場合はItemSpawnPointがアタッチされたゲームオブジェクトを非アクティブ化
        var spawnPoints = FindObjectsByType<ItemSpawnPoint>(FindObjectsSortMode.None);

        foreach (var point in spawnPoints)
        {
            var itemData = itemDatabase.GetItemById(point.ItemId);
            if (itemData == null || itemData.prefab == null) continue;

            var pointTransform = point.transform;
            Instantiate(itemData.prefab, pointTransform.position, pointTransform.rotation);
        }
    }
}
