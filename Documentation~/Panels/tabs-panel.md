# TabsPanel

[Go Back](../README.md)

## Description

`TabsPanel` manages a group of tab buttons and their matching content panels.
Each configured entry pairs one `TabButton` with one panel `GameObject`.

When a tab is selected, `TabsPanel` deactivates the previous panel, activates
the selected panel, updates both tab buttons through `TabButton.SetActive`, and
invokes enter and exit events with the tab index.

## Displayed Data

| Data | Source | Description |
| --- | --- | --- |
| Active tab | `CurrentIndex` | Zero-based index of the selected entry, or `-1` when no tab is active. |
| Tab visual state | `TabButton` | Applies active and idle sprites through `SetActive(bool)`. |
| Content panel | `GameObject` | Shown for the active tab and hidden for inactive tabs. |
| Enter and exit behavior | `TabEvent` | Invokes Inspector-configured callbacks with the tab index. |

## Configuration

| Field | Description |
| --- | --- |
| `entries` | Ordered list of tab button and panel pairs. The entry position is the tab index. |
| `entries.active` | Runtime state showing whether this entry is currently active. |
| `entries.tabButton` | `TabButton` clicked to select this entry. |
| `entries.tabPanel` | Panel object shown when this entry is selected. |
| `entries.onPanelEnter` | Event invoked with this entry's index after the panel is activated. |
| `entries.onPanelExit` | Event invoked with this entry's index before the panel is deactivated. |
| `defaultTabIndex` | Entry selected during `Start` when default selection is enabled. |
| `selectDefaultOnStart` | Selects `defaultTabIndex` during initialization when true. When false, all panels start inactive. |

Configure active and idle sprites on each `TabButton`. `TabsPanel` only tells
the button whether it is selected.

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `CurrentIndex` | `int` | Current tab index, or `-1` when no tab is active. |

### Methods

| Method | Description |
| --- | --- |
| `SetActiveTab(index)` | Selects a valid zero-based tab index. Invalid indexes and repeated selection are ignored. |

### Events

| Event | Type | Description |
| --- | --- | --- |
| `onPanelEnter` | `UnityEvent<int>` | Invoked after a tab's panel is activated. Receives the active tab index. |
| `onPanelExit` | `UnityEvent<int>` | Invoked before a tab's panel is deactivated. Receives the exiting tab index. |

## Examples

### Select a Tab From Code

```csharp
using RO_Flex_UI.Panels;
using UnityEngine;

public class OpenInventoryTab : MonoBehaviour
{
    [SerializeField] private TabsPanel tabsPanel;

    public void OpenEquipment()
    {
        tabsPanel.SetActiveTab(1);
    }
}
```

### React to Tab Events

```csharp
using UnityEngine;

public class TabLogger : MonoBehaviour
{
    public void OnPanelEnter(int index)
    {
        Debug.Log($"Entered tab {index}");
    }

    public void OnPanelExit(int index)
    {
        Debug.Log($"Exited tab {index}");
    }
}
```

Assign these methods to an entry's `onPanelEnter` and `onPanelExit` events in
the Inspector.

[Go Back](../README.md)
