# RoToggle — Integration Spec

[Back to specs index](../README.md)

## Purpose

Package-styled `UnityEngine.UI.Toggle`. Provides consistent prefab styling with no additional RO behavior.

## Current Public Interface

Inherits the full `Toggle` API (`isOn`, `onValueChanged`, `interactable`, etc.). No `IComponent` implementation.

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components;

muteToggle.isOn = settings.Muted;
muteToggle.onValueChanged.AddListener(host.OnMuteChanged);
```

Documented as a styling anchor. Add `IComponent` reference validation only if prefabs require it.

[Back to specs index](../README.md)
