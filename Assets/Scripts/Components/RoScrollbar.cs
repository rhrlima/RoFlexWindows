using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    [DisallowMultipleComponent]
    public class RoScrollbar : Scrollbar
    {
        [Serializable]
        public struct ButtonSprites
        {
            [SerializeField] private Sprite leftSprite;
            [SerializeField] private Sprite rightSprite;
            [SerializeField] private Sprite upSprite;
            [SerializeField] private Sprite downSprite;

            public readonly Sprite GetDecreaseSprite(Direction scrollbarDirection)
            {
                return scrollbarDirection switch
                {
                    Direction.LeftToRight => leftSprite,
                    Direction.RightToLeft => rightSprite,
                    Direction.BottomToTop => downSprite,
                    Direction.TopToBottom => upSprite,
                    _ => leftSprite
                };
            }

            public readonly Sprite GetIncreaseSprite(Direction scrollbarDirection)
            {
                return scrollbarDirection switch
                {
                    Direction.LeftToRight => rightSprite,
                    Direction.RightToLeft => leftSprite,
                    Direction.BottomToTop => upSprite,
                    Direction.TopToBottom => downSprite,
                    _ => rightSprite
                };
            }
        }

        [SerializeField] private float stepPerc = 0.2f;
        [SerializeField] private RoButton decreaseButton;
        [SerializeField] private RoButton increaseButton;
        [SerializeField] private ButtonSprites buttonSprites;

        public ScrollEvent onDecreaseClick = new ScrollEvent();
        public ScrollEvent onIncreaseClick = new ScrollEvent();
        public ScrollEvent onEndScroll = new ScrollEvent();

        private bool scrollStarted;

        protected override void OnEnable()
        {
            base.OnEnable();

            ApplyButtonSprites();

            if (decreaseButton != null)
                decreaseButton.onClick.AddListener(OnDecreaseClick);

            if (increaseButton != null)
                increaseButton.onClick.AddListener(OnIncreaseClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            scrollStarted = false;

            if (decreaseButton != null)
                decreaseButton.onClick.RemoveListener(OnDecreaseClick);

            if (increaseButton != null)
                increaseButton.onClick.RemoveListener(OnIncreaseClick);
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            ApplyButtonSprites();
        }

        private void OnDecreaseClick()
        {
            value -= stepPerc;
            onDecreaseClick?.Invoke(value);
        }

        private void OnIncreaseClick()
        {
            value += stepPerc;
            onIncreaseClick?.Invoke(value);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            scrollStarted = MayScroll(eventData);
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            var shouldInvokeEndScroll = scrollStarted && MayScroll(eventData);
            scrollStarted = false;

            base.OnPointerUp(eventData);

            if (shouldInvokeEndScroll)
                onEndScroll?.Invoke(value);
        }

        private bool MayScroll(PointerEventData eventData)
        {
            return eventData != null &&
                   eventData.button == PointerEventData.InputButton.Left &&
                   IsActive() &&
                   IsInteractable();
        }

        private void ApplyButtonSprites()
        {
            SetButtonSprite(decreaseButton, buttonSprites.GetDecreaseSprite(direction));
            SetButtonSprite(increaseButton, buttonSprites.GetIncreaseSprite(direction));
        }

        private static void SetButtonSprite(RoButton button, Sprite sprite)
        {
            if (button == null || sprite == null)
                return;

            if (button.targetGraphic is Image image)
            {
                image.sprite = sprite;
                return;
            }

            if (button.TryGetComponent(out Image buttonImage))
                buttonImage.sprite = sprite;
        }
    }
}
