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
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text amount;

        private CanvasGroup canvasGroup;

        public override void Start()
        {
            base.Start();

            canvasGroup = transform.GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = true;
            image.gameObject.SetActive(false);
            amount.gameObject.SetActive(false);
        }

        public override void HandleStartDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
            transform.position = eventData.position;

            gameObject.SetActive(true);
            image.gameObject.SetActive(true);
            amount.gameObject.SetActive(true);
        }

        public override void HandleEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;

            image.gameObject.SetActive(false);
            amount.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        public void SetData(DragPayload payload)
        {
            image.sprite = payload.Sprite;
            amount.text = payload.Amount.ToString();
        }

        public override void HandleOnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }
    }
}