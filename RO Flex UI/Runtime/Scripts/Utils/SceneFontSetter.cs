using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RO_Flex_UI.Utils
{
    public class SceneFontSetter : MonoBehaviour
    {
        [SerializeField] private TMP_FontAsset fontAsset;
        [SerializeField] private float fontSize = 16;

        private void Start()
        {
            ApplyFontToSceneTexts();
        }

        public void ApplyFontToSceneTexts()
        {
            if (fontAsset == null)
            {
                Debug.LogError("TMP font not found.", this);
                return;
            }

            var tmpUiCount = 0;
            var tmp3dCount = 0;

            foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                foreach (var text in root.GetComponentsInChildren<TextMeshProUGUI>(true))
                {
                    ApplyTmpFontSettings(text);
                    tmpUiCount++;
                }

                foreach (var text in root.GetComponentsInChildren<TextMeshPro>(true))
                {
                    ApplyTmpFontSettings(text);
                    tmp3dCount++;
                }
            }

            Debug.Log(
                $"Applied font '{fontAsset.name}'. TMP UI: {tmpUiCount}, TMP 3D: {tmp3dCount}",
                this
            );
        }

        private void ApplyTmpFontSettings(TMP_Text text)
        {
#if UNITY_EDITOR
            Undo.RecordObject(text, "Apply TMP Font");
#endif

            text.font = fontAsset;
            text.fontSize = fontSize;

#if UNITY_EDITOR
            EditorUtility.SetDirty(text);
#endif
        }
    }
}