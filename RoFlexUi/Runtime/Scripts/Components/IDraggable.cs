namespace RO_Flex_UI.Components
{
    public interface IDraggable
    {
        bool Dragging { get; }
        bool CanResolveDrop { get; }
        DragPayload CurrentPayload { get; }
        void Configure(object data, object context = null, DragPresentation? presentation = null);
        bool TryDrop(IDropZone dropZone);
    }
}
