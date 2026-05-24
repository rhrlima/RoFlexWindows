

using RO_Flex_UI.Panels;
using UnityEditor;
using UnityEngine.UI;

namespace RO_Flex_UI.Editor
{
    [CustomEditor(typeof(FlexPanel), true), CanEditMultipleObjects]
    public class FlexPanelEditor : UnityEditor.Editor
    {
        private FlexPanel panelTarget;

        private void OnEnable()
        {
            panelTarget = (FlexPanel)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

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

            serializedObject.ApplyModifiedProperties();
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