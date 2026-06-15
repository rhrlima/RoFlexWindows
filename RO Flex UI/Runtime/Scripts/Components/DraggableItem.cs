using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    public readonly struct DragPayload
    {
        public readonly object Data;
        public readonly object Source;
        public readonly Sprite Sprite;
        public readonly int Amount;

        public DragPayload(object data, object source, Sprite sprite, int amount = 0)
        {
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
            image.gameObject.SetActive(false);
            amount.gameObject.SetActive(false);
        }

        public override void HandleStartDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
            transform.position = eventData.position;

            image.gameObject.SetActive(true);
            amount.gameObject.SetActive(true);
        }

        public override void HandleEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;

            image.gameObject.SetActive(false);
            amount.gameObject.SetActive(false);
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