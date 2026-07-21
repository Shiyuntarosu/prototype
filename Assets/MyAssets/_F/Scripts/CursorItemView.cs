using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CursorItemView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        UIManager.Instance.CursorSlot.Changed += Refresh;
    }

    private void Start()
    {
        Refresh();
    }

    private void OnDestroy()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.CursorSlot.Changed -= Refresh;
        }
    }

    private void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out Vector2 position);

        rectTransform.anchoredPosition = position;
    }

    private void Refresh()
    {
        ItemSlot slot = UIManager.Instance.CursorSlot.Slot;

        if (slot.IsEmpty)
        {
            icon.enabled = false;
            countText.text = "";
            return;
        }

        icon.enabled = true;
        icon.sprite = slot.item.data.icon;
        countText.text = slot.count > 1 ? slot.count.ToString() : "";
    }
}