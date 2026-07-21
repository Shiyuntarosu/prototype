using UnityEngine;

public class ItemTransferService
{
    public void LeftClick(CursorSlot cursorSlot, SlotReference target)
    {
        // Cursorが空なら拾う
        if (cursorSlot.IsEmpty)
        {
            PickUp(cursorSlot, target.Slot, target);
            return;
        }

        // 空スロットなら置く
        if (target.Slot.IsEmpty)
        {
            Place(cursorSlot, target);
            return;
        }

        // 同じアイテムならスタック
        if (TryStack(cursorSlot, target.Slot, target))
        {
            return;
        }

        // 違うアイテムなら交換
        Swap(cursorSlot, target.Slot, target);
    }

    public void RightClick(CursorSlot cursorSlot, SlotReference target)
    {
        ItemSlot targetSlot = target.Slot;

        // Cursorが空なら半分持つ
        if (cursorSlot.IsEmpty)
        {
            Split(cursorSlot, target);
            return;
        }

        // 空スロットなら1個置く
        if (targetSlot.IsEmpty)
        {
            PlaceOne(cursorSlot, target);
            return;
        }

        // 同じアイテムなら1個だけスタック
        TryStackOne(cursorSlot, targetSlot, target);
    }

    public void ShiftClick(SlotReference source, ItemContainer destination)
    {
        ItemSlot sourceSlot = source.Slot;

        if (sourceSlot.IsEmpty)
            return;

        int moved = destination.AddItem(sourceSlot);

        if (moved <= 0)
            return;

        source.Container.TryTakeItem(source.Index, moved);
    }

    private void PickUp(CursorSlot cursorSlot, ItemSlot targetSlot, SlotReference target)
    {
        if (targetSlot.IsEmpty)
            return;

        cursorSlot.CopyFrom(targetSlot);

        target.Container.TryTakeItem(target.Index, targetSlot.count);
    }

    private void Place(CursorSlot cursorSlot, SlotReference target)
    {
        if (!target.Container.TryPlaceItem(target.Index, cursorSlot.Slot))
            return;

        cursorSlot.Clear();
    }
    private bool TryStack(CursorSlot cursorSlot, ItemSlot targetSlot, SlotReference target)
    {
        if (!targetSlot.CanStackWith(cursorSlot.Slot))
            return false;

        int add = Mathf.Min(cursorSlot.Slot.count, targetSlot.RemainingStack);

        if (add <= 0)
            return false;

        targetSlot.count += add;
        cursorSlot.Remove(add);

        target.Container.NotifyChanged();

        return true;
    }

    private void Swap(CursorSlot cursorSlot, ItemSlot targetSlot, SlotReference target)
    {
        cursorSlot.Swap(targetSlot);

        target.Container.NotifyChanged();
    }

    private void Split(CursorSlot cursorSlot, SlotReference target)
    {
        ItemSlot targetSlot = target.Slot;

        if (targetSlot.IsEmpty)
            return;

        int take = (targetSlot.count + 1) / 2;

        ItemSlot taken = target.Container.TryTakeItem(target.Index, take);

        cursorSlot.CopyFrom(taken);
    }

    private void PlaceOne(CursorSlot cursorSlot, SlotReference target)
    {
        ItemSlot oneItem = new ItemSlot();

        oneItem.Set(cursorSlot.Slot.item, 1);

        if (!target.Container.TryPlaceItem(target.Index, oneItem))
            return;

        cursorSlot.Remove(1);
    }

    private bool TryStackOne(CursorSlot cursorSlot, ItemSlot targetSlot, SlotReference target)
    {
        if (!cursorSlot.Slot.CanStackWith(targetSlot))
            return false;

        if (targetSlot.RemainingStack <= 0)
            return false;

        targetSlot.count++;
        cursorSlot.Remove(1);

        target.Container.NotifyChanged();

        return true;
    }
}