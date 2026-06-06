using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using System.IO;

namespace RO_Flex_UI.Editor
{
    public class FontAssetGenerator
    {
        // private const string fontsPath = "Assets/Samples/RO Flex UI";
        private const string fontsPath = "Assets/Fonts";
        // private const string characters = "abcdefghijklmnopqrstuvwxyzçABCDEFGHIJKLMNOPQRSTUVWXYZÇ 0123456789<>[]{}()\\/.,:;~`'\"!?@#$%^&*-_=+|áéíóúàèìòùâêîôûäëïöüãõñÁÉÍÓÚÀÈÌÒÙÂÊÎÔÛÄËÏÖÜÃÕÑ";
        // private const string characters = "abcdefghijklmnopqrstuvwxyzçABCDEFGHIJKLMNOPQRSTUVWXYZÇ 0123456789";

        private static readonly (string suffix, string shaderName)[] materialPresents = new[]
        {
            ("Default Bitmap", "TextMeshPro/Bitmap"),
            ("Pixel Outline", "RO Flex UI/PixelOutline"),
            ("Pixel Shadow", "RO Flex UI/PixelShadow")
        };

        [MenuItem("Tools/RO Flex UI/Regenerate TMP Fonts")]
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
            var sourceFont = AssetDatabase.LoadAssetAtPath<Font>(fontPath);
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
                // AtlasPopulationMode.Dynamic,
                AtlasPopulationMode.Static,
                true
            );

            if (fontAsset == null) return;

            fontAsset.hideFlags = HideFlags.None;
            fontAsset.atlasTexture.hideFlags = HideFlags.None;

            // fontAsset.TryAddCharacters(characters, out var missingCharacters);

            // if (!string.IsNullOrEmpty(missingCharacters))
            // {
            //     Debug.LogWarning($"Missing characters in {sourceFont.name}: {missingCharacters}");
            // }

            // fontAsset.atlasPopulationMode = AtlasPopulationMode.Static;

            fontAsset.atlasTexture.name = sourceFont.name + " Atlas";
            fontAsset.atlasTexture.filterMode = FilterMode.Point;
            fontAsset.atlasTexture.wrapMode = TextureWrapMode.Clamp;

            // var shader1 = Shader.Find("TextMeshPro/Bitmap");
            // var material1 = new Material(shader1)
            // {
            //     name = sourceFont.name + " Bitmap Material"
            // };

            // material.SetTexture(ShaderUtilities.ID_MainTex, fontAsset.atlasTexture);

            // fontAsset.material = material;
            // fontAsset.atlasTextures = new[] { fontAsset.atlasTexture };

            AssetDatabase.CreateAsset(fontAsset, assetPath);
            AssetDatabase.AddObjectToAsset(fontAsset.atlasTexture, fontAsset);
            // AssetDatabase.AddObjectToAsset(fontAsset.material, fontAsset);

            CreateMaterialPresets(fontAsset);

            EditorUtility.SetDirty(fontAsset);
            EditorUtility.SetDirty(fontAsset.atlasTexture);
            // EditorUtility.SetDirty(material);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Successfully generated asset at {assetPath}");
        }

        private static void CreateMaterialPresets(TMP_FontAsset fontAsset)
        {
            foreach (var preset in materialPresents)
            {
                var shader = Shader.Find(preset.shaderName);

                if (shader == null)
                {
                    Debug.LogWarning($"Shader not found: {preset.shaderName}");
                    continue;
                }

                var material = new Material(shader)
                {
                    name = $"{fontAsset.name} {preset.suffix}"
                };

                material.SetTexture("_MainTex", fontAsset.atlasTexture);

                AssetDatabase.AddObjectToAsset(material, fontAsset);
                EditorUtility.SetDirty(material);

                if (fontAsset.material == null || preset.suffix == "Default Bitmap")
                {
                    fontAsset.material = material;
                }
            }

            EditorUtility.SetDirty(fontAsset);
        }
    }
}