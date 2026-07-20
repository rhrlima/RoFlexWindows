# RoDropdown

[Go Back](../README.md)

## Description

`RoDropdown` is a styled TextMesh Pro dropdown. It subclasses `TMP_Dropdown`
without adding custom behavior, fields, or events.

## Displayed Data

| Data | Source | Description |
| --- | --- | --- |
| Selected option | `value` | Zero-based index of the selected option. |
| Options | `options` | Labels and optional images shown by the dropdown. |

## Public API

`RoDropdown` uses the inherited `TMP_Dropdown` API.

| Member | Type | Description |
| --- | --- | --- |
| `value` | `int` | Selected option index. |
| `options` | `List<OptionData>` | Available options. |
| `onValueChanged` | `DropdownEvent` | Raised with the selected index. |
| `SetValueWithoutNotify(value)` | Method | Changes selection without invoking the event. |

## Examples

```csharp
dropdown.ClearOptions();
dropdown.AddOptions(new List<string> { "All", "Weapons", "Armor" });
dropdown.onValueChanged.AddListener(FilterInventory);
```

[Go Back](../README.md)
