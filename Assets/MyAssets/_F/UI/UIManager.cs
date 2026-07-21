using System;
using UnityEngine;

[DefaultExecutionOrder(ExecutionOrder.UIManager)]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private PlayerInputReader inputReader;
    public bool IsShiftPressed => inputReader.IsShiftPressed;
    [SerializeField] ContainerWindow containerWindow;
    private ItemTransferService transferService = new ItemTransferService();
    private readonly CursorSlot cursorSlot = new CursorSlot();
    public CursorSlot CursorSlot => cursorSlot;
    public bool HasCursorItem => !cursorSlot.IsEmpty;
    private ContainerWindow currentWindow;
    public bool IsUIOpen => currentWindow != null;
    public ItemContainer OpenedContainer => currentWindow != null ? currentWindow.Container : null;
    private ItemContainer playerContainer;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void OpenContainer(ItemContainer container)
    {
        if (IsUIOpen) return;

        inputReader.SwitchActionMap("UI");

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        currentWindow = containerWindow;
        currentWindow.Open(container);
    }

    public void CloseCurrentWindow()
    {
        if (!IsUIOpen) return;

        inputReader.SwitchActionMap("Player");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentWindow.Close();
        currentWindow = null;
    }

    public bool TryCloseCurrentWindow()
    {
        if (!IsUIOpen)
            return false;

        CloseCurrentWindow();
        return true;
    }

    public void OnSlotLeftClicked(SlotReference slot)
    {
        transferService.LeftClick(cursorSlot, slot);
    }

    public void OnSlotRightClicked(SlotReference slot)
    {
        transferService.RightClick(cursorSlot, slot);
    }

    public void PickCursorItem(ItemSlot slot)
    {
        cursorSlot.Set(slot.item, slot.count);
        NotifyCursorSlotChanged();
    }

    public void ClearCursorItem()
    {
        cursorSlot.Clear();
        NotifyCursorSlotChanged();
    }

    public void SetPlayerContainer(ItemContainer container)
    {
        playerContainer = container;
    }

    public void ShiftFromPlayer(SlotReference slot)
    {
        if (OpenedContainer == null)
            return;

        transferService.ShiftClick(slot, OpenedContainer);
    }

    public void ShiftFromContainer(SlotReference slot)
    {
        if (playerContainer == null)
            return;

        transferService.ShiftClick(slot, playerContainer);
    }

    private void NotifyCursorSlotChanged()
    {
        Debug.Log("cursor item");
        // OnCursorSlotChanged?.Invoke();
    }
}
