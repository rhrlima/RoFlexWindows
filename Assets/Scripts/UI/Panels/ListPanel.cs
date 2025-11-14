using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListPanel : MonoBehaviour
{
    public AutoScroll autoScroll;
    public int numOptions;
    [SerializeField] private List<ListItem> listOptions;
    [SerializeField] private ListItem listItemPrefab;
    [SerializeField] private ListItemEvent _onOptionSelectEvent;
    [SerializeField] private ListItemEvent _onOptionSubmitEvent;
    [SerializeField] private ListItemEvent _onOptionClickEvent;

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

    public void Start()
    {
        if (numOptions > 0)
        {
            CreateOptions(numOptions);
            UpdateNavigation();
        }
        SelectOption(0);
    }

    private void UpdateNavigation()
    {
        ListItem[] children = GetComponentsInChildren<ListItem>();

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

    public void CreateOptions(int numOptions)
    {
        for (int i = 0; i < numOptions; i++)
        {
            ListItem newItem = Instantiate(listItemPrefab, transform);
            newItem.Index = i;
            newItem.OptionText = $"Option {i + 1}";

            newItem.OnSelectEvent.AddListener((ListItem) => HandleOnOptionSelect(newItem));
            newItem.OnSubmitEvent.AddListener((ListItem) => _onOptionSubmitEvent.Invoke(newItem));
            newItem.OnClickEvent.AddListener((ListItem) => _onOptionClickEvent.Invoke(newItem));
        }
    }

    private void HandleOnOptionSelect(ListItem option)
    {
        _onOptionSelectEvent.Invoke(option);
        autoScroll.FitOptiontoView(option.Index);
    }

    public void SelectOption(int index)
    {
        GameObject option = transform.GetChild(index).gameObject;
        var item = option.GetComponent<ListItem>();
        item.ObtainSelectionFocus();
    }
}
