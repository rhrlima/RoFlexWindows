namespace RO_Flex_UI.Components
{
    public enum DragSourceDisposition
    {
        Restore,
        Clear,
    }

    public readonly struct DropResult
    {
        private DropResult(bool accepted, DragSourceDisposition sourceDisposition)
        {
            Accepted = accepted;
            SourceDisposition = sourceDisposition;
        }

        public bool Accepted { get; }
        public DragSourceDisposition SourceDisposition { get; }

        public static DropResult Rejected => default;
        public static DropResult Move => new(true, DragSourceDisposition.Clear);
        public static DropResult Copy => new(true, DragSourceDisposition.Restore);
        public static DropResult Swap => Copy;
    }

    public interface IDropZone
    {
        bool CanDrop(DragPayload payload);
        DropResult Drop(DragPayload payload);
    }
}
