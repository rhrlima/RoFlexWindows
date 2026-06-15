namespace RO_Flex_UI.Components
{
    public interface IDropZone
    {
        bool CanDrop(DragPayload payload);
        bool Drop(DragPayload payload);
    }
}