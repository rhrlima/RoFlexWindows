# DraggableItem — Integration Spec

[Back to specs index](../README.md)

## Purpose

Drags an `IconAmount` source using a floating proxy while carrying arbitrary payload data. Integrates with `DropZone` / `IDropZone` for inventory-style move, swap, and equip flows.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `DragPayload` | Struct | `Item`, `OriginPosition`, `Data`, `Source`, `Sprite`, `Amount`; includes `TryGetData<T>`. |
| `Configure(object, object source, int amount)` | Method | Sets payload before drag. |
| `CurrentPayload` | `DragPayload` | Active drag payload. |
| `Dragging` / `CanResolveDrop` | `bool` | Drag session state. |
| `TryDrop(IDropZone)` | `bool` | Resolves drop against a zone. |
| `onBeginDrag` / `onDrag` / `onEndDrag` | Events | Pointer drag lifecycle. |
| `onDropAccepted` / `onDropRejected` | `UnityEvent<DraggableItem>` | Drop outcome. |
| `EnsureReferences()` | `bool` | Requires source `IconAmount`, proxy `IconAmount`, and parent `Canvas`. |

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components;
using UnityEngine;

public void BindInventorySlot(DraggableItem item, InventorySlot slot)
{
    item.Configure(slot.ItemId, slot, slot.Amount);
    item.GetComponent<IconAmount>().Assign(slot.Icon, slot.Amount);

    item.onDropAccepted.AddListener(_ => host.RefreshInventory());
    item.onDropRejected.AddListener(_ => host.PlayRejectFeedback());
}
```

When disabled mid-drag, callers should receive a cancellation notification (`onDropRejected` or `CancelDrag()`).

For non-numeric proxy display, `IconAmount` needs a string presentation path (see [IconAmount spec](icon-amount.md)).

## Related Scenarios

- [Inventory grid with drag-and-drop](../../integration-review.md#scenario-inventory-grid)

[Back to specs index](../README.md)
