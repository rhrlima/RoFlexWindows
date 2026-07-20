# RoToggle

[Go Back](../README.md)

## Description

`RoToggle` is the thin `UnityEngine.UI.Toggle` subclass used by the Toggle
prefab. It adds no custom behavior, fields, or events.

## Displayed Data

| Data | Source | Description |
| --- | --- | --- |
| Selected state | `isOn` | Whether the toggle is selected. |
| Label | Child text | Optional label supplied by the prefab. |

## Public API

`RoToggle` adds no API beyond `UnityEngine.UI.Toggle`.

| Member | Type | Description |
| --- | --- | --- |
| `isOn` | `bool` | Current selected state. |
| `group` | `ToggleGroup` | Optional group used for radio-button behavior. |
| `onValueChanged` | `ToggleEvent` | Invoked when the selected state changes. |
| `SetIsOnWithoutNotify(value)` | Method | Changes state without invoking the event. |

## Examples

```csharp
toggle.onValueChanged.AddListener(SetFeatureEnabled);
toggle.SetIsOnWithoutNotify(settings.FeatureEnabled);
```

## Related Components

- [Radio Toggle](radio-toggle.md)
- [ToggleSwitch](toggle-switch.md)

[Go Back](../README.md)
