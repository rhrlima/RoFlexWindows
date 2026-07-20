# DropZone

[Go Back](../README.md)

## Description

`DropZone` receives Unity UI drop events and resolves the active payload through
the dragged object's `IDraggable` implementation. Override it to decide which
payloads are valid, update the destination, and choose whether the source is moved
or restored.

`DropZone<TData>` is the typed specialization for project data. It rejects null or
incompatible payload data before invoking its typed methods.

## Displayed Data

`DropZone` displays no data itself. A subclass applies `DragPayload.Presentation`
to a destination visual or maps `DragPayload.Data` into project-specific UI.

## Public API

### Methods

| Method | Description |
| --- | --- |
| `CanDrop(DragPayload payload)` | Returns whether the zone permits the payload. The base implementation returns true. |
| `Drop(DragPayload payload)` | Updates the target and returns the result. The base implementation returns `Move`. |
| `OnDrop(PointerEventData eventData)` | Finds an unresolved `IDraggable`, requests resolution, and raises the matching zone event. |

`DropZone<TData>` exposes typed overloads:

| Method | Description |
| --- | --- |
| `CanDrop(TData data, DragPayload payload)` | Applies project validation after the type check. |
| `Drop(TData data, DragPayload payload)` | Updates the destination using typed data. |

### Events

| Event | Description |
| --- | --- |
| `onDropAccepted` | Raised with the `MonoBehaviour` implementing `IDraggable` after acceptance. |
| `onDropRejected` | Raised with that component after rejection. |

## Drop Results

| Result | Accepted | Source behavior |
| --- | --- | --- |
| `DropResult.Rejected` | No | Restore the source. |
| `DropResult.Move` | Yes | Clear the source data and visual. |
| `DropResult.Copy` | Yes | Restore the source unchanged. |
| `DropResult.Swap` | Yes | Restore the source after the zone replaces its data and visual. |

`Swap` has the same source disposition as `Copy`. A swapping zone must update
`payload.SourceVisual` and call `payload.Draggable.Configure` with the displaced
value before returning.

## Examples

### Typed Item Drop Zone

```csharp
public sealed class ItemDropZone : DropZone<Item>
{
    [SerializeField] private IconText destination;

    protected override bool CanDrop(Item item, DragPayload payload)
    {
        return destination != null && destination.Empty;
    }

    protected override DropResult Drop(Item item, DragPayload payload)
    {
        if (!destination.TryApplyPresentation(payload.Presentation))
            return DropResult.Rejected;

        destination.SetActive(true);
        destination.GetComponent<DraggableItem>()
            ?.Configure(item, this, payload.Presentation);
        return DropResult.Move;
    }
}
```

### Skill Drop Zone

```csharp
public sealed class SkillDropZone : DropZone<Skill>
{
    [SerializeField] private IconText destination;

    protected override bool CanDrop(Skill skill, DragPayload payload)
    {
        return destination != null;
    }

    protected override DropResult Drop(Skill skill, DragPayload payload)
    {
        if (!destination.TryApplyPresentation(payload.Presentation))
            return DropResult.Rejected;

        destination.SetActive(true);
        destination.GetComponent<DraggableItem>()
            ?.Configure(skill, this, payload.Presentation);
        return DropResult.Move;
    }
}
```

The zone owns occupancy checks and domain persistence. Returning `Move` clears the
source visual but does not update an inventory or skill model automatically.

## Related Components

- [DraggableItem](draggable-item.md)
- [IconAmount](icon-amount.md)
- [IconText](icon-text.md)

[Go Back](../README.md)
