using RO_Flex_UI.Components;
using RO_Flex_UI.Config;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Panels
{
    public class UIConfigPanel : IPanel
    {
        [SerializeField] GlobalUIConfigData configData;
        [SerializeField] TMP_Dropdown dropTheme;
        [SerializeField] ROSlider sliderScale;

        private void Start()
        {
            dropTheme.ClearOptions();
            foreach (var theme in Enum.GetValues(typeof(GlobalUIConfigData.UITheme)))
            {
                dropTheme.options.Add(new TMP_Dropdown.OptionData() { text = theme.ToString() });
            }
            dropTheme.onValueChanged.AddListener(ThemeChanged);

            sliderScale.WholeNumbers = true;
            sliderScale.MinValue = 1;
            sliderScale.MaxValue = 4;
            sliderScale.StepPercent = 0.25f;
            sliderScale.OnPointerUp.AddListener(ScaleChanged);
        }

        public void ThemeChanged(int newTheme)
        {
            configData.SetTheme((GlobalUIConfigData.UITheme)newTheme);
        }

        public void ScaleChanged(float newValue)
        {
            configData.SetScale((int)newValue);
        }
    }
}