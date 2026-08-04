using UnityEngine;

#pragma warning disable UDR0001

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
                    _instance = FindAnyObjectByType<T>(FindObjectsInactive.Include);

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        singletonObject.name = $"[Singleton] {typeof(T)}";
                        _instance = singletonObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                _applicationIsQuitting = false;
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