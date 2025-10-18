using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private GameObject buttonGroup;
    [SerializeField] private GameObject panelGroup;
    protected List<TabButton> buttons;
    protected List<TabPanel> panels;

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

    public virtual void OnTabEnter(TabButton button)
    {
        ResetTabs();
        button.SetActive(true);
        panels[button.transform.GetSiblingIndex()].gameObject.SetActive(true);
    }

    public virtual void OnTabExit(TabButton button)
    {

    }

    public virtual void OnTabSelected(TabButton button)
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
