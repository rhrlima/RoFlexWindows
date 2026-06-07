using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RO_Flex_UI.Config
{
    public enum UISpriteID
    {
        None,
        Background,
        Button,
    }

    [Serializable]
    public sealed class UISPriteEntry
    {
        public UISpriteID id;
        public Sprite sprite;
    }

    [CreateAssetMenu(menuName = "Tools/RO Flex UI/Global UI Config", fileName = "GlobalUiConfig")]
    public sealed class GlobalUiConfig : ScriptableObject
    {
        [Header("Font Configuration")]
        [SerializeField] private TMP_FontAsset defaultFont;

        [Header("Fallback")]
        [SerializeField] private Sprite placeholderSprite;

        [Header("Sprite Registry")]
        [SerializeField] private List<UISPriteEntry> spriteEntries = new();

        private readonly Dictionary<UISpriteID, Sprite> _spriteMap = new();
        private bool _isCacheBuilt;

        public Sprite PlaceholderSprite => placeholderSprite;

        private void OnEnable()
        {
            BuildCache();
        }

        private void OnValidate()
        {
            BuildCache();
        }

        private void BuildCache()
        {
            _spriteMap.Clear();

            foreach (var entry in spriteEntries)
            {
                if (entry == null || entry.id == UISpriteID.None || entry.sprite == null)
                    continue;

                if (_spriteMap.ContainsKey(entry.id))
                {
                    Debug.LogWarning($"Duplicate sprite ID '{entry.id}' in GlobalUiConfig '{name}'. Last one wins.", this);
                }

                _spriteMap[entry.id] = entry.sprite;
            }

            _isCacheBuilt = true;
        }

        public Sprite GetSprite(UISpriteID id)
        {
            if (!_isCacheBuilt)
                BuildCache();

            if (id != UISpriteID.None && _spriteMap.TryGetValue(id, out var sprite) && sprite != null)
                return sprite;

            if (placeholderSprite != null)
            {
                Debug.LogWarning($"Sprite ID '{id}' not found in GlobalUiConfig '{name}'. Using placeholder.", this);
                return placeholderSprite;
            }

            Debug.LogWarning($"Sprite ID '{id}' not found in GlobalUiConfig '{name}', and no placeholder is assigned.", this);
            return null;
        }

        public TMP_FontAsset GetDefaultFont()
        {
            return defaultFont;
        }
    }
}