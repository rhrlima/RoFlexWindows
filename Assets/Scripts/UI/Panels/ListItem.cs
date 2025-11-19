using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ListItem : MonoBehaviour, ISelectHandler, IPointerClickHandler, ISubmitHandler
{
    private int _index;
    private float _lastClickTime;

    [SerializeField] private float doubleClickThreshold = 0.3f;
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

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log($"SELECTED {_index}");
        _onSelectEvent.Invoke(this);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log($"SUBMITED {_index}");
        _onSubmitEvent.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"CLICKED {_index}");
        _onClickEvent.Invoke(this);

        var timeSinceLastClick = Time.time - _lastClickTime;
        if (timeSinceLastClick < doubleClickThreshold)
        {
            Debug.Log($"DOUBLE CLICKED {_index}");
            _onSubmitEvent.Invoke(this);
            _lastClickTime = 0;
        }

        _lastClickTime = Time.time;
    }

    public void ObtainSelectionFocus()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        _onSelectEvent.Invoke(this);
    }
}

[System.Serializable]
public class ListItemEvent : UnityEvent<ListItem> { }
