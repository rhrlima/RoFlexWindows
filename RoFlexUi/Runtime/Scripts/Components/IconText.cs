using RO_Flex_UI.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    [RequireComponent(typeof(HorizontalOrVerticalLayoutGroup))]
    public class IconText : IconAmount
    {
        [SerializeField] private TextMeshProUGUI iconText;
        [SerializeField] private bool disableText = false;
        [SerializeField] private bool flipElements = false;
        private HorizontalOrVerticalLayoutGroup layoutGroup;

        protected override void Start()
        {
            base.Start();
        }

        public override bool EnsureReferences()
        {
            layoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();

            if (iconText == null)
            {
                Tools.LogMissingReference(this, nameof(iconText));
                return false;
            }

            if (layoutGroup == null)
            {
                Tools.LogMissingReference(this, nameof(layoutGroup));
                return false;
            }

            return base.EnsureReferences();
        }

        public void FlipElements(bool flip)
        {
            if (!EnsureReferences()) return;

            layoutGroup.reverseArrangement = flip;
        }

        public void ToggleText(bool active)
        {
            disableText = !active;
            if (iconText == null) return;
            iconText.gameObject.SetActive(active);
        }

        public override void SetActive(bool value)
        {
            base.SetActive(value);

            disableText = !value;
            ToggleText(value);
        }

        public void Assign(Sprite sprite, string text, string amount = "")
        {
            base.Assign(sprite, amount);

            if (!EnsureReferences()) return;

            iconText.text = text ?? string.Empty;

            if (string.IsNullOrEmpty(amount))
                disableAmount = true;
        }

        public override DragPresentation CapturePresentation()
        {
            return new DragPresentation(Sprite, Amount, Text);
        }

        public override bool TryApplyPresentation(DragPresentation presentation)
        {
            if (!EnsureReferences()) return false;

            Assign(presentation.Sprite, presentation.Text, presentation.Amount);
            return true;
        }

        public override void Clear()
        {
            base.Clear();
            if (iconText != null)
                iconText.text = string.Empty;
        }

        protected override void OnValidate()
        {
            layoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
            if (iconText == null || layoutGroup == null || iconSprite == null || iconAmount == null)
                return;

            if (!EnsureReferences()) return;

            base.OnValidate();
            ToggleText(visible && !disableText);
            layoutGroup.reverseArrangement = flipElements;
        }

        #region Getter & Setter
        public string Text => iconText != null ? iconText.text : string.Empty;
        public override bool IsVisible => visible && (!disableIcon || !disableAmount || !disableText);
        #endregion
    }
}
