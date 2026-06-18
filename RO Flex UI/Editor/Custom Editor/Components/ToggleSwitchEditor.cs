using RO_Flex_UI.Components;
using UnityEditor;

namespace RO_Flex_UI.Editor
{
    [CustomEditor(typeof(ToggleSwitch)), CanEditMultipleObjects]
    public class ToggleSwitchEditor : UnityEditor.Editor
    {
        private SerializedProperty interactable;
        private SerializedProperty targetGraphic;
        private SerializedProperty handleRect;
        private SerializedProperty direction;
        private SerializedProperty value;
        private SerializedProperty animationDuration;
        private SerializedProperty slideEase;
        private SerializedProperty onToggle;
        private SerializedProperty onToggleOn;
        private SerializedProperty onToggleOff;

        private void OnEnable()
        {
            interactable = serializedObject.FindProperty("m_Interactable");
            targetGraphic = serializedObject.FindProperty("m_TargetGraphic");
            handleRect = serializedObject.FindProperty("m_HandleRect");
            direction = serializedObject.FindProperty("m_Direction");
            value = serializedObject.FindProperty("m_Value");
            animationDuration = serializedObject.FindProperty("animationDuration");
            slideEase = serializedObject.FindProperty("slideEase");
            onToggle = serializedObject.FindProperty("onToggle");
            onToggleOn = serializedObject.FindProperty("onToggleOn");
            onToggleOff = serializedObject.FindProperty("onToggleOff");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawBehaviorSection();
            EditorGUILayout.Space();
            DrawAnimationSection();
            EditorGUILayout.Space();
            DrawVisualsSection();
            EditorGUILayout.Space();
            DrawEventsSection();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawBehaviorSection()
        {
            EditorGUILayout.LabelField("Behavior", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(interactable);

            var isOn = value.floatValue >= 0.5f;
            EditorGUI.showMixedValue = value.hasMultipleDifferentValues;

            EditorGUI.BeginChangeCheck();
            isOn = EditorGUILayout.Toggle("Is On", isOn);

            if (EditorGUI.EndChangeCheck())
                value.floatValue = isOn ? 1f : 0f;

            EditorGUI.showMixedValue = false;
        }

        private void DrawAnimationSection()
        {
            EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(animationDuration);

            if (EditorGUI.EndChangeCheck())
                animationDuration.floatValue = UnityEngine.Mathf.Max(0f, animationDuration.floatValue);

            EditorGUILayout.PropertyField(slideEase);
        }

        private void DrawVisualsSection()
        {
            EditorGUILayout.LabelField("Visuals", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(targetGraphic);
            EditorGUILayout.PropertyField(handleRect);
            EditorGUILayout.PropertyField(direction);
        }

        private void DrawEventsSection()
        {
            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(onToggle);
            EditorGUILayout.PropertyField(onToggleOn);
            EditorGUILayout.PropertyField(onToggleOff);
        }
    }
}
