using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ListItem : MonoBehaviour, ISelectHandler, IPointerClickHandler, ISubmitHandler
{
    private ListPanel panel;
    private Image image;
    [HideInInspector] public int index;

    [SerializeField] private TMP_Text _optionText;

    [SerializeField] private ListItemEvent _onSelectEvent;
    [SerializeField] private ListItemEvent _onSubmitEvent;
    [SerializeField] private ListItemEvent _onClickEvent;

    public string OptionText {
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

        image = GetComponent<Image>();
        if (image == null)
            Debug.LogError("Image component not found in list item.");
    }

    public void OnOptionEnter()
    {
        SetAlpha(1.0f);
    }

    public void OnOptionExit()
    {
        SetAlpha(0f);
    }

    private void SetAlpha(float alphaValue)
    {
        Color color = image.color;
        color.a = alphaValue;
        image.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // panel.SelectOption(index);
        _onClickEvent.Invoke(this);

        Debug.Log($"CLICKED {index}");
    }

    public void OnSelect(BaseEventData eventData)
    {
        _onSelectEvent.Invoke(this);

        Debug.Log($"SELECTED {index}");

        panel.autoScroll.FitOptiontoView(index);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        _onSubmitEvent.Invoke(this);

        Debug.Log($"SUBMITED {index}");
    }

    public void ObtainSelectionFocus()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        _onSelectEvent.Invoke(this);
    }
}

[System.Serializable]
public class ListItemEvent : UnityEvent<ListItem> { }
