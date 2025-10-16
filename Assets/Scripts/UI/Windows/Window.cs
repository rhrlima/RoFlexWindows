using UnityEngine;
using UnityEngine.EventSystems;

public class Window : MonoBehaviour, IWindow, IPointerDownHandler
{
    // Window Options
    [SerializeField] private bool resetToCenter = true;
    [SerializeField] private bool isDraggable = true;
    [SerializeField] private bool isResizable = true;


    // Internal 
    private Draggable draggableComponent;
    private Resizable resizeComponent;


    protected virtual void Awake()
    {
        draggableComponent = GetComponentInChildren<Draggable>(true);
        resizeComponent = GetComponentInChildren<Resizable>(true);
    }

    private void OnValidate()
    {
        ToggleDraggable();
        ToggleResisable();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left
         && eventData.button != PointerEventData.InputButton.Right)
            return;

        // bring it forward
        transform.SetAsLastSibling();
    }

    public void ToggleVisibility()
    {
        if (gameObject.activeInHierarchy)
            HideWindow();
        else
            ShowWindow();
    }

    public void ToggleDraggable()
    {
        if (draggableComponent != null && isDraggable != draggableComponent.enabled)
        {
            draggableComponent.enabled = isDraggable;
        }
    }

    public void ToggleResisable()
    {
        if (resizeComponent != null && isResizable != resizeComponent.enabled)
        {
            resizeComponent.gameObject.SetActive(isResizable);
            resizeComponent.enabled = isResizable;
        }
    }

    public void ShowWindow()
    {
        // TODO manager to close using ESC
        var mgr = WindowManager.GetInstance();
        if (!mgr.Contains(this))
            mgr.PushWindow(this);

        if (resetToCenter)
            CenterWindow();
        else
            FitWindowIntoPlayArea();

        // bring it forward
        transform.SetAsLastSibling();

        gameObject.SetActive(true);
    }

    public void HideWindow()
    {
        gameObject.SetActive(false);
    }

    public void CenterWindow()
    {
        var canvas = gameObject.transform.parent as RectTransform;
        transform.position = canvas.rect.size / 2f;
    }

    public void FitWindowIntoPlayArea()
    {
        //TODO
        Debug.LogWarning("NOT IMPLEMENTED");
    }
}
