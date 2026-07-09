

using RO_Flex_UI.Panels;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UI;
using UnityEngine;

namespace RO_Flex_UI.Editor
{
    [CustomEditor(typeof(FlexPanel), true), CanEditMultipleObjects]
    public class FlexPanelEditor : UnityEditor.Editor
    {
        private FlexPanel panelTarget;
        private SerializedProperty spacing;
        private SerializedProperty entries;
        private ReorderableList entriesList;

        private void OnEnable()
        {
            panelTarget = (FlexPanel)target;
            spacing = serializedObject.FindProperty("spacing");
            entries = serializedObject.FindProperty("entries");

            entriesList = new ReorderableList(serializedObject, entries, true, false, true, true)
            {
                drawElementCallback = DrawEntry,
                elementHeightCallback = GetEntryHeight,
                onAddCallback = AddEntry
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            var newOrientation = (FlexPanel.Orientation)EditorGUILayout.EnumPopup(
                "Orientation",
                panelTarget.LayoutOrientation
            );

            if (EditorGUI.EndChangeCheck())
            {
                panelTarget.LayoutOrientation = newOrientation;

                SwapLayoutGroup(newOrientation);

                EditorUtility.SetDirty(panelTarget);
            }

            DrawEntries();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawEntries()
        {
            entries.isExpanded = EditorGUILayout.Foldout(entries.isExpanded, "Panels", true);

            if (entries.isExpanded)
                entriesList.DoLayoutList();
        }

        private static float GetEntryHeight(int index)
        {
            var line = EditorGUIUtility.singleLineHeight;
            var spacing = EditorGUIUtility.standardVerticalSpacing;
            return (line * 4f) + (spacing * 5f);
        }

        private void DrawEntry(Rect rect, int index, bool isActive, bool isFocused)
        {
            var entry = entries.GetArrayElementAtIndex(index);
            var rectTransform = entry.FindPropertyRelative("rect");
            var mode = entry.FindPropertyRelative("mode");
            var fixedSize = entry.FindPropertyRelative("fixedSize");
            var proportion = entry.FindPropertyRelative("proportion");

            var lineHeight = EditorGUIUtility.singleLineHeight;
            var spacingHeight = EditorGUIUtility.standardVerticalSpacing;

            rect.y += spacingHeight;
            rect.height = lineHeight;

            EditorGUI.LabelField(rect, $"Element {index}");

            rect.y += lineHeight + spacingHeight;
            EditorGUI.PropertyField(rect, rectTransform);

            rect.y += lineHeight + spacingHeight;
            EditorGUI.PropertyField(rect, mode);

            rect.y += lineHeight + spacingHeight;

            if (mode.enumValueIndex == 0)
                EditorGUI.PropertyField(rect, fixedSize, new GUIContent("Size"));
            else
                EditorGUI.PropertyField(rect, proportion);
        }

        private static void AddEntry(ReorderableList list)
        {
            var entries = list.serializedProperty;
            var index = entries.arraySize;
            entries.arraySize++;

            var entry = entries.GetArrayElementAtIndex(index);
            entry.FindPropertyRelative("rect").objectReferenceValue = null;
            entry.FindPropertyRelative("mode").enumValueIndex = 1;
            entry.FindPropertyRelative("fixedSize").floatValue = 24f;
            entry.FindPropertyRelative("proportion").floatValue = 1f;
        }

        private void SwapLayoutGroup(FlexPanel.Orientation newOrientation)
        {
            var go = panelTarget.gameObject;

            if (go.TryGetComponent<HorizontalOrVerticalLayoutGroup>(out var layoutGroup))
                DestroyImmediate(layoutGroup);

            if (newOrientation == FlexPanel.Orientation.Vertical)
            {
                layoutGroup = go.AddComponent<VerticalLayoutGroup>();
                layoutGroup.childForceExpandWidth = true;
                layoutGroup.childForceExpandHeight = false;
            }

            if (newOrientation == FlexPanel.Orientation.Horizontal)
            {
                layoutGroup = go.AddComponent<HorizontalLayoutGroup>();
                layoutGroup.childForceExpandWidth = false;
                layoutGroup.childForceExpandHeight = true;
            }

            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = true;
        }
    }
}
