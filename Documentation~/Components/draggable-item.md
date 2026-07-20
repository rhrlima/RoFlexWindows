# DraggableItem

[Go Back](../README.md)

## Description

`DraggableItem` moves project-specific data from a source visual through a drag
proxy. It implements `IDraggable` and Unity's begin-drag, drag, and end-drag
handlers. A separate [DropZone](drop-zone.md) validates and resolves the drop.

The component does not define an inventory item type. It stores application data as
`object` and captures a `DragPayload` when dragging begins.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| `Data` | `object` | Project value being dragged, such as an item or skill. |
| `Context` | `object` | Optional source slot, inventory, owner, or routing context. |
| `SourceVisual` | `IDragVisual` | Component displaying the value at its origin. |
| `Presentation` | `DragPresentation` | Sprite, amount, and text copied to the drag proxy. |

`IconAmount` and `IconText` are the provided `IDragVisual` implementations.
`IconAmount` renders sprite and amount; `IconText` also renders text.

## Configuration

| Field | Description |
| --- | --- |
| **Proxy Visual Component** | Required `MonoBehaviour` implementing `IDragVisual`. |
| **Source Visual Component** | Optional explicit source. When empty, the first `IDragVisual` on the same GameObject is used. |

The source and proxy need `RectTransform` components, the proxy needs a
`RectTransform` parent, and the draggable must be below a `Canvas`.
`EnsureReferences` adds a non-interactable, non-raycasting `CanvasGroup` to the
proxy when needed.

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Dragging` | `bool` | Whether a drag is active. |
| `CanResolveDrop` | `bool` | Whether the active drag has not yet been resolved. |
| `CurrentPayload` | `DragPayload` | Snapshot captured when the latest drag began. |

### Methods

| Method | Description |
| --- | --- |
| `Configure(data, context, presentation)` | Stores domain data, optional source context, and optional presentation for the next drag. |
| `EnsureReferences()` | Resolves and validates the source, proxy, canvas, and transforms. |
| `TryDrop(dropZone)` | Resolves the current drag through one `IDropZone`. |

`Configure` stores values only. Application code must populate and activate the
source visual separately.

### Events

| Event | Description |
| --- | --- |
| `onBeginDrag` | Raised after the source is hidden and the proxy is shown. |
| `onDrag` | Raised after the proxy position is updated. |
| `onEndDrag` | Raised when Unity ends an active drag. |
| `onDropAccepted` | Raised when a drop returns an accepted result. |
| `onDropRejected` | Raised after a rejected or unresolved drop. |

## Defining a Payload

Configure the component after the application value is known:

```csharp
var presentation = new DragPresentation(
    item.sprite,
    item.amount.ToString(),
    item.name);

sourceVisual.TryApplyPresentation(presentation);
sourceVisual.SetActive(true);
sourceDraggable.Configure(item, sourceSlot, presentation);
```

When dragging begins, the component copies those values, its source position, and
itself into `CurrentPayload`. If presentation is omitted, it captures the fields
currently supported by the source visual.

Use the typed helpers when reading payload values:

```csharp
if (payload.TryGetData<Item>(out var item)) { }
if (payload.TryGetContext<InventorySlot>(out var slot)) { }
if (payload.TryGetSourceVisual<IconText>(out var visual)) { }
```

## Drag Behavior

1. A left-button drag validates the source and proxy.
2. The presentation is applied to the proxy.
3. The source is hidden and the proxy follows the pointer.
4. A resolved move clears the source; copy restores it.
5. An unresolved pointer release restores the source as a rejected drop.

`CurrentPayload` remains available after the drag, so consumers should check
`CanResolveDrop` before attempting resolution.

## Other Data Types

The component can carry skills, spells, equipment, commands, or other types without
changes. Configure the domain object as `Data` and use a typed
`DropZone<Skill>` at the destination:

```csharp
var presentation = new DragPresentation(
    skill.Icon,
    skill.Rank.ToString(),
    skill.Name);

skillVisual.TryApplyPresentation(presentation);
skillVisual.SetActive(true);
skillDraggable.Configure(skill, sourceSkillSlot, presentation);
```

The standard presentation is limited to sprite, amount, and text. A custom
`IDragVisual` can change rendering, but transferring additional presentation
fields also requires extending `DragPresentation`.

## Related Components

- [DropZone](drop-zone.md)
- [IconAmount](icon-amount.md)
- [IconText](icon-text.md)
- [Draggable](draggable.md)

[Go Back](../README.md)
