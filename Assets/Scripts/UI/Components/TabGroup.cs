using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private GameObject buttonGroup;
    [SerializeField] private GameObject panelGroup;

    private List<TabButton> buttons;
    private List<TabPanel> panels;

    public void Start()
    {
        buttons = new List<TabButton>(
            buttonGroup.GetComponentsInChildren<TabButton>(true)
        );
        panels = new List<TabPanel>(
            panelGroup.GetComponentsInChildren<TabPanel>(true)
        );

        foreach (TabButton button in buttons)
        {
            button.TabGroup = this;
        }
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        button.SetActive(true);
        panels[button.transform.GetSiblingIndex()].gameObject.SetActive(true);
    }

    public void OnTabExit(TabButton button)
    {

    }

    public void OnTabSelected(TabButton button)
    {

    }

    public void ResetTabs()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetActive(false);
            panels[i].gameObject.SetActive(false);
        }
    }
}
