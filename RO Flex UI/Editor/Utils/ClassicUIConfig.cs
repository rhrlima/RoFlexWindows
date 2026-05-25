using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RO_Flex_UI.Editor
{
    public static class SceneFontSetter
    {
        [MenuItem("Tools/RO Flex UI/Apply Font To Scene Texts")]
        public static void ApplyFontToSceneTexts()
        {
            // Change these paths to your package/sample paths
            const string tmpFontPath = "Assets/Samples/RO Flex UI/1.0.0/Fonts/RO-custom-regular.asset";

            TMP_FontAsset tmpFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(tmpFontPath);

            if (tmpFont == null)
            {
                Debug.LogError($"TMP font not found at path: {tmpFontPath}");
                return;
            }

            int tmpUiCount = 0;
            int tmp3dCount = 0;
            int legacyCount = 0;

            foreach (GameObject root in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                foreach (TextMeshProUGUI text in root.GetComponentsInChildren<TextMeshProUGUI>(true))
                {
                    Undo.RecordObject(text, "Apply TMP Font");
                    text.font = tmpFont;
                    text.fontSize = 16;
                    EditorUtility.SetDirty(text);
                    tmpUiCount++;
                }

                foreach (TextMeshPro text in root.GetComponentsInChildren<TextMeshPro>(true))
                {
                    Undo.RecordObject(text, "Apply TMP Font");
                    text.font = tmpFont;
                    text.fontSize = 16;
                    EditorUtility.SetDirty(text);
                    tmp3dCount++;
                }
            }

            EditorSceneManagerHelper.MarkSceneDirty();

            Debug.Log(
                $"Applied fonts to scene texts. " +
                $"TMP UI: {tmpUiCount}, TMP 3D: {tmp3dCount}, Legacy UI Text: {legacyCount}"
            );
        }
    }

    internal static class EditorSceneManagerHelper
    {
        public static void MarkSceneDirty()
        {
            var scene = SceneManager.GetActiveScene();

            if (scene.IsValid() && scene.isLoaded)
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
            }
        }
    }
}