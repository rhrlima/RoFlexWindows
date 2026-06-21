using RO_Flex_UI.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO add drag-n-drop
namespace RO_Flex_UI.Components
{
    [RequireComponent(typeof(HorizontalOrVerticalLayoutGroup))]
    public class IconText : MonoBehaviour, IComponent
    {
        [SerializeField] private TextMeshProUGUI iconText;
        [SerializeField] private Image iconSprite;
        private HorizontalOrVerticalLayoutGroup layoutGroup;

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

            layoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
            if (layoutGroup == null)
            {
                Tools.LogMissingReference(this, nameof(layoutGroup));
                return false;
            }

            return true;
        }

        public void FlipElements(bool flip)
        {
            if (!EnsureReferences()) return;

            layoutGroup.reverseArrangement = flip;
        }

        #region Getter & Setter

        public string Text
        {
            get => iconText.text;
            set => iconText.text = value;
        }

        public Sprite Sprite
        {
            get => iconSprite.sprite;
            set => iconSprite.sprite = value;
        }

        #endregion
    }
}