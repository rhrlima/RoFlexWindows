# Input Field

[Go Back](../README.md)

## Description

The Input Field prefab is a styled TextMesh Pro `TMP_InputField`. The package
also exposes `RoInput`, a thin subclass that adds no behavior, fields, or events.
Use the inherited TextMesh Pro API to read input and react to edits.

## Displayed Data

| Data | Source | Description |
| --- | --- | --- |
| Input text | `TMP_InputField.text` | Current editable value. |
| Placeholder | `TMP_InputField.placeholder` | Content shown while the value is empty. |

## Public API

`RoInput` adds no API beyond `TMP_InputField`.

| Member | Type | Description |
| --- | --- | --- |
| `text` | `string` | Current input value. |
| `onValueChanged` | `OnChangeEvent` | Raised whenever the value changes. |
| `onEndEdit` | `SubmitEvent` | Raised when editing ends. |
| `ActivateInputField()` | Method | Gives the field editing focus. |

## Examples

```csharp
[SerializeField] private TMP_InputField characterName;

private void Awake()
{
    characterName.onEndEdit.AddListener(SaveCharacterName);
}
```

[Go Back](../README.md)
