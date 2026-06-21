using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace RO_Flex_UI.Editor
{
    public static class PrefabMenuGenerator
    {
        private const string PREFABS_ROOT = "Packages/com.ricric.roflexui/Runtime/Prefabs";
        private const string OUTPUT_PATH = "Packages/com.ricric.roflexui/Editor/Generated/ROFlexUIMenu.cs";
        private const string SCRIPT_TEMPLATE =
        @"// --- GENERATED CODE - DO NOT EDIT MANUALLY ---
using RO_Flex_UI.Editor;
using UnityEditor;
using UnityEngine;

namespace RoFlexUI.Editor
{{
    public static class ROFlexUI_PrefabsMenu
    {{
{0}
    }}
}}";
        private const string ITEM_TEMPLATE =
        @"        [MenuItem(""GameObject/ROFlexUI/{0}"", false, 10)]
        public static void Create_{1}(MenuCommand command)
        {{
            CreateUtils.CreatePrefab(""{2}"", command.context as GameObject);
        }}
";

        [MenuItem("Tools/ROFlexUI/Regenerate Prefab Menu")]
        public static void BakeMenu()
        {
            var guids = AssetDatabase.FindAssets("t:Prefab", new[] { PREFABS_ROOT });
            var allMethods = new StringBuilder();

            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);

                // Calculates the relative path to the ROOT folder to create sub-menus
                // i.e: "Windows/Popups/MyPrefab.prefab"
                var relativePath = assetPath.Replace(PREFABS_ROOT + "/", "");
                // var fileNameWithExtension = Path.GetFileName(assetPath);
                var menuPath = relativePath.Replace(".prefab", "");

                var safeMethodName = menuPath.Replace("/", "_").Replace(" ", "").Replace("-", "_");

                // {0} = Menu path (ex: Windows/Popup1)
                // {1} = Safe method name
                // {2} = Asset path
                allMethods.AppendLine(string.Format(ITEM_TEMPLATE, menuPath, safeMethodName, assetPath));
            }

            var finalCode = string.Format(SCRIPT_TEMPLATE, allMethods.ToString());

            var directory = Path.GetDirectoryName(OUTPUT_PATH);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            File.WriteAllText(OUTPUT_PATH, finalCode);
            AssetDatabase.Refresh();

            Debug.Log($"<b>ROFlexUI:</b> Prefab menu generated at: {OUTPUT_PATH}");
        }
    }
}
