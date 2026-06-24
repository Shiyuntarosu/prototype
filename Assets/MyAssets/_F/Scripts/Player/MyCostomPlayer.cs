using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class MyCostomPlayer : MonoBehaviour
{
    public static MyCostomPlayer Instance;

    [Header("Interact Setting")]
    [Tooltip("インタラクトの距離")]
    public float interactRange = 5.0f;
    private float holdTime;
    private GameObject interactTarget;

    private PlayerInput _playerInput;
    private MyGameAssets input;
    private Transform _mainCamera;

    private GameManager _gameManager;

    private InventoryController inventoryController; // インベントリ
    private HandController handController; // ハンド
    private PlayerHUDController playerHUDController; // HUD

    [SerializeField] public int throwPower { get; private set; } = 5; // 投げる強さ

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        TryGetComponent(out _playerInput);
        input = new MyGameAssets();
        input.Enable();
        _mainCamera = GameObject.Find("MainCamera").transform;
        _gameManager = FindFirstObjectByType<GameManager>();
        inventoryController = GetComponent<InventoryController>();
        handController = GetComponent<HandController>();
        playerHUDController = GetComponent<PlayerHUDController>();
    }

    void OnDisable()
    {
        input.Player.Disable();
        input.FixedCamera.Disable();
    }

    void OnDestroy()
    {
        input.Dispose();
    }

    void Update()
    {
        if (_playerInput.currentActionMap.name == "Player")
        {
            TryInteract();
            TryPickUpItem();
            TryThrow();
            TryChangeInventoryIdx();
        }
        if (_playerInput.currentActionMap.name == "FixedCamera")
        {
            ExitCurrentVCamera();
        }
    }

    // プレイヤー操作
    public void SetActionMap_Player()
    {
        _playerInput.SwitchCurrentActionMap("Player");
    }

    // 固定カメラ操作
    public void SetActionMap_FixedCamera()
    {
        _playerInput.SwitchCurrentActionMap("FixedCamera");
    }

    // 操作不可
    public void SetActionMap_NonControl()
    {
        _playerInput.SwitchCurrentActionMap("NonControl");
    }

    void ExitCurrentVCamera()
    {
        if (input.FixedCamera.ExitCamera.triggered)
        {
            SetActionMap_Player();
            _mainCamera.TryGetComponent(out CinemachineBrain brain);
            if (brain != null && brain.ActiveVirtualCamera is CinemachineCamera vcam)
            {
                vcam.Priority = 0;
            }
        }
    }

    // アイテムスロット切り替え
    void TryChangeInventoryIdx()
    {
        if (input.Player.ItemSlot_Down.WasPressedThisFrame())
        {
            inventoryController.ChangeItemSlot(1);
        }
        if (input.Player.ItemSlot_Up.WasPressedThisFrame())
        {
            inventoryController.ChangeItemSlot(-1);
        }
    }

    // アイテムを拾う
    void TryPickUpItem()
    {
        // 最初に押したタイミングの場合
        if (input.Player.PickUpItem.WasPressedThisFrame())
        {
            // カメラからRayを飛ばす
            Ray ray = new Ray(_mainCamera.position, _mainCamera.forward);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.5f);  // デバッグ表示

            // オブジェクトに当たったら（初めに当たったオブジェクトのみ取得）
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                // アイテムか確認
                hit.collider.TryGetComponent(out Item component);
                if (component == null) return;

                // アイテムを取得
                ItemData itemData = component.GetItemData();
                if (inventoryController.PickUpItem(itemData))
                {
                    GameObject.Destroy(hit.collider.gameObject);
                }
            }
        }
    }


    // アイテムを投げる
    void TryThrow()
    {
        // 最初に押したタイミングの場合
        if (input.Player.Throw.WasPressedThisFrame())
        {
            handController.ThrowSelectedItem();
        }
    }

    void CostomInteract(GameObject _target)
    {
        // 段ボールの処理
        _target.TryGetComponent(out ItemBox itembox);
        if (itembox != null)
            InteractItemBox(_target);

        // トラックの処理
        _target.TryGetComponent(out Collector collector);
        if (collector != null)
            InteractCollector(_target);
    }

    void TryInteract()
    {
        // ボタンを押している間
        if (input.Player.Interact.IsPressed())
        {
            // カメラからRayを飛ばす
            Ray ray = new Ray(_mainCamera.position, _mainCamera.forward);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.5f);  // デバッグ表示

            // オブジェクトに当たったら（初めに当たったオブジェクトのみ取得）
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                // インタラクト可能オブジェクトか確認
                hit.collider.TryGetComponent(out IInteractable component);

                // 最初に押したタイミングの場合
                if (input.Player.Interact.WasPressedThisFrame())
                {
                    // コンポーネントがnullなら処理を抜ける
                    if (component == null) return;
                    // オブジェクトをターゲットに設定
                    interactTarget = hit.collider.gameObject;
                    // 個別インタラクト処理
                    CostomInteract(interactTarget);
                }

                // 対象のオブジェクトから視線を外した場合はインタラクトを中断する
                if (interactTarget != null)
                {
                    if (interactTarget != hit.collider.gameObject || component == null)
                    {
                        Debug.Log("インタラクト中断");
                        IInteractable com = interactTarget.GetComponent<IInteractable>();

                        // リリースした瞬間に呼ばれる関数（すべての引数パターンを呼び出しておく）
                        com.OnInteractRelease();
                        com.OnInteractRelease(gameObject);
                        com.OnInteractRelease(holdTime);
                        com.OnInteractRelease(gameObject, holdTime);
                        // ホールド時間をリセット
                        holdTime = 0.0f;
                        // ターゲットをリセット
                        interactTarget = null;
                    }
                }
            }
            else // rayがオブジェクトに当たらなかった場合
            {
                // 対象のオブジェクトから視線を外した場合はインタラクトを中断する
                if (interactTarget != null)
                {
                    Debug.Log("インタラクト中断");
                    IInteractable com = interactTarget.GetComponent<IInteractable>();

                    // リリースした瞬間に呼ばれる関数（すべての引数パターンを呼び出しておく）
                    com.OnInteractRelease();
                    com.OnInteractRelease(gameObject);
                    com.OnInteractRelease(holdTime);
                    com.OnInteractRelease(gameObject, holdTime);
                    // ホールド時間をリセット
                    holdTime = 0.0f;
                    // ターゲットをリセット
                    interactTarget = null;
                }
            }
        }

        if (interactTarget == null) return;

        IInteractable target = interactTarget.GetComponent<IInteractable>();
        // ボタンを押した時
        if (input.Player.Interact.WasPressedThisFrame())
        {
            // ホールド時間をリセット
            holdTime = 0.0f;
            // インタラクトした時に呼ばれる関数（すべての引数パターンを呼び出しておく）
            target.OnInteract();
            target.OnInteract(gameObject);
        }

        // ボタンホールド中
        if (input.Player.Interact.IsPressed())
        {
            // ホールド時間を更新
            holdTime += Time.deltaTime;
            // ホールド中に呼ばれる関数（すべての引数パターンを呼び出しておく）
            target.OnInteractHold();
            target.OnInteractHold(gameObject);
            target.OnInteractHold(holdTime);
            target.OnInteractHold(gameObject, holdTime);
        }

        // ボタンを離した時
        if (input.Player.Interact.WasReleasedThisFrame())
        {
            // リリースした瞬間に呼ばれる関数（すべての引数パターンを呼び出しておく）
            target.OnInteractRelease();
            target.OnInteractRelease(gameObject);
            target.OnInteractRelease(holdTime);
            target.OnInteractRelease(gameObject, holdTime);
            // ホールド時間をリセット
            holdTime = 0.0f;
            // ターゲットをリセット
            interactTarget = null;
        }
    }

    // 段ボールのインタラクト処理
    void InteractItemBox(GameObject obj)
    {
        Debug.Log("処理未完成");
    }

    // トラックの処理
    void InteractCollector(GameObject obj)
    {
        Debug.Log("処理未完成");
    }
}