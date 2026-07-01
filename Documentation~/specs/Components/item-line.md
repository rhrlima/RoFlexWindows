# ItemLine — Integration Spec

[Back to specs index](../README.md)

## Purpose

Displays a primary label with a row of gem or socket slots (`IconAmount` instances). Used for equipment lines that show socketed items alongside a name.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `Socket` | Nested class | `open` flag and `IconAmount slot` reference. |
| `numSockets` | `int` | Read-only count of configured sockets. |
| `EnsureReferences()` | `bool` | Validates template and text references. |
| `Text` | `string` | Primary line label. |

Socket list and template are private; there is no runtime API to populate or resize sockets.

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components;
using System.Collections.Generic;
using UnityEngine;

public void BindItemLine(ItemLine line, string name, IReadOnlyList<SocketData> sockets)
{
    line.Text = name;
    line.ConfigureSockets(sockets); // desired API
}

// Per-socket binding
foreach (var socket in line.Sockets) // desired read-only access
{
    if (socket.open && socket.gem != null)
        socket.slot.Assign(socket.gem.Icon, socket.gem.Amount);
    else
        socket.slot.Clear();
}
```

Consumers need:

- `ConfigureSockets(IReadOnlyList<SocketData>)` or per-index `AssignSocket(int, Sprite, int, bool open)`
- `ResizeSockets(int count)` when socket count changes at runtime
- Read-only access to socket `IconAmount` instances for drag-and-drop wiring

## Related Scenarios

- [Item line with gem sockets](../../integration-review.md#scenario-item-line-sockets)

[Back to specs index](../README.md)
