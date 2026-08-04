using System.Globalization;
using RO_Flex_UI.Components;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(IconAmount), typeof(DraggableItem))]
public sealed class ItemExample : MonoBehaviour
{
    [SerializeField] private Item item = new();

    public Item Data => item;

    public DragPresentation Presentation => new(
        item.sprite,
        item.amount.ToString(CultureInfo.InvariantCulture),
        item.name);

    private void Start()
    {
        var visual = GetComponent<IconAmount>();
        var draggable = GetComponent<DraggableItem>();
        if (visual == null || draggable == null)
            return;

        visual.TryApplyPresentation(Presentation);
        visual.SetActive(item.sprite != null);
        draggable.Configure(item, visual, Presentation);
    }
}
