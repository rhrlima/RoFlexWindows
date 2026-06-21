using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RO_Flex_UI.Components;
using RO_Flex_UI.Panels;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RO_Flex_UI.Tests
{
    public class ListPanelTests
    {
        [Test]
        public void ConfiguredItemsAreValidatedInConfiguredOrder()
        {
            var fixture = CreatePanelFixture();
            var externalParent = new GameObject("External", typeof(RectTransform));
            var first = CreateListItem("First", externalParent.transform);
            var second = CreateListItem("Second", externalParent.transform);
            var unlistedChild = CreateListItem("Unlisted", fixture.Root.transform);

            SetConfiguredItems(fixture.Panel, first, null, first, second);
            InvokeStart(fixture.Panel);

            Assert.AreSame(fixture.Root.transform, first.transform.parent);
            Assert.AreSame(fixture.Root.transform, second.transform.parent);
            Assert.IsTrue(first.gameObject.activeSelf);
            Assert.AreSame(second.TargetButton, first.TargetButton.navigation.selectOnDown);
            Assert.AreSame(first.TargetButton, second.TargetButton.navigation.selectOnDown);
            Assert.AreEqual(
                Navigation.Mode.Automatic,
                unlistedChild.GetComponent<RoButton>().navigation.mode);
            Assert.AreEqual(2, GetRuntimeItems(fixture.Panel).Count);

            Object.DestroyImmediate(externalParent);
            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void ConfiguredItemsIgnoreTemplate()
        {
            var fixture = CreatePanelFixture();
            var item = CreateListItem("Item", fixture.Root.transform);
            var template = CreateListItem("Template", fixture.Root.transform);

            SetObjectReference(fixture.Panel, "template", template);
            SetConfiguredItems(fixture.Panel, template, item);
            InvokeStart(fixture.Panel);

            Assert.IsFalse(template.gameObject.activeSelf);
            Assert.AreEqual(1, GetRuntimeItems(fixture.Panel).Count);

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void AddItemAndAddItemsAppendValidItems()
        {
            var fixture = CreatePanelFixture();
            var first = CreateListItem("First", null);
            var second = CreateListItem("Second", null);

            fixture.Panel.AddItem(first);
            fixture.Panel.AddItems(new[] { null, first, second });

            Assert.AreEqual(2, GetRuntimeItems(fixture.Panel).Count);
            Assert.AreSame(fixture.Root.transform, first.transform.parent);
            Assert.AreSame(second.TargetButton, first.TargetButton.navigation.selectOnDown);

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void GenericAddItemsClonesTemplateAndBindsData()
        {
            var fixture = CreatePanelFixture();
            var template = CreateListItem("Template", fixture.Root.transform);
            SetObjectReference(fixture.Panel, "template", template);
            InvokeStart(fixture.Panel);

            fixture.Panel.AddItems(new[] { "First", "Second" }, (item, value) => item.name = value);

            var items = GetRuntimeItems(fixture.Panel);
            Assert.AreEqual(2, items.Count);
            Assert.AreEqual("First", items[0].name);
            Assert.AreEqual("Second", items[1].name);
            Assert.IsFalse(template.gameObject.activeSelf);

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void OnSelectUpdatesFocusedItem()
        {
            var fixture = CreatePanelFixture();
            var first = CreateListItem("First", null);
            var second = CreateListItem("Second", null);
            fixture.Panel.AddItems(new[] { first, second });

            second.OnSelect(null);

            Assert.AreSame(second, fixture.Panel.FocusedItem);
            Assert.IsNull(fixture.Panel.ActivatedItem);

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void OnSubmitUpdatesActivatedItem()
        {
            var fixture = CreatePanelFixture();
            var item = CreateListItem("Item", null);
            fixture.Panel.AddItem(item);

            item.OnSubmit(null);

            Assert.AreSame(item, fixture.Panel.ActivatedItem);

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void ClearResetsRuntimeState()
        {
            var fixture = CreatePanelFixture();
            var item = CreateListItem("Item", null);
            fixture.Panel.AddItem(item);
            item.OnSelect(null);
            item.OnSubmit(null);

            fixture.Panel.Clear();

            Assert.AreEqual(0, GetRuntimeItems(fixture.Panel).Count);
            Assert.IsNull(fixture.Panel.FocusedItem);
            Assert.IsNull(fixture.Panel.ActivatedItem);

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void NavigationHandlesSingleItemAndDisabledLooping()
        {
            var fixture = CreatePanelFixture();
            var first = CreateListItem("First", null);
            fixture.Panel.AddItem(first);

            Assert.AreEqual(Navigation.Mode.Explicit, first.TargetButton.navigation.mode);
            Assert.IsNull(first.TargetButton.navigation.selectOnUp);
            Assert.IsNull(first.TargetButton.navigation.selectOnDown);

            var second = CreateListItem("Second", null);
            fixture.Panel.AddItem(second);
            fixture.Panel.LoopNavigation = false;

            Assert.IsNull(first.TargetButton.navigation.selectOnUp);
            Assert.AreSame(second.TargetButton, first.TargetButton.navigation.selectOnDown);
            Assert.AreSame(first.TargetButton, second.TargetButton.navigation.selectOnUp);
            Assert.IsNull(second.TargetButton.navigation.selectOnDown);

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void SelectOptionIgnoresInvalidIndexes()
        {
            var fixture = CreatePanelFixture();

            Assert.DoesNotThrow(() => fixture.Panel.SelectOption(-1));
            Assert.DoesNotThrow(() => fixture.Panel.SelectOption(0));

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void RenamedSerializedFieldsPreservePreviousNames()
        {
            AssertFieldFormerlyNamed("template", "defaultTemplate");
            AssertFieldFormerlyNamed("items", "listItems");
            AssertFieldFormerlyNamed("items", "initialItems");
        }

        private static PanelFixture CreatePanelFixture()
        {
            var root = new GameObject("List Panel", typeof(RectTransform));
            return new PanelFixture(root, root.AddComponent<ListPanel>());
        }

        private static ListItem CreateListItem(string name, Transform parent)
        {
            var itemObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(RoButton), typeof(ListItem));
            if (parent != null)
                itemObject.transform.SetParent(parent, false);
            return itemObject.GetComponent<ListItem>();
        }

        private static void SetConfiguredItems(ListPanel panel, params ListItem[] items)
        {
            var serializedPanel = new SerializedObject(panel);
            var listProperty = serializedPanel.FindProperty("items");
            listProperty.arraySize = items.Length;

            for (var i = 0; i < items.Length; i++)
                listProperty.GetArrayElementAtIndex(i).objectReferenceValue = items[i];

            serializedPanel.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetObjectReference(ListPanel panel, string propertyName, Object value)
        {
            var serializedPanel = new SerializedObject(panel);
            serializedPanel.FindProperty(propertyName).objectReferenceValue = value;
            serializedPanel.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void InvokeStart(ListPanel panel)
        {
            typeof(ListPanel).GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(panel, null);
        }

        private static List<ListItem> GetRuntimeItems(ListPanel panel)
        {
            return (List<ListItem>)typeof(ListPanel)
                .GetField("items", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(panel);
        }

        private static void AssertFieldFormerlyNamed(string fieldName, string previousName)
        {
            var field = typeof(ListPanel).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            var attributes = field.GetCustomAttributes<FormerlySerializedAsAttribute>();

            foreach (var attribute in attributes)
            {
                if (attribute.oldName == previousName)
                    return;
            }

            Assert.Fail($"{fieldName} does not preserve the serialized name {previousName}.");
        }

        private readonly struct PanelFixture
        {
            public PanelFixture(GameObject root, ListPanel panel)
            {
                Root = root;
                Panel = panel;
            }

            public GameObject Root { get; }
            public ListPanel Panel { get; }
        }
    }
}
