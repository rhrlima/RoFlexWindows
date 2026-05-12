

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

            if (go.TryGetComponent<VerticalLayoutGroup>(out var vlg))
                DestroyImmediate(vlg);

            if (go.TryGetComponent<HorizontalLayoutGroup>(out var hlg))
                DestroyImmediate(hlg);

            if (newOrientation == FlexPanel.Orientation.Vertical)
            {
                vlg = go.AddComponent<VerticalLayoutGroup>();
                vlg.childControlHeight = true;
                vlg.childForceExpandHeight = false;
                vlg.childControlWidth = true;
                vlg.childForceExpandWidth = true;
            }

            if (newOrientation == FlexPanel.Orientation.Horizontal)
            {
                hlg = go.AddComponent<HorizontalLayoutGroup>();
                hlg.childControlWidth = true;
                hlg.childForceExpandWidth = false;
                hlg.childControlHeight = true;
                hlg.childForceExpandHeight = true;
            }
        }
    }
}