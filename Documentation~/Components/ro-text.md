# RoText

[Go Back](../README.md)

## Description

`RoText` extends `TextMeshProUGUI` with an optional per-instance outline-color
override. It creates a temporary material instance so changing one label does not
modify the shared font material.

Use the RoText prefab when a label needs its own outline color. Use the
[Text](text.md) prefab for standard styled text.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| `text` | `string` | Inherited displayed text. |
| `OutlineColor` | `Color` | Per-instance outline color requested by `RoText`. |

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `OverrideOutlineColor` | `bool` | Enables or disables the per-instance override. |
| `OutlineColor` | `Color` | Gets or sets the override color. |
| `SupportsOutlineColor` | `bool` | Whether the assigned material exposes `_OutlineColor`. |

### Methods

| Method | Description |
| --- | --- |
| `ApplyOutlineColorOverride()` | Enables and applies the configured outline color. |
| `ClearOutlineColorOverride()` | Disables the override and refreshes rendering. |

## Examples

```csharp
label.text = "Rare Item";
label.OutlineColor = Color.yellow;
label.ApplyOutlineColorOverride();
```

The override requires a TextMesh Pro material whose shader exposes
`_OutlineColor`.

## Related Components

- [Text](text.md)

[Go Back](../README.md)
