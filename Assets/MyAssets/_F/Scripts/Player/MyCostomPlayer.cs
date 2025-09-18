using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyCostomPlayer : MonoBehaviour
{

    [Header("Interact Setting")]
    [Tooltip("インタラクトの距離")]
    public float interactRange = 5.0f;
    private float holdTime;
    private GameObject interactTarget;

    private PlayerInput _playerInput;
    private MyGameAssets input;
    private Transform _mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TryGetComponent(out _playerInput);
        input = new MyGameAssets();
        input.Enable();
        _mainCamera = GameObject.Find("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInput.currentActionMap.name == "Player")
        {
            TryInteract();
        }
        if (_playerInput.currentActionMap.name == "FixedCamera")
        {
            ExitCurrentVCamera();
        }
    }
    public void SetActionMap_Player()
    {
        _playerInput.SwitchCurrentActionMap("Player");
    }

    public void SetActionMap_FixedCamera()
    {
        _playerInput.SwitchCurrentActionMap("FixedCamera");
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
}