using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace RO_Flex_UI.Tests
{
    public class SingletonPlayModeTests
    {
        private const string UiManagerTypeName = "RO_Flex_UI.Windows.UiManager, Assembly-CSharp";

        private Type uiManagerType;
        private Type singletonType;
        private PropertyInfo instanceProperty;
        private FieldInfo instanceField;
        private FieldInfo applicationIsQuittingField;
        private MethodInfo onApplicationQuitMethod;

        [SetUp]
        public void SetUp()
        {
            ResolveSingletonMembers();
            DestroyAllInstances();
            ResetStaticState();
        }

        [TearDown]
        public void TearDown()
        {
            DestroyAllInstances();
            ResetStaticState();
        }

        [Test]
        public void InstanceReturnsExistingActiveInstance()
        {
            var existing = CreateUiManager("Existing UiManager");
            instanceField.SetValue(null, null);

            var instance = GetInstance();

            Assert.AreSame(existing, instance);
            Assert.AreEqual(1, FindAllInstances().Length);
        }

        [Test]
        public void InstanceFindsInactiveInstanceWithoutCreatingDuplicate()
        {
            var root = new GameObject("Inactive UiManager");
            root.SetActive(false);
            var existing = root.AddComponent(uiManagerType) as Component;
            instanceField.SetValue(null, null);

            var instance = GetInstance();

            Assert.AreSame(existing, instance);
            Assert.IsFalse(instance.gameObject.activeSelf);
            Assert.AreEqual(1, FindAllInstances().Length);
        }

        [Test]
        public void InstanceCreatesNamedPersistentObjectAndReturnsSameInstance()
        {
            var first = GetInstance();
            var second = GetInstance();

            Assert.IsNotNull(first);
            Assert.AreSame(first, second);
            Assert.AreEqual($"[Singleton] {uiManagerType}", first.gameObject.name);
            Assert.AreEqual("DontDestroyOnLoad", first.gameObject.scene.name);
            Assert.AreEqual(1, FindAllInstances().Length);
        }

        [UnityTest]
        public IEnumerator DuplicateInstanceIsDestroyedWhileOriginalSurvives()
        {
            var original = CreateUiManager("Original UiManager");
            var duplicate = CreateUiManager("Duplicate UiManager");

            yield return null;

            Assert.IsNotNull(original);
            Assert.IsTrue(duplicate == null, "Expected the duplicate component to be destroyed.");
            Assert.AreSame(original, GetInstance());
            Assert.AreEqual(1, FindAllInstances().Length);
        }

        [UnityTest]
        public IEnumerator DestroyingInstanceAllowsReplacementToBeCreated()
        {
            var original = GetInstance();
            UnityEngine.Object.Destroy(original.gameObject);

            yield return null;

            var replacement = GetInstance();
            Assert.IsNotNull(replacement);
            Assert.IsTrue(original == null, "Expected the original component to be destroyed.");
            Assert.AreNotSame(original, replacement);
            Assert.AreEqual(1, FindAllInstances().Length);
        }

        [Test]
        public void InstanceDoesNotRecreateSingletonAfterApplicationQuit()
        {
            var existing = GetInstance();
            onApplicationQuitMethod.Invoke(existing, null);
            LogAssert.Expect(
                LogType.Warning,
                $"[Singleton] Instance '{uiManagerType}' already destroyed on application quit.");

            var instanceAfterQuit = GetInstance();

            Assert.IsNull(instanceAfterQuit);
            Assert.AreEqual(1, FindAllInstances().Length);
        }

        private void ResolveSingletonMembers()
        {
            uiManagerType = Type.GetType(UiManagerTypeName);
            Assert.IsNotNull(uiManagerType, $"Expected to resolve {UiManagerTypeName}.");

            singletonType = uiManagerType.BaseType;
            Assert.IsNotNull(singletonType, "Expected UiManager to derive from Singleton<UiManager>.");

            instanceProperty = singletonType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
            instanceField = singletonType.GetField("_instance", BindingFlags.NonPublic | BindingFlags.Static);
            applicationIsQuittingField = singletonType.GetField(
                "_applicationIsQuitting",
                BindingFlags.NonPublic | BindingFlags.Static);
            onApplicationQuitMethod = singletonType.GetMethod(
                "OnApplicationQuit",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(instanceProperty, "Expected to resolve the public Instance property.");
            Assert.IsNotNull(instanceField, "Expected to resolve the singleton instance field.");
            Assert.IsNotNull(applicationIsQuittingField, "Expected to resolve the application quitting field.");
            Assert.IsNotNull(onApplicationQuitMethod, "Expected to resolve OnApplicationQuit.");
        }

        private Component GetInstance()
        {
            return instanceProperty.GetValue(null) as Component;
        }

        private Component CreateUiManager(string name)
        {
            var root = new GameObject(name);
            return root.AddComponent(uiManagerType) as Component;
        }

        private Component[] FindAllInstances()
        {
            var objects = UnityEngine.Object.FindObjectsByType(
                uiManagerType,
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);
            var instances = new Component[objects.Length];

            for (var i = 0; i < objects.Length; i++)
                instances[i] = objects[i] as Component;

            return instances;
        }

        private void DestroyAllInstances()
        {
            if (uiManagerType == null)
                return;

            foreach (var instance in FindAllInstances())
            {
                if (instance != null)
                    UnityEngine.Object.DestroyImmediate(instance.gameObject);
            }
        }

        private void ResetStaticState()
        {
            instanceField?.SetValue(null, null);
            applicationIsQuittingField?.SetValue(null, false);
        }
    }
}
