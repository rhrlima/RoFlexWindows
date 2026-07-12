using TMPro;
using UnityEngine;

namespace RO_Flex_UI.Components
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasRenderer))]
    [ExecuteAlways]
    public class RoText : TextMeshProUGUI
    {
        private static readonly int OutlineColorId = Shader.PropertyToID("_OutlineColor");

        [SerializeField] private bool overrideOutlineColor;
        [SerializeField] private new Color outlineColor = Color.black;

        private Material outlineMaterial;
        private Material outlineSourceMaterial;

        public bool OverrideOutlineColor
        {
            get => overrideOutlineColor;
            set
            {
                if (overrideOutlineColor == value) return;
                overrideOutlineColor = value;
                ApplyOutlineColorOverride();
            }
        }

        public Color OutlineColor
        {
            get => outlineColor;
            set
            {
                if (outlineColor == value) return;
                outlineColor = value;
                ApplyOutlineColorOverride();
            }
        }

        public bool SupportsOutlineColor => fontSharedMaterial != null && fontSharedMaterial.HasProperty(OutlineColorId);

        protected override void OnDisable()
        {
            DestroyOutlineMaterial();
            base.OnDisable();
        }

        protected override void OnDestroy()
        {
            DestroyOutlineMaterial();
            base.OnDestroy();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            ApplyOutlineColorOverride();
        }

        public override Material GetModifiedMaterial(Material baseMaterial)
        {
            var material = base.GetModifiedMaterial(baseMaterial);
            if (!overrideOutlineColor || material == null || !material.HasProperty(OutlineColorId))
            {
                DestroyOutlineMaterial();
                return material;
            }

            var modifiedMaterial = GetOutlineMaterial(material);
            modifiedMaterial.SetColor(OutlineColorId, outlineColor);
            return modifiedMaterial;
        }

        public void ApplyOutlineColorOverride()
        {
            SetMaterialDirty();
        }

        public void ClearOutlineColorOverride()
        {
            overrideOutlineColor = false;
            ApplyOutlineColorOverride();
        }

        private Material GetOutlineMaterial(Material sourceMaterial)
        {
            if (outlineMaterial == null || outlineSourceMaterial != sourceMaterial)
            {
                DestroyOutlineMaterial();
                outlineSourceMaterial = sourceMaterial;
                outlineMaterial = CreateMaterialInstance(sourceMaterial);
                outlineMaterial.name = $"{sourceMaterial.name} (RoText Outline)";
                outlineMaterial.hideFlags = HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;
            }

            return outlineMaterial;
        }

        private void DestroyOutlineMaterial()
        {
            if (outlineMaterial == null)
                return;

            if (Application.isPlaying)
                Destroy(outlineMaterial);
            else
                DestroyImmediate(outlineMaterial);

            outlineMaterial = null;
            outlineSourceMaterial = null;
        }
    }
}
