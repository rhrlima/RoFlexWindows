# SkillEntry — Integration Spec

[Back to specs index](../README.md)

## Purpose

Displays a skill name, level, and cost with increase/decrease controls. Supports passive skills (cost shows "Passive") and fixed-level skills that cannot be changed.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `Name` | `string` | Skill name text. |
| `Level` | `string` | Level display text. |
| `Cost` | `string` | Cost text; returns "Passive" when `IsPassive`. |
| `IsPassive` | `bool` | Passive skill flag. |
| `IsFixedLevel` | `bool` | Fixed level flag (stored but not enforced). |
| `onIncreaseLevel` | `UnityEvent` | Invoked when level-up button is clicked. |
| `onDecreaseLevel` | `UnityEvent` | Invoked when level-down button is clicked. |
| `onSkillLevelUp` | `UnityEvent` | Declared but never invoked. |
| `EnsureReferences()` | `bool` | Validates text and button references. |

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components;
using UnityEngine;

public void BindSkill(SkillEntry entry, SkillViewModel skill, System.Action<SkillViewModel, int> onLevelChange)
{
    entry.Configure(skill.Name, skill.Level.ToString(), skill.CostText, skill.IsPassive, skill.IsFixedLevel);

    entry.onIncreaseLevel.RemoveAllListeners();
    entry.onDecreaseLevel.RemoveAllListeners();
    entry.onIncreaseLevel.AddListener(() => onLevelChange(skill, +1));
    entry.onDecreaseLevel.AddListener(() => onLevelChange(skill, -1));
}
```

When `IsFixedLevel` is true, level buttons should be non-interactable. Passive skills should hide or disable cost controls.

## Related Scenarios

- [Skill list with level controls](../../integration-review.md#scenario-skill-list)

[Back to specs index](../README.md)
