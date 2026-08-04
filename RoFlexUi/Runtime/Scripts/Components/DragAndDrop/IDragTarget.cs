namespace RO_Flex_UI.Components.DragAndDrop
{
    public interface IDragTarget
    {
        bool CanDrop(DragPayload payload);
        void OnDropComplete(DragPayload payload);
    }
}