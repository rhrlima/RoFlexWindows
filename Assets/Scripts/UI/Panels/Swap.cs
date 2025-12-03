using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwapPanel : IPanel
{
    [Header("Swap Panel Settings")]
    public List<SwapablePanel> panels;
    public List<int> groups;
    public int activeGroup = 0;
    private int selectedIndex = 0;

    private void Start()
    {
        AutoFillEntries();
        SwapByIndex(0);
    }

    public void SwapByGroup(int group)
    {
        if (!groups.Contains(group)) return;
        SetActiveGroup(group);
    }

    public void SwapByIndex(int index)
    {
        if (index < 0 || index >= groups.Count) return;
        SwapByGroup(groups[index]);
    }

    private void SetActiveGroup(int group)
    {
        if (panels == null || panels.Count == 0) return;

        activeGroup = group;
        selectedIndex = groups.IndexOf(group);

        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].TogglePanel(panels[i].group == group);
        }
    }

    public void GetNextGroup()
    {
        var index = (selectedIndex + 1) % groups.Count;
        SwapByIndex(index);
    }

    public void GetPreviousGroup()
    {
        var index = (selectedIndex - 1 + groups.Count) % groups.Count;
        SwapByIndex(index);
    }

    private void AutoFillEntries()
    {
        panels ??= new List<SwapablePanel>();
        if (panels.Count == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var rect = transform.GetChild(i) as RectTransform;
                if (rect != null) panels.Add(new SwapablePanel()
                {
                    rect = rect,
                    group = i,
                    isActive = false
                });
            }
        }
        if (groups == null || groups.Count == 0)
        {
            groups = panels
                .Select(p => p.group)
                .Distinct()
                .ToList();    
        }
    }
}

[Serializable]
public class SwapablePanel
{
    public RectTransform rect;
    public int group;
    public bool isActive;
    public void TogglePanel(bool isActive)
    {
        rect.gameObject.SetActive(isActive);
        this.isActive = isActive;
    }
}
