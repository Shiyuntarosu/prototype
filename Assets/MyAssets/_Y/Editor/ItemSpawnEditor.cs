using UnityEditor;
using System.Linq;

#if UNITY_EDITOR
[CustomEditor(typeof(ItemSpawnPoint))]
public class ItemSpawnPointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var spawnPoint = (ItemSpawnPoint)target;
        var database = AssetDatabase.LoadAssetAtPath<ItemDatabase>("Assets/MyAssets/_Y/ScriptableObjects/ItemDatabase.asset");

        if (database != null)
        {
            var itemNames = database.Items.Select(i => i.itemName).ToArray();
            var selectedIndex = EditorGUILayout.Popup("Item", spawnPoint.ItemId, itemNames);
            spawnPoint.SetItemId(selectedIndex);
        }
        else
        {
            EditorGUILayout.HelpBox("ItemDatabase not found.", MessageType.Warning);
        }

        DrawDefaultInspector();
    }
}
#endif

