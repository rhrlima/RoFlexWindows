using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// TODO
// draggable item does not need to be extended from draggable base
// can be component added to others, and "drag a reference"
// take into account the new IconAmount and IconText

namespace RO_Flex_UI.Components
{
    public readonly struct DragPayload
    {
        public readonly Vector2 OriginPosition;
        public readonly object Data;
        public readonly object Source;
        public readonly Sprite Sprite;
        public readonly int Amount;

        public DragPayload(Vector2 originPosition, object data, object source, Sprite sprite, int amount = 0)
        {
            OriginPosition = originPosition;
            Data = data;
            Source = source;
            Sprite = sprite;
            Amount = amount;
        }
    }

    public class DraggableItem : DraggableBase
    {
        [SerializeField] private IconAmount target;
        private IconAmount source;

        public override void Start()
        {
            base.Start();
            if (!EnsureReferences()) return;
        }

        public override bool EnsureReferences()
        {
            source = GetComponent<IconAmount>();
            return base.EnsureReferences() && target.EnsureReferences();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            target.Sprite = source.Sprite;
            target.Text = source.Text;
            target.transform.position = eventData.position;

            source.SetActive(false);
            target.SetActive(true);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (canvas == null || target == null) return;

            if (!dragging) return;

            var rectTransform = target.transform as RectTransform;
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            target.SetActive(false);
            source.SetActive(true);
        }

        // public void SetData(DragPayload payload)
        // {
        //     image.sprite = payload.Sprite;
        //     amount.text = payload.Amount.ToString();
        // }

        // public void OnDrag(PointerEventData eventData)
        // {
        //     transform.position = eventData.position;
        // }
    }
}