using RO_Flex_UI.Components;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPanel : MonoBehaviour, IDropHandler
{
    [SerializeField] private event System.Action<Draggable> onDrop;
    public void OnDrop(PointerEventData eventData)
    {
        if (!eventData.pointerDrag.TryGetComponent<Draggable>(out var draggable)) return;

        // draggable.EndDrag();
        // draggable.transform.SetParent(transform);

        // var dragTransform = draggable.transform as RectTransform;
        // dragTransform.anchoredPosition = Vector2.zero;

        // onDrop?.Invoke(draggable);
    }
}
