using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
{
    [Header("Slider Setup")]
    [SerializeField, Range(0, 1f)] private float sliderValue;
    public bool currentValue { get; private set; }
    private bool previousValue;

    private Slider slider;

    [Header("Animation")]
    [SerializeField, Range(0, 1f)] private float animationDuration = 0.2f;
    [SerializeField] private AnimationCurve slideEase =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine animateSliderCoroutine;

    [Header("Events")]
    [SerializeField] private UnityEvent onToggleOn;
    [SerializeField] private UnityEvent onToggleOff;

    // private ToggleSwitchGroupManager toggleSwitchGroupManager;

    protected void OnValidate()
    {
        SetupToggleComponent();
        slider.value = sliderValue;
    }

    private void SetupToggleComponent()
    {
        if (slider != null) return;

        SetupSliderComponent();
    }

    private void SetupSliderComponent()
    {
        slider = GetComponent<Slider>();

        if (slider == null)
        {
            Debug.Log("No slider found!", this);
            return;
        }

        slider.interactable = false;
        slider.transition = Selectable.Transition.None;
    }

    private void Awake()
    {
        SetupToggleComponent();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    private void Toggle()
    {
        SetStateAndStartAnimation(!currentValue);
    }

    private void SetStateAndStartAnimation(bool value)
    {
        previousValue = currentValue;
        currentValue = value;

        if (previousValue != currentValue)
        {
            if (currentValue)
            {
                onToggleOn?.Invoke();
            }
            else
            {
                onToggleOff?.Invoke();
            }
        }

        animateSliderCoroutine = StartCoroutine(AnimateSlider());
    }

    private IEnumerator AnimateSlider()
    {
        float startValue = slider.value;
        float endValue = currentValue ? 1 : 0;

        float time = 0;
        if (animationDuration > 0)
        {
            while (time < animationDuration)
            {
                time += Time.deltaTime;
                float lerpFactor = slideEase.Evaluate(time / animationDuration);
                slider.value = Mathf.Lerp(startValue, endValue, lerpFactor);

                yield return null;
            }
        }

        slider.value = endValue;
    }
}
