using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainerView : MonoBehaviour
{
    public event Action<SlotReference> OnSlotLeftClicked;
    public event Action<SlotReference> OnSlotRightClicked;
    public event Action<SlotReference> OnSlotShiftLeftClicked;

    [SerializeField] private ItemSlotView slotPrefab;
    [SerializeField] private Transform slotRoot;
    private readonly List<ItemSlotView> slotViews = new();
    public IReadOnlyList<ItemSlotView> SlotViews => slotViews;
    private ItemContainer container;

    public void SetContainer(ItemContainer newContainer)
    {
        if (container != null)
            container.OnChanged -= Refresh;

        container = newContainer;
        ClearSlots();

        if (container == null) return;

        container.OnChanged += Refresh;
        CreateSlots();
        Refresh();
    }

    private void Refresh()
    {
        if (container == null) return;

        for (int i = 0; i < slotViews.Count; i++)
        {
            slotViews[i].SetSlot(container.Slots[i], i);
        }
    }
    private void CreateSlots()
    {
        for (int i = 0; i < container.Slots.Count; i++)
        {
            ItemSlotView view = Instantiate(slotPrefab, slotRoot);

            int index = i;
            view.OnLeftClick += _ =>
            {
                OnSlotLeftClicked?.Invoke(new SlotReference(container, index));
            };
            view.OnRightClick += _ =>
            {
                OnSlotRightClicked?.Invoke(new SlotReference(container, index));
            };
            view.OnShiftLeftClick += _ =>
            {
                OnSlotShiftLeftClicked?.Invoke(new SlotReference(container, index));
            };

            slotViews.Add(view);
        }
    }

    private void ClearSlots()
    {
        foreach (var view in slotViews)
        {
            Destroy(view.gameObject);
        }

        slotViews.Clear();
    }

    private void OnDestroy()
    {
        if (container != null)
            container.OnChanged -= Refresh;
    }
}
