using RO_Flex_UI.Config;
using UnityEditor;

[CustomEditor(typeof(GlobalUIConfigData))]
public class GlobalUIConfigDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var config = target as GlobalUIConfigData;

        var newUIScale = EditorGUILayout.IntSlider("UI Slider", config.uiScale, 1, 4);
        config.SetScale(newUIScale);
    }
}
