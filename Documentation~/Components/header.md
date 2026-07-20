# Header

[Go Back](../README.md)

## Description

`Header` coordinates a window title and three `RoButton` controls: function,
minimize, and close. It forwards each button click through its own UnityEvent so
window behavior can be assigned without modifying the component.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| `Text` | `string` | Gets or replaces the title label. |

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Text` | `string` | Header title text. |
| `OnFunButtonClick` | `ButtonClickedEvent` | Event forwarded from the function button. |
| `OnMinButtonClick` | `ButtonClickedEvent` | Event forwarded from the minimize button. |
| `OnCloseButtonClick` | `ButtonClickedEvent` | Event forwarded from the close button. |

### Methods

| Method | Description |
| --- | --- |
| `EnsureReferences()` | Returns `false` when a required button or title reference is missing. |

## Examples

```csharp
[SerializeField] private Header header;

private void Awake()
{
    header.Text = "Inventory";
    header.OnCloseButtonClick.AddListener(CloseInventory);
}
```

## Related Components

- [RoButton](ro-button.md)

[Go Back](../README.md)
