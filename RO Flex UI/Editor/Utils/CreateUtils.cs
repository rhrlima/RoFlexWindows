using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RO_Flex_UI.Editor
{
    public static class CreateUtils
    {
        public static void CreatePrefab(string path, GameObject parent = null)
        {
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
            var lastView = SceneView.lastActiveSceneView;
            gameObject.transform.position = lastView ? lastView.pivot : Vector3.zero;

            StageUtility.PlaceGameObjectInCurrentStage(gameObject);
            GameObjectUtility.EnsureUniqueNameForSibling(gameObject);

            Undo.RegisterChildrenOrderUndo(gameObject, $"Create Object: {gameObject.name}");
            Selection.activeGameObject = gameObject;

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}