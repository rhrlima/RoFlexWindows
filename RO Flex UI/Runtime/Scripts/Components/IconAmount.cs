using RO_Flex_UI.Utils;
using System.ComponentModel;
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
            if (!Tools.IsValid(this, iconSprite)) return false;
            if (!Tools.IsValid(this, iconAmount)) return false;
            return true;
        }

        public void ToggleAmount(bool active)
        {
            // disableAmount = !active;
            iconAmount.gameObject.SetActive(active);
        }

        public void ToggleIcon(bool active)
        {
            // disableIcon = !active;
            iconSprite.gameObject.SetActive(active);
        }

        public virtual void SetActive(bool value)
        {
            ToggleIcon(value);
            ToggleAmount(value);

            disableAmount = !value;
            disableIcon = !value;
            visible = value;
        }

        public virtual void Assign(Sprite sprite, string amount)
        {
            if (!EnsureReferences()) return;

            iconSprite.sprite = sprite;
            iconAmount.text = amount.ToString();
        }

        public virtual void Clear()
        {
            if (!EnsureReferences()) return;

            iconSprite.sprite = null;
            iconAmount.text = string.Empty;
        }

        protected virtual void OnValidate()
        {
            ToggleIcon(visible && !disableIcon);
            ToggleAmount(visible && !disableAmount);
        }

        #region Getter & Setter
        public Sprite Sprite => iconSprite.sprite;
        public string Amount => iconAmount.text;
        public bool IsVisible => visible && (!disableIcon || !disableAmount);
        #endregion
    }
}