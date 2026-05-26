using UnityEngine;

namespace RO_Flex_UI.Config
{
    [DefaultExecutionOrder(-1000)]
    public sealed class UIConfigProvider : MonoBehaviour
    {
        public static UIConfigProvider Instance { get; private set; }

        [SerializeField] private GlobalUiConfig activeConfig;

        public GlobalUiConfig ActiveConfig => activeConfig;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        public Sprite GetSprite(UISpriteID id)
        {
            if (activeConfig == null)
            {
                Debug.LogWarning($"UiConfigProvider has no active GlobalUiConfig. Cannot resolve sprite '{id}'.", this);
                return null;
            }

            return activeConfig.GetSprite(id);
        }
    }
}