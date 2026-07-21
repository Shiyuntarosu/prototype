using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotView : MonoBehaviour, IPointerClickHandler
{
    public Action<int> OnLeftClick;
    public Action<int> OnRightClick;
    public Action<int> OnShiftLeftClick;

    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text countText;

    public ItemSlot Slot { get; private set; }

    private Animator animator;

    public int Index { get; private set; }

    public void SetSelected(bool sts)
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        animator.SetBool("Selected", sts);
    }

    public void SetSlot(ItemSlot newSlot, int index)
    {
        Slot = newSlot;
        Index = index;
        Refresh();
    }

    private void Refresh()
    {
        // アイテムを持っていない
        if (Slot == null || Slot.IsEmpty)
        {
            // アイコンは非表示
            icon.enabled = false;
            // 個数は非表示
            countText.text = "";
            return;
        }

        // アイコン
        icon.enabled = true;
        icon.sprite = Slot.item.data.icon;
        // 個数
        countText.text = Slot.count > 1 ? Slot.count.ToString() : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool shift = UIManager.Instance.IsShiftPressed;

        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                if (shift)
                {
                    OnShiftLeftClick?.Invoke(Index);
                }
                else
                {
                    OnLeftClick?.Invoke(Index);
                }
                break;

            case PointerEventData.InputButton.Right:
                OnRightClick?.Invoke(Index);
                break;
        }
    }
}
