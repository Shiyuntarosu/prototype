using Unity.Cinemachine;
using UnityEngine;

public class GameEventObject : MonoBehaviour, IInteractable
{
    // 固定カメラ
    [SerializeField] private CinemachineCamera virtualCamera;
    // 個別処理用
    [SerializeField] private CostomGameEvent costom;

    private enum eGameEventState
    {
        Idle,       // 待機
        Running,    // 実行中
        OnComplete, // 完了時
        Finished,   // 終了
    }

    // 状態
    private eGameEventState state;
    [SerializeField] private bool isDone;

    [Header("Options")]
    [SerializeField] private bool Interactable;

    private void Start()
    {
        Initialize();
    }
    private void Update()
    {
        RunningUpdate();
        OnComplete();
    }

    // 初期化処理
    private void Initialize()
    {
        Debug.Log("初期化");
        // 個別処理取得
        TryGetComponent(out costom);
        // 待機状態へ
        state = eGameEventState.Idle;
        // 固定カメラを無効化
        virtualCamera.Priority = 0;
        // カスタム処理呼び出し
        if (costom != null)
        {
            costom.Initialize();
        }
    }

    // インタラクト時
    public void OnInteract(GameObject _player)
    {
        // インタラクトオプションが無効なら処理を飛ばす
        if (!Interactable)
            return;

        // 待機中以外は処理を飛ばす
        if (state != eGameEventState.Idle)
            return;

        // 実行状態へ
        state = eGameEventState.Running;
        // 固定カメラを有効化
        virtualCamera.Priority = 100;
        // プレイヤーの入力マップを変更

        // カスタム処理呼び出し
        if (costom != null)
        {
            costom.OnInteract();
        }
    }

    // 実行時の更新処理
    private void RunningUpdate()
    {
        // 実行状態以外の場合は処理を飛ばす
        if (state != eGameEventState.Running)
            return;

        Debug.Log("実行中");
        // カスタム処理呼び出し
        if (costom != null)
        {
            costom.RunningUpdate();
            isDone = costom.isDone;
        }
        if (isDone)
        {
            state = eGameEventState.OnComplete;
        }
    }

    // 完了時の処理
    private void OnComplete()
    {
        // 完了状態以外は処理を飛ばす
        if (state != eGameEventState.OnComplete)
            return;

        Debug.Log("完了");
        // 終了状態へ
        state = eGameEventState.Finished;
        // 固定カメラを無効化
        virtualCamera.Priority = 0;
        // カスタム処理呼び出し
        if (costom != null)
        {
            costom.OnComplete();
        }
    }
}
