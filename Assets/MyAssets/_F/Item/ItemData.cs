using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;

    public int id;

    public int maxStack = 1;

    public GameObject prefab;

    public Sprite icon;

    public Vector3 onHandOffset;   // 手に持った時の位置調整
}
