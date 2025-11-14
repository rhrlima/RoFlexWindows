using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ListItem : MonoBehaviour, ISelectHandler, IPointerClickHandler, ISubmitHandler
{
    private ListPanel panel;
    private int _index;
    [SerializeField] private TMP_Text _optionText;
    [SerializeField] private ListItemEvent _onSelectEvent;
    [SerializeField] private ListItemEvent _onSubmitEvent;
    [SerializeField] private ListItemEvent _onClickEvent;

    public int Index
    {
        get => _index;
        set => _index = value;
    }

    public string OptionText
    {
        get => _optionText.text;
        set => _optionText.text = value;
    }

    public ListItemEvent OnSelectEvent
    {
        get => _onSelectEvent;
        set => _onSelectEvent = value;
    }

    public ListItemEvent OnSubmitEvent
    {
        get => _onSubmitEvent;
        set => _onSubmitEvent = value;
    }

    public ListItemEvent OnClickEvent
    {
        get => _onClickEvent;
        set => _onClickEvent = value;
    }

    private void Start()
    {
        panel = GetComponentInParent<ListPanel>();
        if (panel == null)
            Debug.LogError("ListPanel script not found in parent.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"CLICKED {_index}");
        _onClickEvent.Invoke(this);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log($"SELECTED {_index}");
        _onSelectEvent.Invoke(this);
        // panel.autoScroll.FitOptiontoView(index);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log($"SUBMITED {_index}");
        _onSubmitEvent.Invoke(this);
    }

    public void ObtainSelectionFocus()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        _onSelectEvent.Invoke(this);
    }
}

[System.Serializable]
public class ListItemEvent : UnityEvent<ListItem> { }
