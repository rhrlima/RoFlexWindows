using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RO_Flex_UI.Editor
{
    public class FontAssetPostprocessor : AssetPostprocessor
    {
        private static readonly string[] ValidFontExtensions = { ".ttf", ".otf" };
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            OnPostprocessFonts(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
        }

        private static void OnPostprocessFonts(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (var assetPath in importedAssets)
            {
                if (!ValidFontExtensions.Any(assetPath.EndsWith))
                {
                    continue;
                }

                if (!assetPath.Contains("Samples/ROFlexUI"))
                {
                    continue;
                }

                FontAssetGenerator.Generate(assetPath);
            }
        }
    }
}