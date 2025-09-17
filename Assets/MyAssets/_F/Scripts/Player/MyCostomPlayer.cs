using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class MyCostomPlayer : MonoBehaviour
{

    [Header("interact")]
    [Tooltip("インタラクトの距離")]
    public float interactRange = 5.0f;

    private PlayerInput _playerInput;
    private MyGameAssets input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TryGetComponent(out _playerInput);
        input = new MyGameAssets();
        input.Enable();
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
            var camera = GameObject.Find("MainCamera");
            camera.TryGetComponent(out CinemachineBrain brain);
            if (brain != null && brain.ActiveVirtualCamera is CinemachineCamera vcam)
            {
                vcam.Priority = 0;
            }
        }
    }

    void TryInteract()
    {
        if (input.Player.Interact.triggered)
        {
            // カメラからRayを飛ばす
            var _camera = GameObject.Find("MainCamera").transform;
            Ray ray = new Ray(_camera.position, _camera.forward);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.5f);  // デバッグ表示
                                                                        // 初めに当たったオブジェクトのみ取得
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                // インタラクト可能オブジェクトか確認
                hit.collider.TryGetComponent<IInteractable>(out var target);
                if (target != null)
                {
                    // インタラクトする
                    target.ReceiveInteract(gameObject);
                }
            }
        }
    }
}
