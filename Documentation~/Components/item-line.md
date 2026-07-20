# ItemLine

[Go Back](../README.md)

## Description

`ItemLine` displays an item label followed by a configured list of socket
entries. Each socket contains an `IconAmount` slot and an `open` flag; closed
sockets are hidden during initialization.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| `Text` | `string` | Item label. |
| `numSockets` | `int` | Number of configured socket entries. |
| Socket | `IconAmount` | Visual assigned to a socket entry. |

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Text` | `string` | Gets or replaces the item label. |
| `numSockets` | `int` | Returns the configured socket count. |

### Methods

| Method | Description |
| --- | --- |
| `EnsureReferences()` | Validates the label and every configured socket slot. |

## Examples

```csharp
itemLine.Text = item.DisplayName;
Debug.Log($"Configured sockets: {itemLine.numSockets}");
```

## Related Components

- [IconAmount](icon-amount.md)

[Go Back](../README.md)
