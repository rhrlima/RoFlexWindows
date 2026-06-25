using RO_Flex_UI.Utils;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    public class IconAmount : MonoBehaviour, IComponent
    {
        [SerializeField] private Image iconSprite;
        [SerializeField] private TextMeshProUGUI iconText;
        [SerializeField] private bool visible;

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

        public void Assign(Sprite sprite, int amount)
        {
            if (!EnsureReferences()) return;

            if (sprite == null || amount <= 0)
            {
                Clear();
                return;
            }

            iconSprite.sprite = sprite;
            iconText.text = amount.ToString();
        }

        public void Clear()
        {
            if (!EnsureReferences())
                return;

            iconSprite.sprite = null;
            iconText.text = string.Empty;
        }

        public void SetActive(bool value)
        {
            visible = value;
            this.gameObject.SetActive(value);
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
        public bool IsVisible => visible;
        #endregion
    }
}