using RO_Flex_UI.Components;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace RO_Flex_UI.Tests
{
    public class RoButtonTests
    {
        private const string PrefabPath = Setup.PrefabRoot + "Components/RoButton.prefab";

        [Test]
        public void SetLabelFromCode()
        {
            var gameObject = new GameObject("RoButton");
            var button = gameObject.AddComponent<RoButton>();

            Assert.IsTrue(button.interactable, "A newly created RoButton should be interactable by default.");

            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void InteractableCanBeChangedFromCode()
        {
            var gameObject = new GameObject("RoButton");
            var button = gameObject.AddComponent<RoButton>();

            button.interactable = false;
            Assert.IsFalse(button.interactable, "RoButton should report interactable false after being disabled from code.");

            button.interactable = true;
            Assert.IsTrue(button.interactable, "RoButton should report interactable true after being enabled from code.");

            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void OnClickInvokesRegisteredListener()
        {
            var gameObject = new GameObject("RoButton");
            var button = gameObject.AddComponent<RoButton>();
            var clickCount = 0;

            button.onClick.AddListener(() => clickCount++);
            button.onClick.Invoke();

            Assert.AreEqual(1, clickCount, "RoButton.onClick should invoke a registered listener exactly once.");

            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void OnClickDoesNotInvokeRemovedListener()
        {
            var gameObject = new GameObject("RoButton");
            var button = gameObject.AddComponent<RoButton>();
            var clickCount = 0;
            void listener() => clickCount++;

            button.onClick.AddListener(listener);
            button.onClick.RemoveListener(listener);
            button.onClick.Invoke();

            Assert.AreEqual(0, clickCount, "RoButton.onClick should not invoke a listener after it has been removed.");

            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void CanBeInstantiatedFromPrefab()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            Assert.IsNotNull(prefab, $"Expected to load prefab at path: {PrefabPath}");

            var instance = Object.Instantiate(prefab);
            var button = instance.GetComponent<RoButton>();
            Assert.IsNotNull(button, $"Expected root object of {PrefabPath} to contain {typeof(RoButton).FullName}.");

            Object.DestroyImmediate(instance);
        }
    }
}
