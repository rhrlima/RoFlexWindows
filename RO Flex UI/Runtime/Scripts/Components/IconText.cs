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

            if (!Tools.IsValid(this, iconText)) return false;
            if (!Tools.IsValid(this, layoutGroup)) return false;
            return base.EnsureReferences(); ;
        }

        public void FlipElements(bool flip)
        {
            if (!EnsureReferences()) return;

            layoutGroup.reverseArrangement = flip;
        }

        public void ToggleText(bool active)
        {
            disableText = !active;
            iconText.gameObject.SetActive(active);
        }

        public override void SetActive(bool value)
        {
            base.SetActive(value);
            ToggleText(value);
        }

        public void Assign(Sprite sprite, string text, string amount = "")
        {
            base.Assign(sprite, amount);

            if (!EnsureReferences()) return;

            iconText.text = text;

            if (string.IsNullOrEmpty(amount))
                disableAmount = true;
        }

        public override void Clear()
        {
            base.Clear();
            iconText.text = string.Empty;
        }

        protected override void OnValidate()
        {
            if (!EnsureReferences()) return;

            base.OnValidate();
            ToggleText(visible && !disableText);
            layoutGroup.reverseArrangement = flipElements;
        }

        #region Getter & Setter
        public string Text => iconText.text;
        public new bool IsVisible => visible && (!disableIcon || !disableAmount || !disableText);
        #endregion
    }
}