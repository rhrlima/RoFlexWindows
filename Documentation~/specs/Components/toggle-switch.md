# ToggleSwitch — Integration Spec

[Back to specs index](../README.md)

## Purpose

Animated on/off control built on `Slider` semantics. Provides boolean toggle with slide animation and separate on/off events.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `IsOn` | `bool` | True when slider value is at max (may reflect animation mid-flight). |
| `SetIsOn(bool, bool notify, bool animated)` | Method | Sets state with optional notification and animation. |
| `onToggle` | `ToggleEvent` | `UnityEvent<bool>` on state change. |
| `onToggleOn` / `onToggleOff` | `UnityEvent` | Directional toggle events. |

Does not implement `IComponent`. `OnMove` is empty, blocking inherited gamepad navigation.

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components;

notificationsSwitch.SetIsOn(settings.NotificationsEnabled, notify: false);
notificationsSwitch.onToggle.AddListener(host.OnNotificationsToggled);

// For logic reads during animation, use committed state:
// notificationsSwitch.CommittedState — desired API
```

[Back to specs index](../README.md)
