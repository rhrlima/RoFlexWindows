namespace RO_Flex_UI.Components.DragAndDrop
{
    public interface IDragSource
    {
        DragPayload CreatePayload();
        bool CanDrag();
        void OnDragComplete();
    }
}