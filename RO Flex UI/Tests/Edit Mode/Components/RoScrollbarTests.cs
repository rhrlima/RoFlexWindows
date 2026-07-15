using System.Reflection;
using NUnit.Framework;
using RO_Flex_UI.Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RO_Flex_UI.Tests
{
    public class RoScrollbarTests
    {
        private const string PrefabPath = "Assets/Prefabs/Components/RoScrollbar.prefab";

        [Test]
        public void PrefabContainsConfiguredButtons()
        {
            var fixture = CreateFixture();

            Assert.AreEqual(
                "RO_Flex_UI.Components.RoScrollbar",
                fixture.Scrollbar.GetType().FullName,
                "Expected the prefab root to use RoScrollbar.");
            Assert.IsNotNull(fixture.DecreaseButton, "Expected a configured decrease button.");
            Assert.IsNotNull(fixture.IncreaseButton, "Expected a configured increase button.");

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void ButtonsChangeValueByPercentageAndClamp()
        {
            var fixture = CreateFixture();
            SetStepPercentage(fixture.Scrollbar, 0.2f);
            fixture.Scrollbar.value = 0.5f;
            var decreaseValue = -1f;
            var increaseValue = -1f;
            GetScrollEvent(fixture.Scrollbar, "onDecreaseClick").AddListener(value => decreaseValue = value);
            GetScrollEvent(fixture.Scrollbar, "onIncreaseClick").AddListener(value => increaseValue = value);

            fixture.DecreaseButton.onClick.Invoke();
            Assert.AreEqual(0.3f, fixture.Scrollbar.value, 0.0001f);
            Assert.AreEqual(0.3f, decreaseValue, 0.0001f);

            fixture.IncreaseButton.onClick.Invoke();
            Assert.AreEqual(0.5f, fixture.Scrollbar.value, 0.0001f);
            Assert.AreEqual(0.5f, increaseValue, 0.0001f);

            fixture.Scrollbar.value = 0.95f;
            fixture.IncreaseButton.onClick.Invoke();
            Assert.AreEqual(1f, fixture.Scrollbar.value, 0.0001f);

            fixture.Scrollbar.value = 0.05f;
            fixture.DecreaseButton.onClick.Invoke();
            Assert.AreEqual(0f, fixture.Scrollbar.value, 0.0001f);

            Object.DestroyImmediate(fixture.Root);
        }

        [TestCase(Scrollbar.Direction.LeftToRight, "leftSprite", "rightSprite")]
        [TestCase(Scrollbar.Direction.RightToLeft, "rightSprite", "leftSprite")]
        [TestCase(Scrollbar.Direction.BottomToTop, "downSprite", "upSprite")]
        [TestCase(Scrollbar.Direction.TopToBottom, "upSprite", "downSprite")]
        public void DirectionAppliesExpectedButtonSprites(
            Scrollbar.Direction direction,
            string decreaseSpriteProperty,
            string increaseSpriteProperty)
        {
            var fixture = CreateFixture();
            fixture.Scrollbar.SetDirection(direction, includeRectLayouts: false);

            fixture.Scrollbar.enabled = false;
            fixture.Scrollbar.enabled = true;

            Assert.AreEqual(
                GetButtonSprite(fixture.Scrollbar, decreaseSpriteProperty),
                fixture.DecreaseButton.targetGraphic.GetComponent<Image>().sprite);
            Assert.AreEqual(
                GetButtonSprite(fixture.Scrollbar, increaseSpriteProperty),
                fixture.IncreaseButton.targetGraphic.GetComponent<Image>().sprite);

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void EndScrollFiresOnlyAfterValidPointerInteraction()
        {
            var fixture = CreateFixture();
            var invocationCount = 0;
            var endValue = -1f;
            GetScrollEvent(fixture.Scrollbar, "onEndScroll").AddListener(value =>
            {
                invocationCount++;
                endValue = value;
            });

            fixture.Scrollbar.OnPointerUp(CreatePointerEventData(PointerEventData.InputButton.Left));
            fixture.Scrollbar.OnPointerDown(CreatePointerEventData(PointerEventData.InputButton.Right));
            fixture.Scrollbar.OnPointerUp(CreatePointerEventData(PointerEventData.InputButton.Right));
            fixture.IncreaseButton.onClick.Invoke();
            Assert.AreEqual(0, invocationCount);

            fixture.Scrollbar.value = 0.4f;
            fixture.Scrollbar.OnPointerDown(CreatePointerEventData(PointerEventData.InputButton.Left));
            Assert.AreEqual(0, invocationCount, "End scroll should not fire on pointer down.");

            fixture.Scrollbar.OnPointerUp(CreatePointerEventData(PointerEventData.InputButton.Left));
            Assert.AreEqual(1, invocationCount);
            Assert.AreEqual(fixture.Scrollbar.value, endValue, 0.0001f);

            fixture.Scrollbar.interactable = false;
            fixture.Scrollbar.OnPointerDown(CreatePointerEventData(PointerEventData.InputButton.Left));
            fixture.Scrollbar.OnPointerUp(CreatePointerEventData(PointerEventData.InputButton.Left));
            Assert.AreEqual(1, invocationCount);

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void DisableAndEnableDoesNotDuplicateButtonListeners()
        {
            var fixture = CreateFixture();
            SetStepPercentage(fixture.Scrollbar, 0.2f);
            fixture.Scrollbar.value = 0f;

            fixture.Scrollbar.enabled = false;
            fixture.Scrollbar.enabled = true;
            fixture.Scrollbar.enabled = false;
            fixture.Scrollbar.enabled = true;
            fixture.IncreaseButton.onClick.Invoke();

            Assert.AreEqual(0.2f, fixture.Scrollbar.value, 0.0001f);

            Object.DestroyImmediate(fixture.Root);
        }

        private static Fixture CreateFixture()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            Assert.IsNotNull(prefab, $"Expected to load prefab at path: {PrefabPath}");

            var root = Object.Instantiate(prefab);
            var scrollbar = root.GetComponent<Scrollbar>();
            Assert.IsNotNull(scrollbar, $"Expected the root object of {PrefabPath} to contain a Scrollbar.");

            var serializedObject = new SerializedObject(scrollbar);
            return new Fixture(
                root,
                scrollbar,
                serializedObject.FindProperty("decreaseButton").objectReferenceValue as RoButton,
                serializedObject.FindProperty("increaseButton").objectReferenceValue as RoButton);
        }

        private static void SetStepPercentage(Scrollbar scrollbar, float percentage)
        {
            var serializedObject = new SerializedObject(scrollbar);
            serializedObject.FindProperty("stepPerc").floatValue = percentage;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static Sprite GetButtonSprite(Scrollbar scrollbar, string propertyName)
        {
            var serializedObject = new SerializedObject(scrollbar);
            return serializedObject.FindProperty($"buttonSprites.{propertyName}").objectReferenceValue as Sprite;
        }

        private static UnityEvent<float> GetScrollEvent(Scrollbar scrollbar, string fieldName)
        {
            var field = scrollbar.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
            Assert.IsNotNull(field, $"Expected to find public event field {fieldName}.");
            var scrollEvent = field.GetValue(scrollbar) as UnityEvent<float>;
            Assert.IsNotNull(scrollEvent, $"Expected {fieldName} to be initialized.");
            return scrollEvent;
        }

        private static PointerEventData CreatePointerEventData(PointerEventData.InputButton button)
        {
            return new PointerEventData(null)
            {
                button = button,
                position = Vector2.zero
            };
        }

        private readonly struct Fixture
        {
            public Fixture(GameObject root, Scrollbar scrollbar, RoButton decreaseButton, RoButton increaseButton)
            {
                Root = root;
                Scrollbar = scrollbar;
                DecreaseButton = decreaseButton;
                IncreaseButton = increaseButton;
            }

            public GameObject Root { get; }
            public Scrollbar Scrollbar { get; }
            public RoButton DecreaseButton { get; }
            public RoButton IncreaseButton { get; }
        }
    }
}
