using System.Linq;
using UnityEditor;

namespace RO_Flex_UI.Editor
{
    public class FontAssetPostprocessor : AssetPostprocessor
    {
        private static readonly string[] ValidFontExtensions =
        {
            ".ttf",
            ".otf"
        };

        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            PrefabMenuGenerator.BakeMenu();
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

                if (!assetPath.Contains("Samples/RO Flex UI"))
                {
                    continue;
                }

                FontAssetGenerator.Generate(assetPath);
            }
        }
    }
}