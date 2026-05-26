using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Config
{
    [RequireComponent(typeof(Image))]
    public sealed class GlobalImageBinder : MonoBehaviour
    {
        [SerializeField] private UISpriteID spriteId;
        [SerializeField] private bool setNativeSize;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            Apply();
        }

        [ContextMenu("Apply")]
        public void Apply()
        {
            if (_image == null)
                _image = GetComponent<Image>();

            if (UIConfigProvider.Instance == null)
            {
                Debug.LogWarning($"No UiConfigProvider found. Cannot apply sprite ID '{spriteId}'.", this);
                return;
            }

            var sprite = UIConfigProvider.Instance.GetSprite(spriteId);
            if (sprite == null)
                return;

            _image.sprite = sprite;

            if (setNativeSize)
                _image.SetNativeSize();
        }

        public void SetSpriteId(UISpriteID id)
        {
            spriteId = id;
            Apply();
        }
    }
}