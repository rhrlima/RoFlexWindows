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

        private bool hasPresentationState;
        private bool hasContent;
        private bool showAmount;
        private bool visible = true;

        public bool IsVisible => isActiveAndEnabled
            && gameObject.activeInHierarchy
            && visible
            && (!hasPresentationState || hasContent);

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
            if (!EnsureReferences())
                return;

            if (sprite == null || amount <= 0)
            {
                Clear();
                return;
            }

            hasPresentationState = true;
            hasContent = true;
            showAmount = amount > 1;
            iconSprite.sprite = sprite;
            iconText.text = amount.ToString(CultureInfo.InvariantCulture);
            RefreshVisibility();
        }

        public void Clear()
        {
            if (!EnsureReferences())
                return;

            hasPresentationState = true;
            hasContent = false;
            showAmount = false;
            iconSprite.sprite = null;
            iconText.text = string.Empty;
            RefreshVisibility();
        }

        public void SetActive(bool value)
        {
            visible = value;
            RefreshVisibility();
        }

        private void RefreshVisibility()
        {
            if (!EnsureReferences())
                return;

            if (!hasPresentationState)
            {
                iconSprite.gameObject.SetActive(visible);
                iconText.gameObject.SetActive(visible);
                return;
            }

            iconSprite.gameObject.SetActive(visible && hasContent);
            iconText.gameObject.SetActive(visible && hasContent && showAmount);
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