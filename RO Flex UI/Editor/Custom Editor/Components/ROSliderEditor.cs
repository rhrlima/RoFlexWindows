using RO_Flex_UI.Components;
using UnityEditor;
using UnityEditor.UI;

namespace RO_Flex_UI.Editor
{
    [CustomEditor(typeof(RoSlider))]
    public class RoSlider2Editor : SliderEditor
    {
        private SerializedProperty stepSize;
        private SerializedProperty onDecreaseClick;
        private SerializedProperty onIncreaseClick;
        private SerializedProperty onPointerUp;

        protected override void OnEnable()
        {
            base.OnEnable();

            stepSize = serializedObject.FindProperty("stepSize");
            onDecreaseClick = serializedObject.FindProperty("onDecreaseClick");
            onIncreaseClick = serializedObject.FindProperty("onIncreaseClick");
            onPointerUp = serializedObject.FindProperty("onPointerUp");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.Slider(stepSize, 0f, 1f);

            EditorGUILayout.PropertyField(onDecreaseClick);
            EditorGUILayout.PropertyField(onIncreaseClick);
            EditorGUILayout.PropertyField(onPointerUp);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
