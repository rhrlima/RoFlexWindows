using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// look at InputWindow, for a delegate callback example

public class ListModal : Window
{
    private Action<ListItem> _onConfirmAction;
    private Action<ListItem> _onCancelAction;

    [SerializeField] private ListPanel listPanel;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private UnityEvent _onConfirmEvent;
    [SerializeField] private UnityEvent _onCancelEvent;

    public UnityEvent OnConfirmEvent
    {
        get => _onConfirmEvent;
        set => _onConfirmEvent = value;
    }

    public UnityEvent OnCancelEvent
    {
        get => _onCancelEvent;
        set => _onCancelEvent = value;
    }

    public void ShowModal(List<string> options,
        Action<ListItem> onConfirm,
        Action<ListItem> onClick = null,
        Action<ListItem> onSelect = null,
        Action<ListItem> onCancel = null
    )
    {
        ShowWindow();

        listPanel.SetOptions(options);
        listPanel._onOptionSubmitAction = onConfirm;
        listPanel._onOptionClickAction = onClick;
        listPanel._onOptionSelectAction = onSelect;

        _onConfirmAction = onConfirm;
        _onCancelAction = onCancel;
    }

    public void OnConfirm()
    {
        _onConfirmEvent.Invoke();
        HideWindow();
    }

    public void OnCancel()
    {
        _onCancelEvent.Invoke();
        HideWindow();
    }
}
