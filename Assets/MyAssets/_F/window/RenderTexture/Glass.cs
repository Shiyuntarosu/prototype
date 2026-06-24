using UnityEngine;
using UnityEngine.Rendering.Universal;


public class Glass : MonoBehaviour
{
    Camera cam;
    public GameObject target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var obj = Instantiate(target);
        // 板の位置と法線（Transformから取得）
        Vector3 planePoint = transform.position;
        Vector3 planeNormal = transform.forward; // 例: 板の表方向

        // 反射行列を作成
        Matrix4x4 reflectionMatrix = ReflectionMatrix(planeNormal, planePoint);

        // オブジェクトのワールド行列を反射
        Matrix4x4 newWorld = reflectionMatrix * obj.transform.localToWorldMatrix;

        // 実際に Transform に反映するなら
        obj.transform.SetPositionAndRotation(
            newWorld.GetColumn(3),                 // 位置
            newWorld.rotation                      // 回転
        );


        Matrix4x4 toOrigin = Matrix4x4.Translate(-planePoint);  // 原点へ移動
        Matrix4x4 back = Matrix4x4.Translate(planePoint);   // 元に戻す

        Matrix4x4 viewMatrix = cam.worldToCameraMatrix; // v
        Matrix4x4 projMatrix = cam.projectionMatrix; // p
        Matrix4x4 modelMatrix = target.transform.localToWorldMatrix; // m

        viewMatrix = viewMatrix * reflectionMatrix * toOrigin * back;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static Matrix4x4 ReflectionMatrix(Vector3 planeNormal, Vector3 planePoint)
    {
        planeNormal.Normalize();

        float nx = planeNormal.x;
        float ny = planeNormal.y;
        float nz = planeNormal.z;

        // 基本の反射行列 (原点基準)
        Matrix4x4 reflect = new Matrix4x4();
        reflect.m00 = 1 - 2 * nx * nx;
        reflect.m01 = -2 * nx * ny;
        reflect.m02 = -2 * nx * nz;
        reflect.m03 = 0;

        reflect.m10 = -2 * ny * nx;
        reflect.m11 = 1 - 2 * ny * ny;
        reflect.m12 = -2 * ny * nz;
        reflect.m13 = 0;

        reflect.m20 = -2 * nz * nx;
        reflect.m21 = -2 * nz * ny;
        reflect.m22 = 1 - 2 * nz * nz;
        reflect.m23 = 0;

        reflect.m30 = 0;
        reflect.m31 = 0;
        reflect.m32 = 0;
        reflect.m33 = 1;

        // 平面位置を考慮して「戻す→反射→移動」の順に組み合わせる
        Matrix4x4 toOrigin = Matrix4x4.Translate(-planePoint);
        Matrix4x4 back = Matrix4x4.Translate(planePoint);

        return back * reflect * toOrigin;
    }

}
