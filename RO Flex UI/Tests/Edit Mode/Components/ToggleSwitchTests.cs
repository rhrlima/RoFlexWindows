using RO_Flex_UI.Components;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Tests
{
    public class ToggleSwitchTests
    {
        [Test]
        public void InitialOffStateReportsIsOnFalse()
        {
            var fixture = CreateToggleSwitch();

            fixture.Toggle.SetIsOn(false, notify: false, animated: false);

            Assert.IsFalse(fixture.Toggle.IsOn, "ToggleSwitch should report off when its value is 0.");

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void SetIsOnWithoutNotifyDoesNotInvokeToggleEvents()
        {
            var fixture = CreateToggleSwitch();
            var toggleCount = 0;
            var onCount = 0;
            fixture.Toggle.onToggle.AddListener(_ => toggleCount++);
            fixture.Toggle.onToggleOn.AddListener(() => onCount++);

            fixture.Toggle.SetIsOn(true, notify: false, animated: false);

            Assert.IsTrue(fixture.Toggle.IsOn, "ToggleSwitch should still update state when notifications are disabled.");
            Assert.AreEqual(0, toggleCount, "ToggleSwitch.onToggle should not invoke when notify is false.");
            Assert.AreEqual(0, onCount, "ToggleSwitch.onToggleOn should not invoke when notify is false.");

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void CompletedLeftClickTogglesOnAndInvokesOnEvents()
        {
            var fixture = CreateToggleSwitch();
            SetAnimationDuration(fixture.Toggle, 0f);
            var toggleValue = false;
            var toggleCount = 0;
            var onCount = 0;
            fixture.Toggle.onToggle.AddListener(value =>
            {
                toggleValue = value;
                toggleCount++;
            });
            fixture.Toggle.onToggleOn.AddListener(() => onCount++);

            fixture.Toggle.OnPointerClick(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.IsTrue(fixture.Toggle.IsOn, "ToggleSwitch should turn on after a completed left click.");
            Assert.IsTrue(toggleValue, "ToggleSwitch.onToggle should emit true when turning on.");
            Assert.AreEqual(1, toggleCount, "ToggleSwitch.onToggle should invoke once when turning on.");
            Assert.AreEqual(1, onCount, "ToggleSwitch.onToggleOn should invoke once when turning on.");

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void SecondCompletedLeftClickTogglesOffAndInvokesOffEvents()
        {
            var fixture = CreateToggleSwitch();
            SetAnimationDuration(fixture.Toggle, 0f);
            fixture.Toggle.SetIsOn(true, notify: false, animated: false);
            var toggleValue = true;
            var toggleCount = 0;
            var offCount = 0;
            fixture.Toggle.onToggle.AddListener(value =>
            {
                toggleValue = value;
                toggleCount++;
            });
            fixture.Toggle.onToggleOff.AddListener(() => offCount++);

            fixture.Toggle.OnPointerClick(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.IsFalse(fixture.Toggle.IsOn, "ToggleSwitch should turn off after a completed left click while on.");
            Assert.IsFalse(toggleValue, "ToggleSwitch.onToggle should emit false when turning off.");
            Assert.AreEqual(1, toggleCount, "ToggleSwitch.onToggle should invoke once when turning off.");
            Assert.AreEqual(1, offCount, "ToggleSwitch.onToggleOff should invoke once when turning off.");

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void PointerDownAndDragDoNotChangeValue()
        {
            var fixture = CreateToggleSwitch();
            fixture.Toggle.SetValueWithoutNotify(0f);

            fixture.Toggle.OnPointerDown(CreatePointerEventData(PointerEventData.InputButton.Left));
            fixture.Toggle.OnDrag(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.AreEqual(0f, fixture.Toggle.value, "ToggleSwitch should ignore pointer down and drag input.");

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void NonInteractableSwitchIgnoresClicks()
        {
            var fixture = CreateToggleSwitch();
            SetAnimationDuration(fixture.Toggle, 0f);
            fixture.Toggle.interactable = false;

            fixture.Toggle.OnPointerClick(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.IsFalse(fixture.Toggle.IsOn, "ToggleSwitch should ignore clicks while non-interactable.");

            Object.DestroyImmediate(fixture.Root);
        }

        private static ToggleSwitchFixture CreateToggleSwitch()
        {
            var root = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas));
            var toggleObject = new GameObject("ToggleSwitch", typeof(RectTransform));
            toggleObject.transform.SetParent(root.transform, false);

            var toggleSwitch = toggleObject.AddComponent<ToggleSwitch>();
            toggleSwitch.SetIsOn(false, notify: false, animated: false);

            return new ToggleSwitchFixture(root, toggleSwitch);
        }

        private static PointerEventData CreatePointerEventData(PointerEventData.InputButton button)
        {
            return new PointerEventData(null)
            {
                button = button
            };
        }

        private static void SetAnimationDuration(ToggleSwitch toggleSwitch, float duration)
        {
            var field = typeof(ToggleSwitch).GetField(
                "animationDuration",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            field.SetValue(toggleSwitch, duration);
        }

        private readonly struct ToggleSwitchFixture
        {
            public ToggleSwitchFixture(GameObject root, ToggleSwitch toggle)
            {
                Root = root;
                Toggle = toggle;
            }

            public GameObject Root { get; }
            public ToggleSwitch Toggle { get; }
        }
    }
}
