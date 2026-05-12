using UnityEngine;
using UnityEngine.UI;
using UnitySlider = UnityEngine.UI.Slider;

namespace RO_Flex_UI.Components
{
    public class ROSlider : MonoBehaviour
    {
        [SerializeField] private Button decreaseButton;
        [SerializeField] private UnitySlider slider;
        [SerializeField] private Button increaseButton;
        private UnitySlider.Direction direction = UnitySlider.Direction.LeftToRight;
        [HideInInspector][SerializeField] private float stepPercent = 0.1f; // 10%
        [HideInInspector][SerializeField] private UnitySlider.SliderEvent onValueChanged = new();

        private void Awake()
        {
            BindEvents();
            SyncExternalEvent();
        }

        private void OnDestroy()
        {
            UnbindEvents();
        }

        private void OnValidate()
        {
            if (slider == null)
                return;

            stepPercent = Mathf.Clamp(stepPercent, 0.01f, 1f);
        }

        private void ApplyOrientation()
        {
            if (slider == null)
                return;

            var rect = transform as RectTransform;

            switch (direction)
            {
                case UnitySlider.Direction.LeftToRight:
                    {
                        slider.direction = UnitySlider.Direction.LeftToRight;
                        rect.localRotation = Quaternion.identity;
                        break;
                    }

                case UnitySlider.Direction.RightToLeft:
                    {
                        slider.direction = UnitySlider.Direction.RightToLeft;
                        rect.localRotation = Quaternion.identity;
                        break;
                    }

                case UnitySlider.Direction.BottomToTop:
                    {
                        slider.direction = UnitySlider.Direction.LeftToRight;
                        rect.localRotation = Quaternion.Euler(0f, 0f, 90f);
                        break;
                    }

                case UnitySlider.Direction.TopToBottom:
                    {
                        slider.direction = UnitySlider.Direction.RightToLeft;
                        rect.localRotation = Quaternion.Euler(0f, 0f, 90f);
                        break;
                    }
            }
        }

        private void BindEvents()
        {
            if (decreaseButton != null)
                decreaseButton.onClick.AddListener(OnDecreaseClicked);

            if (increaseButton != null)
                increaseButton.onClick.AddListener(OnIncreaseClicked);

            if (slider != null)
                slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void UnbindEvents()
        {
            if (decreaseButton != null)
                decreaseButton.onClick.RemoveListener(OnDecreaseClicked);

            if (increaseButton != null)
                increaseButton.onClick.RemoveListener(OnIncreaseClicked);

            if (slider != null)
                slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            onValueChanged.Invoke(value);
        }

        private void SyncExternalEvent()
        {
            if (slider == null)
                return;

            onValueChanged.Invoke(slider.value);
        }

        private void OnDecreaseClicked()
        {
            ChangeBy(-GetSignedStep());
        }

        private void OnIncreaseClicked()
        {
            ChangeBy(GetSignedStep());
        }

        /// <summary>
        /// If slider is RightToLeft, visual increase means lower numeric value.
        /// So buttons automatically invert behavior.
        /// </summary>
        private float GetSignedStep()
        {
            float step = (slider.maxValue - slider.minValue) * stepPercent;

            if (slider.direction == UnitySlider.Direction.RightToLeft)
                step *= -1f;

            return step;
        }

        private void ChangeBy(float delta)
        {
            SetValue(slider.value + delta);
        }

        public void SetValue(float value)
        {
            if (slider == null)
                return;

            value = Mathf.Clamp(value, slider.minValue, slider.maxValue);

            if (slider.wholeNumbers)
                value = Mathf.Round(value);

            slider.value = value;
        }

        public float GetValue()
        {
            return slider != null ? slider.value : 0f;
        }

        public void SetRange(float min, float max)
        {
            if (slider == null)
                return;

            slider.minValue = min;
            slider.maxValue = max;

            SetValue(slider.value);
        }

        public void SetInteractable(bool interactable)
        {
            if (slider != null)
                slider.interactable = interactable;

            if (decreaseButton != null)
                decreaseButton.interactable = interactable;

            if (increaseButton != null)
                increaseButton.interactable = interactable;
        }

        public void SetDirection(UnitySlider.Direction direction)
        {
            if (slider == null)
                return;

            slider.direction = direction;
        }

        #region Proxy Properties

        public float Value
        {
            get => slider.value;
            set => SetValue(value);
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

        public UnitySlider.Direction Direction
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
            set => slider.interactable = value;
        }

        public void SetValueWithoutNotify(float value)
        {
            if (slider == null)
                return;

            slider.SetValueWithoutNotify(value);
        }

        #endregion
    }
}