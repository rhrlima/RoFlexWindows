using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    public class ToggleSwitch : Slider, IPointerClickHandler
    {
        [Serializable]
        public class ToggleEvent : UnityEvent<bool> { }

        [Header("Animation")]
        [SerializeField, Min(0f)] private float animationDuration = 0.2f;
        [SerializeField] private AnimationCurve slideEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Events")]
        public ToggleEvent onToggle = new();
        public UnityEvent onToggleOn = new();
        public UnityEvent onToggleOff = new();

        private Coroutine animateSliderCoroutine;
        private bool committedState;
        private bool targetState;

        public bool IsOn => Mathf.Approximately(value, maxValue);

        protected override void Awake()
        {
            base.Awake();

            SetupToggleSemantics();
            SyncStateFromValue();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            SetupToggleSemantics();
            SyncStateFromValue();
        }

        protected override void OnDisable()
        {
            if (animateSliderCoroutine != null)
            {
                StopCoroutine(animateSliderCoroutine);
                animateSliderCoroutine = null;
            }

            base.OnDisable();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            SetupToggleSemantics();
            value = Mathf.Clamp01(value);
            SyncStateFromValue();
        }

        public void SetIsOn(bool isOn, bool notify = true, bool animated = true)
        {
            var previousState = committedState;
            targetState = isOn;

            if (animateSliderCoroutine != null)
            {
                StopCoroutine(animateSliderCoroutine);
                animateSliderCoroutine = null;
            }

            if (!animated || animationDuration <= 0f || !isActiveAndEnabled)
            {
                SetValueWithoutNotify(GetValueForState(isOn));
                CompleteToggle(previousState, isOn, notify);
                return;
            }

            animateSliderCoroutine = StartCoroutine(AnimateSlider(previousState, isOn, notify));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!MayToggle(eventData))
                return;

            if (animateSliderCoroutine == null)
                SyncStateFromValue();

            SetIsOn(!targetState);
        }

        public override void OnDrag(PointerEventData eventData)
        {
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
        }

        public override void OnMove(AxisEventData eventData)
        {
        }

        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (eventData != null)
                eventData.useDragThreshold = true;
        }

        private IEnumerator AnimateSlider(bool previousState, bool isOn, bool notify)
        {
            var startValue = value;
            var endValue = GetValueForState(isOn);
            var elapsed = 0f;

            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                var normalizedTime = Mathf.Clamp01(elapsed / animationDuration);
                var easedTime = slideEase != null ? slideEase.Evaluate(normalizedTime) : normalizedTime;
                SetValueWithoutNotify(Mathf.Lerp(startValue, endValue, easedTime));

                yield return null;
            }

            SetValueWithoutNotify(endValue);
            animateSliderCoroutine = null;
            CompleteToggle(previousState, isOn, notify);
        }

        private void CompleteToggle(bool previousState, bool isOn, bool notify)
        {
            committedState = isOn;
            targetState = isOn;

            if (!notify || previousState == isOn)
                return;

            onToggle?.Invoke(isOn);

            if (isOn)
                onToggleOn?.Invoke();
            else
                onToggleOff?.Invoke();
        }

        private void SyncStateFromValue()
        {
            committedState = IsOn;
            targetState = committedState;
        }

        private void SetupToggleSemantics()
        {
            minValue = 0f;
            maxValue = 1f;
            wholeNumbers = false;
        }

        private bool MayToggle(PointerEventData eventData)
        {
            return IsActive()
                && IsInteractable()
                && eventData != null
                && eventData.button == PointerEventData.InputButton.Left;
        }

        private static float GetValueForState(bool isOn)
        {
            return isOn ? 1f : 0f;
        }
    }
}