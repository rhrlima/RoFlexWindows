# RoDropdown — Integration Spec

[Back to specs index](../README.md)

## Purpose

Package-styled `TMP_Dropdown` with required `Image`. Used for option selection in settings and forms.

## Current Public Interface

Inherits the full `TMP_Dropdown` API (`options`, `value`, `onValueChanged`, etc.).

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components;
using System.Collections.Generic;
using System.Linq;

qualityDropdown.ClearOptions();
qualityDropdown.AddOptions(qualities.Select(q => q.Label).ToList());
qualityDropdown.value = currentIndex;
qualityDropdown.onValueChanged.AddListener(host.OnQualityChanged);
```

[Back to specs index](../README.md)
