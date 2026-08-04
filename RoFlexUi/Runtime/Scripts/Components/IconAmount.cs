using RO_Flex_UI.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    [ExecuteAlways]
    public class IconAmount : MonoBehaviour, IComponent
    {
        [SerializeField] protected Image iconSprite;
        [SerializeField] protected TextMeshProUGUI iconAmount;
        [SerializeField] protected bool disableIcon = false;
        [SerializeField] protected bool disableAmount = false;
        [SerializeField] protected bool visible = true;

        protected virtual void Start()
        {
            if (!EnsureReferences()) return;
        }

        public virtual bool EnsureReferences()
        {
            if (iconSprite == null)
            {
                Tools.LogMissingReference(this, nameof(iconSprite));
                return false;
            }

            if (iconAmount == null)
            {
                Tools.LogMissingReference(this, nameof(iconAmount));
                return false;
            }

            return true;
        }

        public void ToggleAmount(bool active)
        {
            if (iconAmount == null) return;
            iconAmount.gameObject.SetActive(active);
        }

        public void ToggleIcon(bool active)
        {
            if (iconSprite == null) return;
            iconSprite.gameObject.SetActive(active);
        }

        public virtual void SetActive(bool value)
        {
            disableAmount = !value;
            disableIcon = !value;
            visible = value;

            ToggleIcon(visible && !disableIcon);
            ToggleAmount(visible && !disableAmount);
        }

        public virtual void Assign(Sprite sprite, string amount)
        {
            if (!EnsureReferences()) return;

            iconSprite.sprite = sprite;
            iconAmount.text = amount ?? string.Empty;
        }

        public virtual void Clear()
        {
            if (!EnsureReferences()) return;

            iconSprite.sprite = null;
            iconAmount.text = string.Empty;
            visible = false;

            SetActive(false);
        }

        protected virtual void OnValidate()
        {
            SetActive(visible);
        }

        #region Getter & Setter
        public Sprite Sprite => iconSprite != null ? iconSprite.sprite : null;
        public string Amount => iconAmount != null ? iconAmount.text : string.Empty;
        public virtual bool IsVisible => visible && (!disableIcon || !disableAmount);
        public bool Empty => Sprite == null && string.IsNullOrEmpty(Amount);
        #endregion
    }
}