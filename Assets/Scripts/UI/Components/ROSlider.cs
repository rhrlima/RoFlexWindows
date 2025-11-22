using System;
using UnityEngine;
using UnityEngine.UI;

public class ROSlider : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>(true);
        }
    }

    public void AddByValue(float amount)
    {
        if (slider == null) return;
        slider.value += amount;
    }

    public void AddByPercent(float percent)
    {
        if (slider == null) return;
        float increaseAmount = (slider.maxValue - slider.minValue) * (percent / 100f);
        slider.value += increaseAmount;
    }
}
