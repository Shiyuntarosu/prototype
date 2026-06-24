using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // カメラ
    public GameObject mainCamera { get; private set; }
    // プレイヤー
    public MyCostomPlayer player { get; private set; }

    [SerializeField]
    // 残りのオブジェクトの数
    int ObjectsCount;

    [SerializeField]
    // 回収したオブジェクトの数
    int CollectedItemCount;

    // クリアフラグ
    bool isClear;

    void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // インスタンス設定
        mainCamera = GameObject.Find("MainCamera");
        player = FindFirstObjectByType<MyCostomPlayer>();

        //仮初期化
        ObjectsCount = 3;
        CollectedItemCount = 0;
        isClear = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isClear)
        {

            if (ObjectsCount <= CollectedItemCount)
            {
                isClear = true;
            }
            if (isClear)
            {
                Debug.Log("クリア");
                SceneLoad("TitleScene");
            }
        }

    }

    public void AddItemCount(int count)
    {
        CollectedItemCount += count;
    }

    public void SceneLoad(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
