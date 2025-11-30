using System.Collections.Generic;
using UnityEngine;

public class SwapPanel : MonoBehaviour
{
    public List<GameObject> panels;
    public int activePanelIndex = 0;

    public void Start()
    {
        AutoFillEntries();
        SetActivePanel(activePanelIndex);
    }

    public void SetActivePanel(int index)
    {
        if (panels == null || panels.Count == 0) return;
        if (index < 0 || index >= panels.Count) return;

        activePanelIndex = index;

        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(i == activePanelIndex);
        }
    }

    public void ToggleNextPanel()
    {
        if (panels == null || panels.Count == 0) return;

        activePanelIndex = (activePanelIndex + 1) % panels.Count;
        SetActivePanel(activePanelIndex);
    }

    public void TogglePreviousPanel()
    {
        if (panels == null || panels.Count == 0) return;

        activePanelIndex = (activePanelIndex - 1 + panels.Count) % panels.Count;
        SetActivePanel(activePanelIndex);
    }

    void AutoFillEntries()
    {
        panels ??= new List<GameObject>();
        if (panels.Count == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var ch = transform.GetChild(i) as RectTransform;
                if (ch != null) panels.Add(ch.gameObject);
            }
            return;
        }
    }
}
