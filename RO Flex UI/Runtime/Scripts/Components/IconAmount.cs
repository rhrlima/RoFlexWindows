using RO_Flex_UI.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    public class IconAmount : MonoBehaviour, IComponent
    {
        [SerializeField] private Image iconSprite;
        [SerializeField] private TextMeshProUGUI iconText;

        private void Start()
        {
            if (!EnsureReferences()) return;
        }

        public bool EnsureReferences()
        {
            if (iconSprite == null)
            {
                Tools.LogMissingReference(this, nameof(iconSprite));
                return false;
            }
            if (iconText == null)
            {
                Tools.LogMissingReference(this, nameof(iconText));
                return false;
            }

            return true;
        }
        public void ToggleText(bool active)
        {
            iconText.gameObject.SetActive(active);
        }
        public void SetActive(bool value)
        {
            iconSprite.gameObject.SetActive(value);
            iconText.gameObject.SetActive(value);
        }

        #region Getter & Setter
        public Sprite Sprite
        {
            get => iconSprite.sprite;
            set => iconSprite.sprite = value;
        }
        public string Text
        {
            get => iconText.text;
            set => iconText.text = value;
        }
        #endregion
    }
}