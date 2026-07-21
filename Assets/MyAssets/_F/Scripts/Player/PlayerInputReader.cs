using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(ExecutionOrder.PlayerInputReader)]
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputReader : MonoBehaviour
{
    public bool IsPlayerMode => playerInput.currentActionMap.name == "Player";

    public bool IsUIMode => playerInput.currentActionMap.name == "UI";

    private PlayerInput playerInput;
    private InputAction interactAction;
    private InputAction subInteractAction;
    private InputAction pickUpAction;
    private InputAction itemSlotUpAction;
    private InputAction itemSlotDownAction;
    private InputAction cancelAction;
    private InputAction shiftAction;
    public bool InteractPressedThisFrame => interactAction.WasPressedThisFrame();
    public bool InteractPressed => interactAction.IsPressed();
    public bool InteractReleasedThisFrame => interactAction.WasReleasedThisFrame();
    public bool SubInteractPressedThisFrame => subInteractAction.WasPressedThisFrame();
    public bool PickUpPressedThisFrame => pickUpAction.WasPressedThisFrame();
    public bool ItemSlotUpPressedThisFrame => itemSlotUpAction.WasPressedThisFrame();
    public bool ItemSlotDownPressedThisFrame => itemSlotDownAction.WasPressedThisFrame();
    public bool CancelPressedThisFrame => cancelAction.WasPressedThisFrame();
    public bool IsShiftPressed => shiftAction.IsPressed();

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
        subInteractAction = playerInput.actions["SubInteract"];
        pickUpAction = playerInput.actions["PickUpItem"];
        itemSlotUpAction = playerInput.actions["ItemSlot_Up"];
        itemSlotDownAction = playerInput.actions["ItemSlot_Down"];
        cancelAction = playerInput.actions["Cancel"];
        shiftAction = playerInput.actions["Shift"];
    }

    public bool IsCurrentActionMap(string actionMapName)
    {
        return playerInput.currentActionMap.name == actionMapName;
    }

    public void SwitchActionMap(string actionMapName)
    {
        playerInput.SwitchCurrentActionMap(actionMapName);
    }
}
