using UnityEngine;

public partial class MyCustomPlayer : MonoBehaviour
{
    public static MyCustomPlayer Instance;

    [Header("Interact Setting")]
    [Tooltip("インタラクトの距離")]
    public float interactRange = 5.0f;
    private float holdTime;
    private GameObject interactTarget;
    private PlayerInputReader input;
    private Transform mainCamera;
    private InventoryController inventory; // インベントリ
    private HandController handController; // ハンド
    [SerializeField] public int throwPower { get; private set; } = 5; // 投げる強さ

    void Awake()
    {
        Instance = this;
        mainCamera = GameObject.Find("MainCamera").transform;
        input = GetComponent<PlayerInputReader>();
        inventory = GetComponent<InventoryController>();
        handController = GetComponent<HandController>();
    }

    void Update()
    {
        if (input.IsCurrentActionMap("Player"))
        {
            TryInteract();
            TryPickUpItem();
            TryThrow();
            TryChangeInventoryIdx();
        }
        if (input.IsCurrentActionMap("UI"))
        {
            TryCloseWindow();
        }
    }

    private void TryCloseWindow()
    {
        if (input.CancelPressedThisFrame)
        {
            UIManager.Instance.TryCloseCurrentWindow();
        }
    }

    // プレイヤー操作
    public void SetActionMap_Player()
    {
        input.SwitchActionMap("Player");
    }

    // アイテムスロット切り替え
    void TryChangeInventoryIdx()
    {
        if (input.ItemSlotDownPressedThisFrame)
        {
            inventory.ChangeItemSlot(1);
        }
        if (input.ItemSlotUpPressedThisFrame)
        {
            inventory.ChangeItemSlot(-1);
        }
    }

    // アイテムを拾う
    void TryPickUpItem()
    {
        // 最初に押したタイミングの場合
        if (input.PickUpPressedThisFrame)
        {
            // カメラからRayを飛ばす
            Ray ray = new Ray(mainCamera.position, mainCamera.forward);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.5f);  // デバッグ表示

            // オブジェクトに当たったら（初めに当たったオブジェクトのみ取得）
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                // アイテムか確認
                hit.collider.TryGetComponent(out WorldItem component);
                if (component == null) return;

                // アイテムを取得
                ItemInstance item = component.GetItemInstance();
                if (inventory.PickUpItem(item))
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
        if (input.SubInteractPressedThisFrame)
        {
            handController.ThrowSelectedItem();
        }
    }

    void TryInteract()
    {
        // ボタンを押している間
        if (input.InteractPressed)
        {
            // カメラからRayを飛ばす
            Ray ray = new Ray(mainCamera.position, mainCamera.forward);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.5f);  // デバッグ表示

            // オブジェクトに当たったら（初めに当たったオブジェクトのみ取得）
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                // インタラクト可能オブジェクトか確認
                hit.collider.TryGetComponent(out IInteractable component);

                // 最初に押したタイミングの場合
                if (input.InteractPressedThisFrame)
                {
                    // コンポーネントがnullなら処理を抜ける
                    if (component == null) return;
                    // オブジェクトをターゲットに設定
                    interactTarget = hit.collider.gameObject;
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
        if (input.InteractPressedThisFrame)
        {
            // ホールド時間をリセット
            holdTime = 0.0f;
            // インタラクトした時に呼ばれる関数（すべての引数パターンを呼び出しておく）
            target.OnInteract();
            target.OnInteract(gameObject);
        }

        // ボタンホールド中
        if (input.InteractPressed)
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
        if (input.InteractReleasedThisFrame)
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

    // 手に持っているアイテムを取り出す
    public ItemSlot TakeSelectedItem()
    {
        return inventory.TakeSelectedItem();
    }

    // アイテムを追加する
    public bool AddItem(ItemInstance item, int amount)
    {
        return inventory.TryAddItem(item, amount);
    }

    public ItemSlot GetSelectedItemSlot()
    {
        return inventory.PeekSelectedItemSlot;
    }
}