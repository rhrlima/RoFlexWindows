using RO_Flex_UI.Utils;
using UnityEditor;
using UnityEngine;

namespace RO_Flex_UI.Editor
{
    [CustomEditor(typeof(SceneFontSetter))]
    public class SceneFontSetterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Apply Font to Scene Texts"))
            {
                var setter = (SceneFontSetter)target;
                setter.ApplyFontToSceneTexts();
            }
        }
    }
}