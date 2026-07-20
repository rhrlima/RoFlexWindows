# RoButton

[Go Back](../README.md)

## Description

`RoButton` is a clickable Unity UI component for RO Flex UI. It extends
`UnityEngine.UI.Button`, implements the package's `IComponent` reference-check
contract, and requires an `Image` component on the same GameObject.

## Displayed Data

`RoButton` does not define its own display data. Its appearance comes from the
inherited `Button` configuration and the required `Image` component. Text,
icons, and other content can be added as child UI elements.

| Data | Source | Description |
| --- | --- | --- |
| Button image | `Image` | Displays the button background or sprite. |
| Interaction state | `Selectable` | Applies the configured normal, highlighted, pressed, selected, and disabled visuals. |
| Child content | Child UI elements | Displays optional labels, icons, or other button content. |

## Public API

`RoButton` inherits its interaction API from `UnityEngine.UI.Button`.

| Member | Type | Description |
| --- | --- | --- |
| `interactable` | `bool` | Enables or disables user interaction. |
| `onClick` | `ButtonClickedEvent` | Invoked when the user clicks the button. |
| `Select()` | Method | Selects the button through Unity's event system. |
| `EnsureReferences()` | Method | Currently returns `true`; available for subclasses that validate additional references. |

## Examples

### Handle a Click

```csharp
using RO_Flex_UI.Components;
using UnityEngine;

public class ConfirmAction : MonoBehaviour
{
    [SerializeField] private RoButton confirmButton;

    private void Awake()
    {
        confirmButton.onClick.AddListener(Confirm);
    }

    private void Confirm()
    {
        Debug.Log("Confirmed");
    }
}
```

### Update Interaction

```csharp
public void SetCanConfirm(bool canConfirm)
{
    confirmButton.interactable = canConfirm;
}
```

## Related Components

- [Header](header.md)
- [ListItem](list-item.md)
- [TabsPanel](../Panels/tabs-panel.md)

[Go Back](../README.md)
