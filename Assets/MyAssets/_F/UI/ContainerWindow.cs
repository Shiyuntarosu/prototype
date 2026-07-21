using UnityEngine;

public class ContainerWindow : UIWindow
{
    [SerializeField] private ItemContainerView containerView;
    [SerializeField] private InventoryController inventoryController;

    private ItemContainer currentContainer;
    public ItemContainer Container => currentContainer;

    public bool IsOpen => currentContainer != null;

    public void Open(ItemContainer container)
    {
        currentContainer = container;

        containerView.SetContainer(container);

        gameObject.SetActive(true);

        containerView.OnSlotLeftClicked += OnSlotLeftClicked;
        containerView.OnSlotRightClicked += OnSlotRightClicked;
        containerView.OnSlotShiftLeftClicked += OnShiftLeftClicked;
    }

    public override void Close()
    {
        containerView.OnSlotLeftClicked -= OnSlotLeftClicked;
        containerView.OnSlotRightClicked -= OnSlotRightClicked;
        containerView.OnSlotShiftLeftClicked -= OnShiftLeftClicked;

        currentContainer = null;

        containerView.SetContainer(null);

        gameObject.SetActive(false);
    }

    private void OnSlotLeftClicked(SlotReference reference)
    {
        UIManager.Instance.OnSlotLeftClicked(reference);
    }

    private void OnSlotRightClicked(SlotReference reference)
    {
        UIManager.Instance.OnSlotRightClicked(reference);
    }

    private void OnShiftLeftClicked(SlotReference slot)
    {
        UIManager.Instance.ShiftFromContainer(slot);
    }
}