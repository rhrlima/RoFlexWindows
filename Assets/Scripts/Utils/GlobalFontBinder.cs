using TMPro;
using UnityEngine;

namespace RO_Flex_UI.Config
{
    [RequireComponent(typeof(TMP_Text))]
    public sealed class GlobalFontBinder : MonoBehaviour
    {
        private TMP_Text textComponent;
        public Material fontPreset;
        private void Awake()
        {
            textComponent = GetComponent<TMP_Text>();

            Debug.Log($"{textComponent.font.name} {textComponent.fontSharedMaterial.name}");
            textComponent.font = UIConfigProvider.Instance.GetFont();
            textComponent.fontSharedMaterial = fontPreset;
            Debug.Log($"{textComponent.font.name} {textComponent.fontSharedMaterial.name}");
        }
    }
}