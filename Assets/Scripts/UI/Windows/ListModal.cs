using System.Collections.Generic;
using UnityEngine;

public class ListModal : Window, IModalWindow
{
    private ListPanel listPanel;

    private void Start()
    {
        listPanel = GetComponentInChildren<ListPanel>();

        if (listPanel == null)
            Debug.LogError("Content Panel of type 'ListPanel' not found.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnConfirm();
        }
    }

    public override void ShowWindow()
    {
        listPanel.SelectOption(0);
        base.ShowWindow();
    }

    public void OnConfirm()
    {
        var selectedValue = listPanel.GetSelectedOption();
        Debug.Log($"SELECTED OPTION: {selectedValue}");
        HideWindow();
    }

    public void OnCancel()
    {
        listPanel.SelectOption(0);
        HideWindow();
    }

    public void ShowModal(List<string> options)
    {
        // listPanel.SetOptions(options);
        ShowWindow();
    }
}
