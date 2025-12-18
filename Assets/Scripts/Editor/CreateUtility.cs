using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class CreateUtility
{
    public static void CreatePrefab(string path, GameObject parent = null)
    {
        // GameObject newObject = PrefabUtility.InstantiatePrefab(Resources.Load(path)) as GameObject;
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        if (prefab == null)
        {
            Debug.LogError($"Prefab not found at path: {path}");
            return;
        }

        var newObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

        if (parent != null)
        {
            newObject.transform.SetParent(parent.transform, false);
        }

        Place(newObject);
    }

    public static void Place(GameObject gameObject)
    {
        SceneView lastView = SceneView.lastActiveSceneView;
        gameObject.transform.position = lastView ? lastView.pivot : Vector3.zero;

        StageUtility.PlaceGameObjectInCurrentStage(gameObject);
        GameObjectUtility.EnsureUniqueNameForSibling(gameObject);

        Undo.RegisterChildrenOrderUndo(gameObject, $"Create Object: {gameObject.name}");
        Selection.activeGameObject = gameObject;

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
