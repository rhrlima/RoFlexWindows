using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    public class ItemEntry : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IDropZone
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite sprite;
        [SerializeField] private TextMeshProUGUI itemAmount;
        [SerializeField] private int type;
        [SerializeField] private bool canDrag;
        [SerializeField] public DraggableItem dragHandler;
        [SerializeField] private int amount;

        private void Start()
        {
            itemAmount.text = amount.ToString();
            image.sprite = sprite;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (dragHandler == null) return;

            if (canDrag)
            {
                HideItem();
                dragHandler.SetData(new DragPayload(null, this, sprite, amount));
                dragHandler.HandleStartDrag(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (dragHandler == null) return;

            if (canDrag)
            {
                ShowItem();
                dragHandler.HandleEndDrag(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (dragHandler == null) return;
            if (!canDrag) return;

            dragHandler.HandleOnDrag(eventData);
        }

        private void ShowItem()
        {
            image.gameObject.SetActive(true);
            itemAmount.gameObject.SetActive(true);
        }

        private void HideItem()
        {
            image.gameObject.SetActive(false);
            itemAmount.gameObject.SetActive(false);
        }

        public bool CanDrop(DragPayload payload)
        {
            if (!(payload.Source is ItemEntry)) return false;

            var source = payload.Source as ItemEntry;
            Debug.Log($"{source.canDrag} {source.amount}");

            return false;
        }

        public bool Drop(DragPayload payload)
        {
            throw new System.NotImplementedException();
        }

    }
}