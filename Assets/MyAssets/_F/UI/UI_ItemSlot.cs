using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text countText;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        icon.sprite = null;
        countText.text = "";
    }

    public void SetSelected(bool sts)
    {
        animator.SetBool("Selected", sts);
    }

    public void SetItem(ItemSlot itemSlot)
    {
        // アイテムを持っていない
        if (itemSlot.IsEmpty)
        {
            // アイコンは非表示
            icon.enabled = false;
            // 個数は非表示
            countText.text = "";
            return;
        }

        // アイコン
        icon.enabled = true;
        icon.sprite = itemSlot.item.icon;
        // 個数
        countText.text = itemSlot.count > 1 ? itemSlot.count.ToString() : "";
    }
}
