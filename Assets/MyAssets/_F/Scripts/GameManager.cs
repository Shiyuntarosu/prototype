using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(ExecutionOrder.GameManager)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject mainCamera { get; private set; } // カメラ
    public MyCustomPlayer player { get; private set; } // プレイヤー

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // インスタンス設定
        mainCamera = GameObject.Find("MainCamera");
        player = FindFirstObjectByType<MyCustomPlayer>();
    }

    public void SceneLoad(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
