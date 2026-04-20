using UnityEngine;
using UnityEngine.UI;

namespace Flex.UI
{
    /// <summary>
    /// Wrapper component for a Slider with decrease/increase buttons.
    ///
    /// Expected hierarchy:
    /// ROSlider
    /// ├── BtnDecrease
    /// ├── Slider
    /// └── BtnIncrease
    ///
    /// Attach this script to MySlider.
    /// </summary>
    public class ROSlider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button decreaseButton;
        [SerializeField] private Slider slider;
        [SerializeField] private Button increaseButton;

        [Header("Behavior")]
        [Range(0.01f, 1f)]
        [SerializeField] private float stepPercent = 0.1f; // 10%

        [Header("Events")]
        public Slider.SliderEvent onValueChanged = new Slider.SliderEvent();

        private void Awake()
        {
            BindEvents();
            RefreshButtonMeaning();
            SyncExternalEvent();
        }

        private void OnDestroy()
        {
            UnbindEvents();
        }

        private void OnValidate()
        {
            stepPercent = Mathf.Clamp(stepPercent, 0.01f, 1f);

            if (slider != null)
                RefreshButtonMeaning();
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

            if (slider.direction == Slider.Direction.RightToLeft)
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

        public void SetDirection(Slider.Direction direction)
        {
            if (slider == null)
                return;

            slider.direction = direction;
            RefreshButtonMeaning();
        }

        private void RefreshButtonMeaning()
        {
            // Optional:
            // swap button visual positions if using RTL layout.
            // Here we only invert behavior, not hierarchy positions.
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

        public void SetValueWithoutNotify(float value)
        {
            if (slider != null)
                slider.SetValueWithoutNotify(value);
        }

        #endregion
    }
}