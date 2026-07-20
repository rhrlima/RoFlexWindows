using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using System.IO;

namespace RO_Flex_UI.Editor
{
    public class FontAssetGenerator
    {
        // private const string fontsPath = "Assets/Samples/ROFlexUI";
        private const string fontsPath = "Assets/Fonts";

        [MenuItem("Tools/ROFlexUI/Regenerate TMP Fonts")]
        public static void GenerateAll()
        {
            var guids = AssetDatabase.FindAssets("t:Font", new[] { fontsPath });
            Debug.Log($"Found {guids.Length} font assets to process.");

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                Generate(path);
            }
        }

        public static void Generate(string fontPath)
        {
            var sourceFont = PrepareSourceFont(fontPath);

            if (sourceFont == null)
            {
                Debug.LogError($"Failed to load font at path: {fontPath}");
                return;
            }

            var assetPath = Path.ChangeExtension(fontPath, ".asset");

            var fontAsset = TMP_FontAsset.CreateFontAsset(
                sourceFont,
                16,
                5,
                GlyphRenderMode.RASTER_HINTED,
                256, 256,
                AtlasPopulationMode.Dynamic,
                true
            );

            if (fontAsset == null) return;

            fontAsset.hideFlags = HideFlags.None;
            fontAsset.atlasTexture.hideFlags = HideFlags.None;

            fontAsset.atlasTexture.name = sourceFont.name + " Atlas";
            fontAsset.atlasTexture.filterMode = FilterMode.Point;
            fontAsset.atlasTexture.wrapMode = TextureWrapMode.Clamp;

            AssetDatabase.CreateAsset(fontAsset, assetPath);
            AssetDatabase.AddObjectToAsset(fontAsset.atlasTexture, fontAsset);

            CreateMaterialPreset(fontAsset, assetPath, "Default", "TextMeshPro/Bitmap", true);
            CreateMaterialPreset(fontAsset, assetPath, "Outline", "ROFlexUI/Fonts/Pixel Outline");
            CreateMaterialPreset(fontAsset, assetPath, "Shadow", "ROFlexUI/Fonts/Pixel Shadow");

            EditorUtility.SetDirty(fontAsset);
            EditorUtility.SetDirty(fontAsset.atlasTexture);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Successfully generated asset at {assetPath}");
        }

        private static Font PrepareSourceFont(string fontPath)
        {
            var importer = AssetImporter.GetAtPath(fontPath) as TrueTypeFontImporter;

            if (importer == null)
            {
                Debug.LogError($"Could not find font importer for: {fontPath}");
                return null;
            }

            var requiresReimport = false;

            if (!importer.includeFontData)
            {
                importer.includeFontData = true;
                requiresReimport = true;
            }

            if (importer.fontTextureCase != FontTextureCase.Dynamic)
            {
                importer.fontTextureCase = FontTextureCase.Dynamic;
                requiresReimport = true;
            }

            if (requiresReimport)
            {
                Debug.Log($"Updating import settings for: {fontPath}");
                importer.SaveAndReimport();
            }

            return AssetDatabase.LoadAssetAtPath<Font>(fontPath);
        }

        private static void CreateMaterialPreset(
            TMP_FontAsset fontAsset,
            string fontAssetPath,
            string suffix,
            string shaderName
        )
        {
            CreateMaterialPreset(fontAsset, fontAssetPath, suffix, shaderName, false);
        }

        private static void CreateMaterialPreset(
            TMP_FontAsset fontAsset,
            string fontAssetPath,
            string suffix,
            string shaderName,
            bool is_default
        )
        {
            var shader = Shader.Find(shaderName);

            if (shader == null)
            {
                Debug.LogWarning($"Shader not found: {shaderName}");
                return;
            }

            var folder = Path.GetDirectoryName(fontAssetPath);
            var materialPath = Path.Combine(folder, $"{fontAsset.name} {suffix}.mat").Replace("\\", "/");

            var baseMaterial = fontAsset.material;

            if (baseMaterial == null)
            {
                Debug.LogWarning($"Font asset has no base material: {fontAsset.name}");
                return;
            }

            var existingMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (existingMaterial != null)
            {
                if (is_default)
                    fontAsset.material = existingMaterial;

                return;
            }

            var material = new Material(baseMaterial)
            {
                name = $"{fontAsset.name} {suffix}",
                shader = shader
            };

            material.SetTexture(ShaderUtilities.ID_MainTex, fontAsset.atlasTexture);

            AssetDatabase.CreateAsset(material, materialPath);

            if (is_default)
                fontAsset.material = material;

            EditorUtility.SetDirty(material);

            Debug.Log($"Created TMP material preset: {materialPath}");
        }
    }
}