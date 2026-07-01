# DropZone — Integration Spec

[Back to specs index](../README.md)

## Purpose

Accepts drops from `DraggableItem` instances. Subclass to validate and apply payload data (move item, equip gear, merge stacks).

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `CanDrop(DragPayload)` | `virtual bool` | Default returns `true`. |
| `Drop(DragPayload)` | `virtual bool` | Default returns `true`. |
| `onDropAccepted` / `onDropRejected` | `UnityEvent<DraggableItem>` | Outcome events (pass item, not payload). |
| `OnDrop(PointerEventData)` | Method | Unity event-system entry point. |

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components;

public class EquipDropZone : DropZone
{
    public override bool CanDrop(DragPayload payload) =>
        payload.TryGetData<ItemId>(out var id) && CanEquip(id);

    public override bool Drop(DragPayload payload)
    {
        host.Equip(payload);
        return true;
    }
}

// Subscribe with payload context — desired API:
// dropZone.OnDropAccepted.AddListener(payload => host.OnEquipped(payload));
```

Default `CanDrop` / `Drop` should be conservative (`false`) or documented so integrators do not forget overrides.

## Related Scenarios

- [Inventory grid with drag-and-drop](../../integration-review.md#scenario-inventory-grid)

[Back to specs index](../README.md)
