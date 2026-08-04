using RO_Flex_UI.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    [ExecuteAlways]
    public class IconAmount : MonoBehaviour, IComponent, IDragVisual
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

<<<<<<< HEAD:RoFlexUi/Runtime/Scripts/Components/IconAmount.cs
            ToggleIcon(visible && !disableIcon);
            ToggleAmount(visible && !disableAmount);
=======
            ToggleIcon(value);
            ToggleAmount(value);
>>>>>>> 4b59f84 (refac: Rework draggable):RO Flex UI/Runtime/Scripts/Components/IconAmount.cs
        }

        public virtual void Assign(Sprite sprite, string amount)
        {
            if (!EnsureReferences()) return;

            iconSprite.sprite = sprite;
            iconAmount.text = amount ?? string.Empty;
<<<<<<< HEAD:RoFlexUi/Runtime/Scripts/Components/IconAmount.cs
=======
        }

        public virtual DragPresentation CapturePresentation()
        {
            return new DragPresentation(Sprite, Amount);
        }

        public virtual bool TryApplyPresentation(DragPresentation presentation)
        {
            if (!EnsureReferences()) return false;

            Assign(presentation.Sprite, presentation.Amount);
            return true;
>>>>>>> 4b59f84 (refac: Rework draggable):RO Flex UI/Runtime/Scripts/Components/IconAmount.cs
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
<<<<<<< HEAD:RoFlexUi/Runtime/Scripts/Components/IconAmount.cs
=======
        public RectTransform RectTransform => transform as RectTransform;
>>>>>>> 4b59f84 (refac: Rework draggable):RO Flex UI/Runtime/Scripts/Components/IconAmount.cs
        public Sprite Sprite => iconSprite != null ? iconSprite.sprite : null;
        public string Amount => iconAmount != null ? iconAmount.text : string.Empty;
        public virtual bool IsVisible => visible && (!disableIcon || !disableAmount);
        public bool Empty => Sprite == null && string.IsNullOrEmpty(Amount);
        #endregion
    }
}
