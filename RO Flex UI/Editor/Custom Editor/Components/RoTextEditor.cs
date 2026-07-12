using RO_Flex_UI.Components;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace RO_Flex_UI.Editor
{
    [CustomEditor(typeof(RoText), true), CanEditMultipleObjects]
    public class RoTextEditor : TMP_EditorPanelUI
    {
        private SerializedProperty overrideOutlineColor;
        private SerializedProperty outlineColor;

        protected override void OnEnable()
        {
            base.OnEnable();

            overrideOutlineColor = serializedObject.FindProperty("overrideOutlineColor");
            outlineColor = serializedObject.FindProperty("outlineColor");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            DrawOutlineSection();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                ApplyOutlineColorOverrideToTargets();
            }
            else
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawOutlineSection()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("RO Outline", EditorStyles.boldLabel);

            var supportsOutline = SupportsOutlineColor();
            EditorGUILayout.PropertyField(overrideOutlineColor, new GUIContent("Override Outline Color"));

            using (new EditorGUI.DisabledScope(!supportsOutline || !overrideOutlineColor.boolValue))
                EditorGUILayout.PropertyField(outlineColor, new GUIContent("Outline Color"));

            if (!supportsOutline)
                EditorGUILayout.HelpBox("The selected material does not expose _OutlineColor.", MessageType.Info);
        }

        private bool SupportsOutlineColor()
        {
            foreach (var targetObject in targets)
            {
                if (targetObject is RoText text && text.SupportsOutlineColor)
                    return true;
            }

            return false;
        }

        private void ApplyOutlineColorOverrideToTargets()
        {
            foreach (var targetObject in targets)
            {
                if (targetObject is not RoText text)
                    continue;

                text.ApplyOutlineColorOverride();
                EditorUtility.SetDirty(text);
            }
        }
    }
}
