using UnityEditor;
using UnityEngine;

public static class UIMenu
{
    [MenuItem("GameObject/Ragnarok/UI/Window")]
    public static void CreateWindow(MenuCommand command)
    {
        CreateUtility.CreatePrefab(
            "Assets/Prefabs/UI/Windows/Window.prefab",
            command.context as GameObject
        );
    }
}
