using RO_Flex_UI.Components;
using UnityEditor;
using UnityEditor.UI;

namespace RO_Flex_UI.Editor
{
    [CustomEditor(typeof(RoScrollbar))]
    public class RoScrollbarEditor : ScrollbarEditor
    {
        private SerializedProperty stepPerc;
        private SerializedProperty onDecreaseClick;
        private SerializedProperty onIncreaseClick;
        private SerializedProperty onEndScroll;

        protected override void OnEnable()
        {
            base.OnEnable();

            stepPerc = serializedObject.FindProperty("stepPerc");
            onDecreaseClick = serializedObject.FindProperty("onDecreaseClick");
            onIncreaseClick = serializedObject.FindProperty("onIncreaseClick");
            onEndScroll = serializedObject.FindProperty("onEndScroll");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.Slider(stepPerc, 0f, 1f);
            EditorGUILayout.PropertyField(onDecreaseClick);
            EditorGUILayout.PropertyField(onIncreaseClick);
            EditorGUILayout.PropertyField(onEndScroll);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
