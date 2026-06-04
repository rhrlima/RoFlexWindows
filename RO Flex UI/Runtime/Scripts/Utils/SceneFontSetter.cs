using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RO_Flex_UI.Utils
{
    public static class SceneFontSetter
    {
        [MenuItem("Tools/RO Flex UI/Apply Font To Scene Texts")]
        public static void ApplyFontToSceneTexts()
        {
            const string tmpFontPath = "Assets/Fonts/RO-custom-regular.asset";

            TMP_FontAsset tmpFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(tmpFontPath);

            if (tmpFont == null)
            {
                Debug.LogError($"TMP font not found at path: {tmpFontPath}");
                return;
            }

            int tmpUiCount = 0;
            int tmp3dCount = 0;

            foreach (GameObject root in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                foreach (TextMeshProUGUI text in root.GetComponentsInChildren<TextMeshProUGUI>(true))
                {
                    Undo.RecordObject(text, "Apply TMP Font");

                    ApplyTmpFontSettings(text, tmpFont);

                    EditorUtility.SetDirty(text);
                    tmpUiCount++;
                }

                foreach (TextMeshPro text in root.GetComponentsInChildren<TextMeshPro>(true))
                {
                    Undo.RecordObject(text, "Apply TMP Font");

                    ApplyTmpFontSettings(text, tmpFont);

                    EditorUtility.SetDirty(text);
                    tmp3dCount++;
                }
            }

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                SceneManager.GetActiveScene()
            );

            Debug.Log(
                $"Applied fonts. TMP UI: {tmpUiCount}, TMP 3D: {tmp3dCount}"
            );
        }

        private static void ApplyTmpFontSettings(TMP_Text text, TMP_FontAsset font)
        {
            text.font = font;
            text.fontSize = 16;
            text.SetAllDirty();
        }
    }
}