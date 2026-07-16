using UnityEngine;

namespace RO_Flex_UI.Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
        private static bool _applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit.");
                    return null;
                }

                if (_instance == null)
                {
                    // Look for an existing instance
                    _instance = FindAnyObjectByType<T>(FindObjectsInactive.Include);

                    // Create new instance if none exists
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        singletonObject.name = $"[Singleton] {typeof(T)}";
                        _instance = singletonObject.AddComponent<T>();

                        // Optional: Persists across scenes
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>(FindObjectsInactive.Include);
            }
            _applicationIsQuitting = false;
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}