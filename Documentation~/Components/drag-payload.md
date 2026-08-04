# DragPayload

[Go Back](../README.md)

## Description

`DragPayload` is the value transferred during a proxy drag-and-drop session. It
preserves the source that created it, project-defined data, and the sprite and
text used by the visual proxy.

## Public API

### Fields

| Field | Type | Description |
| --- | --- | --- |
| `source` | `IDragSource` | Source that created the payload. |
| `data` | `object` | Project-defined value being transferred. |
| `sprite` | `Sprite` | Sprite displayed by the proxy and exposed to the target. |
| `text` | `string` | Text displayed by the proxy and exposed to the target. A null constructor value becomes an empty string. |

All fields are `readonly`.

### Constructor

| Constructor | Description |
| --- | --- |
| `DragPayload(IDragSource source, Sprite sprite, string text, object data)` | Creates a payload with its source, presentation, and project value. |

### Methods

| Method | Description |
| --- | --- |
| `GetData<T>()` | Returns `data` as `T`, or the default value when it is incompatible. |
| `TryGetData<T>(out T data)` | Returns whether `data` is compatible with `T` and supplies the typed value. |

The manager rejects a null payload returned by `IDragSource.CreatePayload()`.
Payload fields themselves are not validated by the constructor.

## Related Pages

- [Drag and Drop](drag-and-drop.md)
- [`IDragSource`](idrag-source.md)
- [`IDragTarget`](idrag-target.md)
- [`DraggableManager`](draggable-manager.md)

[Go Back](../README.md)
