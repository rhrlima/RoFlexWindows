using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListPanel : MonoBehaviour
{
    private int _selectedIndex;

    [SerializeField] private bool autoScroll = true;
    [SerializeField] private RectTransform viewport;
    [SerializeField] private RectTransform content;
    [SerializeField] private ListItem listItemPrefab;
    [SerializeField] private ListItemEvent _onOptionSelectEvent;
    [SerializeField] private ListItemEvent _onOptionSubmitEvent;
    [SerializeField] private ListItemEvent _onOptionClickEvent;

    public Action<ListItem> _onOptionSubmitAction;
    public Action<ListItem> _onOptionClickAction;
    public Action<ListItem> _onOptionSelectAction;

    public int SelectedIndex
    {
        get => _selectedIndex;
        set => _selectedIndex = value;
    }

    public ListItemEvent OnOptionSelectEvent
    {
        get => _onOptionSelectEvent;
        set => _onOptionSelectEvent = value;
    }

    public ListItemEvent OnOptionSubmitEvent
    {
        get => _onOptionSubmitEvent;
        set => _onOptionSubmitEvent = value;
    }

    public ListItemEvent OnOptionClickEvent
    {
        get => _onOptionClickEvent;
        set => _onOptionClickEvent = value;
    }

    public void ClearOptions()
    {
        var toDestroy = transform.GetComponentsInChildren<ListItem>(true);

        foreach (ListItem child in toDestroy)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator SetOptionsRoutine(List<string> options)
    {
        ClearOptions();
        yield return null;

        for (int i = 0; i < options.Count; i++)
        {
            ListItem newItem = Instantiate(listItemPrefab, transform);
            newItem.Index = i;
            newItem.OptionText = options[i];

            newItem.OnSelectEvent.AddListener((ListItem) => HandleOnOptionSelect(newItem));
            newItem.OnSubmitEvent.AddListener((ListItem) => HandleOnOptionSubmit(newItem));
            newItem.OnClickEvent.AddListener((ListItem) => HandleOnOptionClick(newItem));
        }

        UpdateNavigation();
        SelectOption(0);
    }

    public void SetOptions(List<string> options)
    {
        StartCoroutine(SetOptionsRoutine(options));
    }

    public void UpdateNavigation()
    {
        ListItem[] children = GetComponentsInChildren<ListItem>(true);

        if (children.Length < 2) return;

        for (int i = 0; i < children.Length; i++)
        {
            var item = children[i];
            var nav = item.GetComponent<Button>().navigation;

            nav.selectOnUp = children[(i - 1 + children.Length) % children.Length].GetComponent<Button>();
            nav.selectOnDown = children[(i + 1) % children.Length].GetComponent<Button>();

            item.gameObject.GetComponent<Button>().navigation = nav;
        }
    }

    private void HandleOnOptionSelect(ListItem option)
    {
        FitOptiontoView(option.Index);
        SelectedIndex = option.Index;

        _onOptionSelectEvent?.Invoke(option);
        _onOptionSelectAction?.Invoke(option);
    }

    private void HandleOnOptionSubmit(ListItem option)
    {
        SelectedIndex = option.Index;

        _onOptionSubmitEvent?.Invoke(option);
        _onOptionSubmitAction?.Invoke(option);
    }

    private void HandleOnOptionClick(ListItem option)
    {
        SelectedIndex = option.Index;

        _onOptionClickEvent?.Invoke(option);
        _onOptionClickAction?.Invoke(option);
    }

    public void SelectOption(int index)
    {
        SelectedIndex = index;

        GameObject option = transform.GetChild(index).gameObject;
        var item = option.GetComponent<ListItem>();
        item.ObtainSelectionFocus();
    }

    public void ConfirmOption()
    {
        var option = transform.GetChild(SelectedIndex).gameObject;
        var item = option.GetComponent<ListItem>();

        _onOptionSubmitAction?.Invoke(item);
        _onOptionSubmitAction?.Invoke(item);
    }

    public void FitOptiontoView(int index)
    {
        if (!autoScroll) return;

        var itemRect = transform.GetChild(index).transform as RectTransform;

        float itemHeight = itemRect.rect.height;
        float itemYPos = itemRect.localPosition.y;

        float viewportHeight = viewport.rect.height;

        // makes sure the content default position is zero (top aligned)
        float currentContentY = content.localPosition.y - viewportHeight / 2;

        float targetTopY = currentContentY + itemYPos;
        float targetBottomY = -itemYPos + itemHeight - viewportHeight - currentContentY;

        if (targetBottomY > 0)
        {
            content.localPosition += new Vector3(0, targetBottomY, 0);
        }

        if (targetTopY > 0)
        {
            content.localPosition -= new Vector3(0, targetTopY, 0);
        }
    }
}
