using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    public class ROSlider : MonoBehaviour
    {
        [SerializeField] private Button decreaseButton;
        [SerializeField] private Slider slider;
        [SerializeField] private Button increaseButton;
        private EventTrigger.Entry pointerUpEntry;
        [HideInInspector][SerializeField] private Slider.Direction direction = Slider.Direction.LeftToRight;
        [HideInInspector][SerializeField] private float stepPercent = 0.1f;
        [HideInInspector][SerializeField] private Slider.SliderEvent onValueChanged = new Slider.SliderEvent();
        [FormerlySerializedAs("onEndDrag")]
        [HideInInspector][SerializeField] private Slider.SliderEvent onPointerUp = new Slider.SliderEvent();

        private void Awake()
        {
            if (!EnsureReferences())
                return;

            ApplyOrientation();
        }

        private void OnEnable()
        {
            if (!EnsureReferences())
                return;

            decreaseButton.onClick.AddListener(OnDecreaseClicked);
            increaseButton.onClick.AddListener(OnIncreaseClicked);

            slider.onValueChanged.AddListener(OnSliderValueChanged);
            pointerUpEntry.callback.AddListener(OnSliderPointerUp);
        }

        private void OnDisable()
        {
            decreaseButton.onClick.RemoveListener(OnDecreaseClicked);
            increaseButton.onClick.RemoveListener(OnIncreaseClicked);

            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            pointerUpEntry.callback.RemoveListener(OnSliderPointerUp);
        }

        private void OnValidate()
        {
            if (slider == null)
                return;

            stepPercent = Mathf.Clamp(stepPercent, 0.01f, 1f);

            if (MaxValue < MinValue)
                MaxValue = MinValue;

            Value = Mathf.Clamp(Value, MinValue, MaxValue);
        }

        public bool EnsureReferences()
        {
            if (slider == null)
            {
                Debug.LogError($"[{name}] Missing Slider as a child component.");
                return false;
            }

            if (decreaseButton == null)
            {
                Debug.LogError($"[{name}] Missing Decrease Button as a child component.");
                return false;
            }

            if (increaseButton == null)
            {
                Debug.LogError($"[{name}] Missing Increase Button as a child component.");
                return false;
            }

            var trigger = slider.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                Debug.LogError($"[{name}] Missing EventTrigger as a component of child Slider.");
                return false;
            }

            pointerUpEntry = trigger.triggers.Find(entry => entry.eventID == EventTriggerType.PointerUp);
            if (pointerUpEntry == null)
            {
                Debug.LogError($"[{name}] Missing EventTrigger.PointerUp in child Slider component.");
                return false;
            }

            return true;
        }

        private void ApplyOrientation()
        {
            if (slider == null)
                return;

            var rect = transform as RectTransform;

            switch (direction)
            {
                case Slider.Direction.LeftToRight:
                    {
                        slider.direction = Slider.Direction.LeftToRight;
                        rect.localRotation = Quaternion.identity;
                        break;
                    }

                case Slider.Direction.RightToLeft:
                    {
                        slider.direction = Slider.Direction.RightToLeft;
                        rect.localRotation = Quaternion.identity;
                        break;
                    }

                case Slider.Direction.BottomToTop:
                    {
                        slider.direction = Slider.Direction.LeftToRight;
                        rect.localRotation = Quaternion.Euler(0f, 0f, 90f);
                        break;
                    }

                case Slider.Direction.TopToBottom:
                    {
                        slider.direction = Slider.Direction.RightToLeft;
                        rect.localRotation = Quaternion.Euler(0f, 0f, 90f);
                        break;
                    }
            }
        }

        private void OnDecreaseClicked()
        {
            Value += -GetSignedStep();

            // triggers the pointer-up event
            OnSliderPointerUp(null);
        }

        private void OnIncreaseClicked()
        {
            Value += GetSignedStep();

            // triggers the pointer-up event
            OnSliderPointerUp(null);
        }

        /// <summary>
        /// If slider is RightToLeft, visual increase means lower numeric value.
        /// So buttons automatically invert behavior.
        /// </summary>
        private float GetSignedStep()
        {
            var range = MaxValue - MinValue;
            var step = range * stepPercent;

            if (slider.wholeNumbers)
            {
                step = Mathf.Round(step);
                step = Mathf.Clamp(step, 1f, range);
            }

            if (slider.direction == Slider.Direction.RightToLeft)
                step *= -1f;

            return step;
        }

        private void OnSliderValueChanged(float value)
        {
            onValueChanged?.Invoke(value);
        }

        private void OnSliderPointerUp(BaseEventData data)
        {
            onPointerUp?.Invoke(slider.value);
        }

        #region Proxy Properties

        public float Value
        {
            get => slider.value;
            set => slider.value = value;
        }

        public float MinValue
        {
            get => slider.minValue;
            set => slider.minValue = value;
        }

        public float MaxValue
        {
            get => slider.maxValue;
            set => slider.maxValue = value;
        }

        public bool WholeNumbers
        {
            get => slider.wholeNumbers;
            set => slider.wholeNumbers = value;
        }

        public float StepPercent
        {
            get => stepPercent;
            set => stepPercent = value;
        }

        public Slider.Direction Direction
        {
            get => direction;
            set
            {
                direction = value;
                ApplyOrientation();
            }
        }

        public bool Interactable
        {
            get => slider.interactable;
            set
            {
                slider.interactable = value;
                decreaseButton.interactable = value;
                increaseButton.interactable = value;
            }
        }

        public void SetValueWithoutNotify(float value)
        {
            if (slider == null)
                return;

            slider.SetValueWithoutNotify(value);
        }

        public Slider.SliderEvent OnValueChanged
        {
            get => onValueChanged;
            set => onValueChanged = value;
        }

        public Slider.SliderEvent OnPointerUp
        {
            get => onPointerUp;
            set => onPointerUp = value;
        }

        #endregion
    }
}