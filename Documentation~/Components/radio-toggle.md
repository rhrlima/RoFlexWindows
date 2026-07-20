# Radio Toggle

[Go Back](../README.md)

## Description

The Radio Toggle prefab presents `RoToggle` with radio-button styling. Assign
multiple instances to the same Unity `ToggleGroup` when only one option may be
selected.

## Displayed Data

| Data | Source | Description |
| --- | --- | --- |
| Selected state | `isOn` | Whether this option is selected. |
| Label | Child text | Optional option label supplied by the prefab. |

## Public API

The prefab uses the inherited Unity `Toggle` API.

| Member | Type | Description |
| --- | --- | --- |
| `isOn` | `bool` | Current selected state. |
| `group` | `ToggleGroup` | Group that enforces radio-button exclusivity. |
| `onValueChanged` | `ToggleEvent` | Invoked when the state changes. |
| `SetIsOnWithoutNotify(value)` | Method | Changes state without invoking the event. |

## Examples

```csharp
radioToggle.group = classGroup;
radioToggle.onValueChanged.AddListener(SelectClass);
```

## Related Components

- [RoToggle](ro-toggle.md)
- [ToggleSwitch](toggle-switch.md)

[Go Back](../README.md)
