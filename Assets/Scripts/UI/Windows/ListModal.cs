using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// look at InputWindow, for a delegate callback example

public class ListModal : Window
{
    [Header("List Settings")]
    [SerializeField] private ListPanel listPanel;
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
