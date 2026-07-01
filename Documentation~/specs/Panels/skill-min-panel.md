# SkillMinPanel — Integration Spec

[Back to specs index](../README.md)

## Purpose

Container for a scrollable list of `SkillEntry` rows in the skill window. Intended to mirror `ListPanel` population patterns for skills.

## Current Public Interface

| Field | Type | Description |
| --- | --- | --- |
| `skillEntryPrefab` | `SkillEntry` | Row template (public field). |
| `contentPanel` | `Transform` | Parent for instantiated entries (public field). |

No `IPanel`, no `EnsureReferences`, no population or event API.

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components;
using RO_Flex_UI.Panels;

skillMinPanel.Clear();
skillMinPanel.AddSkills(skills, (entry, skill) =>
{
    entry.Configure(skill.Name, skill.Level, skill.Cost, skill.IsPassive, skill.IsFixedLevel);
    entry.onIncreaseLevel.AddListener(() => host.LevelUp(skill));
    entry.onDecreaseLevel.AddListener(() => host.LevelDown(skill));
});
```

Should follow the same clear → add → bind flow as `ListPanel`.

## Related Scenarios

- [Skill list with level controls](../../integration-review.md#scenario-skill-list)

[Back to specs index](../README.md)
