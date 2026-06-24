using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private List<UI_ItemSlot> slots;
    public IReadOnlyList<UI_ItemSlot> Slots { get { return slots; } }

    public void OnInitialize()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.TryGetComponent(out UI_ItemSlot component);
            if (component != null)
            {
                slots.Add(component);
            }
        }
    }
}
