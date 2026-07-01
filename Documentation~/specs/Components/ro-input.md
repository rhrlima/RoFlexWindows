# RoInput — Integration Spec

[Back to specs index](../README.md)

## Purpose

Package-styled `TMP_InputField` for text entry in RO Flex UI forms and modals.

## Current Public Interface

Inherits the full `TMP_InputField` API (`text`, `onValueChanged`, `onSubmit`, `interactable`, etc.).

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components;

nameField.text = player.Name;
nameField.onSubmit.AddListener(host.OnNameSubmitted);
nameField.interactable = !isReadOnly;
```

No RO-specific binding layer is required unless shared validation or reference checks are added later.

[Back to specs index](../README.md)
