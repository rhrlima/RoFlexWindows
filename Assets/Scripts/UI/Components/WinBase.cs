using UnityEngine;
using UnityEngine.EventSystems;

public class WinBase : MonoBehaviour, IPointerDownHandler
{
    // Internal References
    private Draggable draggablePanel;

    // Window Options
    public bool resetToCenter = true;

    public void Awake()
    {
        draggablePanel = GetComponentInChildren<Draggable>(true);
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

    public void ShowWindow() {
        if (gameObject == null)
            return;

        gameObject.SetActive(true);

        // var mgr = UiManager.Instance;
        // if (!mgr.WindowStack.Contains(this))
        //     mgr.WindowStack.Add(this);

        if (resetToCenter)
            CenterWindow();
        // else
        //     FitWindowIntoPlayArea();

        // bring it forward
        transform.SetAsLastSibling();

        // ((RectTransform)transform).ForceUpdateRectTransforms();
    }

    public void HideWindow() {

        if (gameObject == null)
            return;

        // if (draggablePanel) draggablePanel.EndDrag();

        gameObject.SetActive(false);
    }

    public void CenterWindow()
    {
        var canvas = (RectTransform) gameObject.transform.parent;
        transform.position = canvas.rect.size / 2f;
    }
}
