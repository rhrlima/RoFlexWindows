using System;
using UnityEngine;

namespace RO_Flex_UI.Config
{
    [CreateAssetMenu(menuName = "RO Flex UI/Data/Global UI Config", fileName = "GlobalUIConfigData")]
    public class GlobalUIConfigData : ScriptableObject
    {
        public enum UITheme
        {
            MODERN,
            CLASSIC
        }

        public UITheme uiTheme;
        [Range(1, 4)]
        public int uiScale;

        public event Action<GlobalUIConfigData> OnConfigChanged;

        public void SetTheme(UITheme newTheme)
        {
            uiTheme = newTheme;

            OnConfigChanged?.Invoke(this);
        }

        public void SetScale(int newScale)
        {
            uiScale = Math.Clamp(newScale, 1, 4);

            OnConfigChanged?.Invoke(this);
        }
    }
}