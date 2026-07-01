# Header — Integration Spec

[Back to specs index](../README.md)

## Purpose

Window title bar with optional fun, minimize, and close buttons. Used on modal and standard windows.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `Text` | `string` | Window title. |
| `OnFunButtonClick` | `Button.ButtonClickedEvent` | Fun/auxiliary button click. |
| `OnMinButtonClick` | `Button.ButtonClickedEvent` | Minimize button click. |
| `OnCloseButtonClick` | `Button.ButtonClickedEvent` | Close button click. |
| `EnsureReferences()` | `bool` | Validates button and title references. |

Individual button visibility and interactable state are not exposed.

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components;
using UnityEngine;

public void BindWindowHeader(Header header, string title, bool showMinimize = true)
{
    header.Text = title;
    header.SetButtonVisible(HeaderButton.Minimize, showMinimize); // desired API
    header.OnCloseButtonClick.AddListener(CloseWindow);
}
```

Hosts should control which chrome buttons appear without disabling the entire header.

## Related Scenarios

- [Movable / resizable window](../../integration-review.md#scenario-window)

[Back to specs index](../README.md)
