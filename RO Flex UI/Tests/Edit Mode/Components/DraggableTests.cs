using RO_Flex_UI.Components;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Tests
{
    public class DraggableTests
    {
        private const string PrefabPath = Setup.PrefabRoot + "Components/Draggable.prefab";

        [Test]
        public void ReturnToOriginCanBeChangedFromCode()
        {
            var fixture = CreateDraggable();
            var draggable = fixture.Draggable;

            Assert.IsFalse(draggable.ReturnToOrigin, "A newly created Draggable should not return to origin by default.");

            draggable.ReturnToOrigin = true;
            Assert.IsTrue(draggable.ReturnToOrigin, "Draggable should report ReturnToOrigin true after being enabled from code.");

            draggable.ReturnToOrigin = false;
            Assert.IsFalse(draggable.ReturnToOrigin, "Draggable should report ReturnToOrigin false after being disabled from code.");

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void BeginDragWithLeftButtonStartsDragging()
        {
            var fixture = CreateDraggable();
            var draggable = fixture.Draggable;
            var eventData = CreatePointerEventData(PointerEventData.InputButton.Left);

            draggable.OnBeginDrag(eventData);

            Assert.IsTrue(draggable.Dragging, "Draggable should enter dragging state after a left-button begin drag.");
            Assert.IsFalse(eventData.useDragThreshold, "Draggable should disable the drag threshold when dragging begins.");

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void BeginDragWithRightButtonDoesNotStartDragging()
        {
            var fixture = CreateDraggable();
            var draggable = fixture.Draggable;
            var eventData = CreatePointerEventData(PointerEventData.InputButton.Right);

            draggable.OnBeginDrag(eventData);

            Assert.IsFalse(draggable.Dragging, "Draggable should ignore begin drag events from buttons other than the left button.");

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void DragMovesTargetByPointerDeltaScaledByCanvas()
        {
            var fixture = CreateDraggable();
            var draggable = fixture.Draggable;
            fixture.Canvas.scaleFactor = 2f;
            fixture.Target.anchoredPosition = new Vector2(4f, 6f);

            draggable.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left));
            draggable.OnDrag(CreatePointerEventData(PointerEventData.InputButton.Left, new Vector2(8f, -4f)));

            Assert.AreEqual(new Vector2(8f, 4f), fixture.Target.anchoredPosition, "Draggable should move by pointer delta divided by the Canvas scale factor.");

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void EndDragStopsDraggingAndReturnsToOriginWhenEnabled()
        {
            var fixture = CreateDraggable();
            var draggable = fixture.Draggable;
            fixture.Target.anchoredPosition = new Vector2(10f, 20f);
            draggable.ReturnToOrigin = true;

            draggable.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left));
            draggable.OnDrag(CreatePointerEventData(PointerEventData.InputButton.Left, new Vector2(5f, 5f)));
            draggable.OnEndDrag(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.IsFalse(draggable.Dragging, "Draggable should leave dragging state when drag ends.");
            Assert.AreEqual(new Vector2(10f, 20f), fixture.Target.anchoredPosition, "Draggable should restore the original anchored position when ReturnToOrigin is enabled.");

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void DragEventsInvokeRegisteredListeners()
        {
            var fixture = CreateDraggable();
            var draggable = fixture.Draggable;
            var beginDragCount = 0;
            var dragCount = 0;
            var endDragCount = 0;
            draggable.onBeginDrag = new Draggable.DragEvent();
            draggable.onDrag = new Draggable.DragEvent();
            draggable.onEndDrag = new Draggable.DragEvent();

            draggable.onBeginDrag.AddListener(_ => beginDragCount++);
            draggable.onDrag.AddListener(_ => dragCount++);
            draggable.onEndDrag.AddListener(_ => endDragCount++);

            draggable.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left));
            draggable.OnDrag(CreatePointerEventData(PointerEventData.InputButton.Left, new Vector2(1f, 1f)));
            draggable.OnEndDrag(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.AreEqual(1, beginDragCount, "Draggable.onBeginDrag should invoke a registered listener exactly once.");
            Assert.AreEqual(1, dragCount, "Draggable.onDrag should invoke a registered listener exactly once.");
            Assert.AreEqual(1, endDragCount, "Draggable.onEndDrag should invoke a registered listener exactly once.");

            Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void CanBeInstantiatedFromPrefab()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            Assert.IsNotNull(prefab, $"Expected to load prefab at path: {PrefabPath}");

            var instance = Object.Instantiate(prefab);
            var draggable = instance.GetComponent<Draggable>();
            Assert.IsNotNull(draggable, $"Expected root object of {PrefabPath} to contain {typeof(Draggable).FullName}.");

            Object.DestroyImmediate(instance);
        }

        private static DraggableFixture CreateDraggable()
        {
            var root = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas));
            var canvas = root.GetComponent<Canvas>();
            canvas.scaleFactor = 1f;

            var targetObject = new GameObject("Draggable", typeof(RectTransform));
            targetObject.transform.SetParent(root.transform, false);

            var target = targetObject.GetComponent<RectTransform>();
            var draggable = targetObject.AddComponent<Draggable>();

            return new DraggableFixture(root, canvas, target, draggable);
        }

        private static PointerEventData CreatePointerEventData(PointerEventData.InputButton button, Vector2 delta = default)
        {
            return new PointerEventData(null)
            {
                button = button,
                delta = delta
            };
        }

        private readonly struct DraggableFixture
        {
            public DraggableFixture(GameObject root, Canvas canvas, RectTransform target, Draggable draggable)
            {
                Root = root;
                Canvas = canvas;
                Target = target;
                Draggable = draggable;
            }

            public GameObject Root { get; }
            public Canvas Canvas { get; }
            public RectTransform Target { get; }
            public Draggable Draggable { get; }
        }
    }
}
