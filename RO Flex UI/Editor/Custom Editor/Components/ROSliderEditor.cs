using RO_Flex_UI.Components;
using UnityEditor;
using UnityEngine.UI;

namespace RO_Flex_UI.Editor
{
    [CustomEditor(typeof(ROSlider))]
    public class ROSliderEditor : UnityEditor.Editor
    {
        private SerializedProperty stepPercentProperty;

        private SerializedProperty orientationProperty;

        private SerializedProperty onValueChangedProperty;

        private ROSlider sliderTarget;

        private void OnEnable()
        {
            sliderTarget = (ROSlider)target;

            stepPercentProperty =
                serializedObject.FindProperty("stepPercent");

            orientationProperty =
                serializedObject.FindProperty("direction");

            onValueChangedProperty =
                serializedObject.FindProperty("onValueChanged");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            serializedObject.Update();

            EditorGUILayout.Space();

            DrawBehaviorSection();

            serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(sliderTarget);
        }

        private void DrawBehaviorSection()
        {
            EditorGUILayout.LabelField(
                "Behavior",
                EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();

            sliderTarget.Interactable = EditorGUILayout.Toggle(
                "Interactable",
                sliderTarget.Interactable
            );

            sliderTarget.Direction = (Slider.Direction)EditorGUILayout.EnumPopup(
                "Direction",
                sliderTarget.Direction
            );

            sliderTarget.MinValue = EditorGUILayout.FloatField(
                "Min Value",
                sliderTarget.MinValue);

            sliderTarget.MaxValue = EditorGUILayout.FloatField(
                "Max Value",
                sliderTarget.MaxValue);

            sliderTarget.WholeNumbers = EditorGUILayout.Toggle(
                "Whole Numbers",
                sliderTarget.WholeNumbers);

            sliderTarget.Value = EditorGUILayout.Slider(
                "Value",
                sliderTarget.Value,
                sliderTarget.MinValue,
                sliderTarget.MaxValue
            );

            EditorGUILayout.Slider(
                    stepPercentProperty,
                    0f,
                    1f
            );

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(onValueChangedProperty);
        }
    }
}