using UnityEngine;


[RequireComponent(typeof(MyGameInputs))]
public class MyCostomPlayer : MonoBehaviour
{

    [Header("interact")]
    [Tooltip("インタラクトの距離")]
    public float interactRange = 5.0f;

    private MyGameInputs _input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TryGetComponent(out _input);
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.interact)
        {
            TryInteract();
            _input.interact = false;
        }
    }


    void TryInteract()
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
