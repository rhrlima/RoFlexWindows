# SwapPanel

[Go Back](../README.md)

## Description

`SwapPanel` activates all configured child panels belonging to one group and
deactivates the rest. Multiple panels may share a group ID and become visible
together. When entries or groups are empty, direct children are discovered and
assigned sequential group IDs during `Start`.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| `activeGroup` | `int` | Group ID most recently selected. |
| Panel active state | `bool` | Whether an entry's GameObject belongs to the active group. |

## Public API

### Methods

| Method | Description |
| --- | --- |
| `SwapByIndex(index, notify = true)` | Activates the group at a valid zero-based group-list index. |
| `SwapByGroup(group, notify = true)` | Activates a configured group ID. |
| `GetNextGroup(notify = true)` | Wraps to the next configured group. |
| `GetPreviousGroup(notify = true)` | Wraps to the previous configured group. |
| `EnsureReferences()` | Currently throws `NotImplementedException`; do not call it. |

### Events

| Event | Description |
| --- | --- |
| `onSwapEvent` | C# `Action` invoked after a successful notified swap. |

On startup the panel selects group-list index `0` without invoking
`onSwapEvent`. Invalid indexes and group IDs are ignored.

## Examples

```csharp
swapPanel.onSwapEvent += RefreshVisiblePanel;
swapPanel.SwapByGroup(2);

// Change selection without notifying listeners.
swapPanel.GetNextGroup(notify: false);
```

## Related Panels

- [TabsPanel](tabs-panel.md)

[Go Back](../README.md)
