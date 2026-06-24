using RO_Flex_UI.Utils;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    public class RoSlider : Slider, IComponent
    {
        [Serializable]
        public struct ButtonSprites
        {
            [SerializeField] private Sprite leftSprite;
            [SerializeField] private Sprite rightSprite;
            [SerializeField] private Sprite upSprite;
            [SerializeField] private Sprite downSprite;

            public readonly Sprite GetDecreaseSprite(Slider.Direction direction)
            {
                return direction switch
                {
                    Direction.LeftToRight => leftSprite,
                    Direction.RightToLeft => rightSprite,
                    Direction.BottomToTop => downSprite,
                    Direction.TopToBottom => upSprite,
                    _ => leftSprite
                };
            }

            public readonly Sprite GetIncreaseSprite(Slider.Direction direction)
            {
                return direction switch
                {
                    Direction.LeftToRight => rightSprite,
                    Direction.RightToLeft => leftSprite,
                    Direction.BottomToTop => upSprite,
                    Direction.TopToBottom => downSprite,
                    _ => rightSprite
                };
            }
        }

        [SerializeField] private float stepSize = 0.2f;
        [SerializeField] private RoButton decreaseButton;
        [SerializeField] private RoButton increaseButton;
        [SerializeField] private RectTransform dragArea;
        [SerializeField] private ButtonSprites buttonSprites;

        public SliderEvent onDecreaseClick;
        public SliderEvent onIncreaseClick;
        public SliderEvent onPointerUp;

        private bool dragStartedInDragArea;

        protected override void Start()
        {
            base.Start();

            if (!EnsureReferences()) return;
        }

        public bool EnsureReferences()
        {
            if (decreaseButton == null)
            {
                Tools.LogMissingReference(this, nameof(decreaseButton));
            }
            if (increaseButton == null)
            {
                Tools.LogMissingReference(this, nameof(increaseButton));
            }
            if (dragArea == null)
            {
                Tools.LogMissingReference(this, nameof(dragArea));
            }
            return true;
        }

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

            dragStartedInDragArea = false;

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
            value -= GetStepSize();
            onDecreaseClick?.Invoke(value);
        }

        private void OnIncreaseClick()
        {
            value += GetStepSize();
            onIncreaseClick?.Invoke(value);
        }

        private float GetStepSize()
        {
            return maxValue * stepSize;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            dragStartedInDragArea = MayDrag(eventData) && IsPointerInsideDragArea(eventData);

            if (!dragStartedInDragArea)
                return;

            base.OnPointerDown(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (!dragStartedInDragArea)
                return;

            base.OnDrag(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!dragStartedInDragArea)
                return;

            dragStartedInDragArea = false;

            base.OnPointerUp(eventData);

            if (MayDrag(eventData))
            {
                Debug.Log("OnPointerUp");
            }
        }

        private bool MayDrag(PointerEventData eventData)
        {
            if (IsActive() && IsInteractable())
            {
                return eventData.button == PointerEventData.InputButton.Left;
            }

            return false;
        }

        private bool IsPointerInsideDragArea(PointerEventData eventData)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(
                       dragArea,
                       eventData.position,
                       eventData.pressEventCamera);
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