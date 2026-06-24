using UnityEngine;

public class TheBoy : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Vector3 firstPosition;
    [SerializeField] private Vector3 secondPosition;

    void Start()
    {
        var camera = GameObject.Find("MainCamera");
        camera.TryGetComponent(out mainCamera);
    }

    void Update()
    {
        if (gameObject == null || mainCamera == null) return;

        Vector3 objToCam = (mainCamera.transform.position - transform.position).normalized;
        Vector3 camForward = mainCamera.transform.forward;

        // 角度を計算
        float angle = Vector3.Angle(objToCam, camForward);

        // 一度目の出現の場合
        if (transform.position == firstPosition)
        {
            // 一定角度横向いたら
            if (angle >= 110.0f)
            {
                // Renderer を無効化
                GetComponent<Renderer>().enabled = false;
                // 座標を移動
                transform.position = secondPosition;
            }
        }
        // 二度目の出現の場合
        if (transform.position == secondPosition)
        {
            // 一定角度横向いたら
            if (angle <= 70.0f)
            {
                // 再度描画
                GetComponent<Renderer>().enabled = true;
            }
        }
    }

    public void SpawnTheBoyFirstTime()
    {
        Instantiate(gameObject, firstPosition, new Quaternion());
    }
}
