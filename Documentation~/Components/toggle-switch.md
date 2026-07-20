# ToggleSwitch

[Go Back](../README.md)

## Description

`ToggleSwitch` presents a two-state control built on Unity's `Slider`. Pointer
clicks and keyboard/controller submit actions toggle the state. A configurable
animation moves the handle between the slider's minimum and maximum values.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| `IsOn` | `bool` | Whether the slider value equals its maximum value. |

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `IsOn` | `bool` | Read-only toggle state. |

### Methods

| Method | Description |
| --- | --- |
| `SetIsOn(isOn, notify = true, animated = true)` | Changes state, optionally suppressing events or animation. |

### Events

| Event | Description |
| --- | --- |
| `onToggle` | Invoked with the new Boolean state. |
| `onToggleOn` | Invoked when the switch becomes on. |
| `onToggleOff` | Invoked when the switch becomes off. |

## Examples

```csharp
toggleSwitch.SetIsOn(settings.MusicEnabled, notify: false, animated: false);
toggleSwitch.onToggle.AddListener(SetMusicEnabled);
```

## Related Components

- [RoToggle](ro-toggle.md)

[Go Back](../README.md)
