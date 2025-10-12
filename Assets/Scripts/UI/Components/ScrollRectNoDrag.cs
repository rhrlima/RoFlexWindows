using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoDragScrollRect : ScrollRect
{
    // Override OnBeginDrag to prevent dragging
    public override void OnBeginDrag(PointerEventData eventData)
    {
        // Intentionally left empty to block the start of a drag
    }

    // Override OnDrag to prevent dragging movement
    public override void OnDrag(PointerEventData eventData)
    {
        // Intentionally left empty to block the drag update
    }

    // Override OnEndDrag to prevent ending the drag
    public override void OnEndDrag(PointerEventData eventData)
    {
        // Intentionally left empty to block the end of a drag
    }

    // If you want to disable mouse wheel *entirely* on the panel, then uncomment and empty this:
    /*
    public override void OnScroll(PointerEventData data)
    {
        // Intentionally left empty to block mouse wheel scrolling
    }
    */
}