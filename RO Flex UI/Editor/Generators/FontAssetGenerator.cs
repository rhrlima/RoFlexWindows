using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using System.IO;
using TMPro.EditorUtilities;

namespace RO_Flex_UI.Editor
{
    public class FontAssetGenerator
    {
        private const string fontsPath = "Assets/Samples/RO Flex UI";
        private const string characters = "abcdefghijklmnopqrstuvwxyzçABCDEFGHIJKLMNOPQRSTUVWXYZÇ 0123456789<>[]{}()\\/.,:;~`'\"!?@#$%^&*-_=+|áéíóúàèìòùâêîôûäëïöüãõñÁÉÍÓÚÀÈÌÒÙÂÊÎÔÛÄËÏÖÜÃÕÑ";
        // private const string characters = "abcdefghijklmnopqrstuvwxyzçABCDEFGHIJKLMNOPQRSTUVWXYZÇ 0123456789";

        [MenuItem("Tools/RO Flex UI/Regenerate TMP Fonts")]
        public static void GenerateAll()
        {
            var guids = AssetDatabase.FindAssets("t:Font", new[] { fontsPath });
            Debug.Log($"Found {guids.Length} font assets to process.");

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                Generate(path);
                // Generate2(path);
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
                AtlasPopulationMode.Dynamic,
                true
            );

            if (fontAsset == null) return;

            fontAsset.hideFlags = HideFlags.None;
            fontAsset.atlasTexture.hideFlags = HideFlags.None;

            fontAsset.TryAddCharacters(characters, true);

            fontAsset.atlasTexture.name = sourceFont.name + " Atlas";

            fontAsset.atlasTexture.filterMode = FilterMode.Point;
            fontAsset.atlasTexture.wrapMode = TextureWrapMode.Clamp;

            // var shader = Shader.Find("TextMeshPro/Distance Field");
            var shader = Shader.Find("TextMeshPro/Bitmap");
            var material = new Material(shader);

            material.name = sourceFont.name + " Material";

            material.SetTexture("_MainTex", fontAsset.atlasTexture);

            fontAsset.material = material;

            AssetDatabase.CreateAsset(fontAsset, assetPath);
            AssetDatabase.AddObjectToAsset(fontAsset.atlasTexture, fontAsset);
            AssetDatabase.AddObjectToAsset(fontAsset.material, fontAsset);

            EditorUtility.SetDirty(fontAsset.atlasTexture);
            EditorUtility.SetDirty(fontAsset.material);
            EditorUtility.SetDirty(fontAsset);

            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

            ConvertAtlasToStatic(fontAsset);

            Debug.Log($"Successfully generated asset at {assetPath}");
        }

        private static void ConvertAtlasToStatic(TMP_FontAsset fontAsset)
        {
            if (fontAsset == null) return;

            fontAsset.atlasPopulationMode = AtlasPopulationMode.Static;

            EditorUtility.SetDirty(fontAsset);
            AssetDatabase.SaveAssets();

            Debug.Log("Converted atlas to static for font asset: " + fontAsset.name);
        }
    }
}