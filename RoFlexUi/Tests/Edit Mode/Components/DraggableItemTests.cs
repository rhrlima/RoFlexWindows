using NUnit.Framework;
using RO_Flex_UI.Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RO_Flex_UI.Tests
{
    public class DraggableItemTests
    {
        [Test]
        public void EnsureReferencesReturnsFalseWhenProxyMissing()
        {
            var fixture = CreateFixture(assignProxy: false);

            Assert.IsFalse(fixture.Item.EnsureReferences());

            Assert.DoesNotThrow(() => fixture.Item.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left)));
            Assert.IsFalse(fixture.Item.Dragging);
            Assert.IsTrue(fixture.SourceImage.gameObject.activeSelf);

            fixture.Dispose();
        }

        [Test]
        public void SuccessfulDropClearsSourceAndEmitsAccepted()
        {
            var fixture = CreateFixture();
            var dropZone = new AcceptingDropZone();
            var accepted = 0;
            var rejected = 0;
            var ended = 0;
            fixture.Item.onDropAccepted.AddListener(_ => accepted++);
            fixture.Item.onDropRejected.AddListener(_ => rejected++);
            fixture.Item.onEndDrag.AddListener(_ => ended++);

            fixture.Item.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left, new Vector2(20f, 10f)));

            Assert.IsTrue(fixture.Item.Dragging);
            Assert.IsTrue(fixture.Item.CanResolveDrop);
            Assert.IsFalse(fixture.SourceImage.gameObject.activeSelf);
            Assert.IsTrue(fixture.ProxyImage.gameObject.activeSelf);

            Assert.IsTrue(fixture.Item.TryDrop(dropZone));

            Assert.AreEqual(1, accepted);
            Assert.AreEqual(0, rejected);
            Assert.IsFalse(fixture.Item.CanResolveDrop);
            Assert.IsFalse(fixture.SourceImage.gameObject.activeSelf);
            Assert.IsFalse(fixture.ProxyImage.gameObject.activeSelf);
            Assert.IsNull(fixture.SourceImage.sprite);

            fixture.Item.OnEndDrag(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.IsFalse(fixture.Item.Dragging);
            Assert.AreEqual(1, accepted);
            Assert.AreEqual(0, rejected);
            Assert.AreEqual(1, ended);

            fixture.Dispose();
        }

        [Test]
        public void UnsuccessfulDropRestoresSourceAndEmitsRejected()
        {
            var fixture = CreateFixture();
            var dropZone = new RejectingDropZone();
            var accepted = 0;
            var rejected = 0;
            fixture.Item.onDropAccepted.AddListener(_ => accepted++);
            fixture.Item.onDropRejected.AddListener(_ => rejected++);

            fixture.Item.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left, new Vector2(10f, 5f)));
            var origin = fixture.Item.CurrentPayload.OriginPosition;
            fixture.Item.OnDrag(CreatePointerEventData(PointerEventData.InputButton.Left, new Vector2(90f, 45f)));

            Assert.IsFalse(fixture.Item.TryDrop(dropZone));

            Assert.AreEqual(0, accepted);
            Assert.AreEqual(1, rejected);
            Assert.IsFalse(fixture.Item.CanResolveDrop);
            Assert.AreEqual(origin, fixture.ProxyRect.anchoredPosition);
            Assert.IsTrue(fixture.SourceImage.gameObject.activeSelf);
            Assert.IsFalse(fixture.ProxyImage.gameObject.activeSelf);

            fixture.Dispose();
        }

        [Test]
        public void CopyDropRestoresSourceWithoutClearingPresentation()
        {
            var fixture = CreateFixture();

            fixture.Item.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.IsTrue(fixture.Item.TryDrop(new CopyingDropZone()));
            Assert.IsTrue(fixture.SourceImage.gameObject.activeSelf);
            Assert.AreSame(fixture.Sprite, fixture.SourceImage.sprite);
            Assert.IsFalse(fixture.ProxyImage.gameObject.activeSelf);

            fixture.Dispose();
        }

        [Test]
        public void SwapDropCanReplaceSourcePresentationAndDataBeforeRestore()
        {
            var fixture = CreateFixture();
            var texture = new Texture2D(1, 1);
            var replacement = Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.zero);

            fixture.Item.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.IsTrue(fixture.Item.TryDrop(new SwappingDropZone(replacement)));
            Assert.IsTrue(fixture.SourceImage.gameObject.activeSelf);
            Assert.AreSame(replacement, fixture.SourceImage.sprite);

            fixture.Item.OnEndDrag(CreatePointerEventData(PointerEventData.InputButton.Left));
            fixture.Item.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.AreEqual("shield", fixture.Item.CurrentPayload.Data);
            fixture.Item.OnEndDrag(CreatePointerEventData(PointerEventData.InputButton.Left));

            fixture.Dispose();
            Object.DestroyImmediate(replacement);
            Object.DestroyImmediate(texture);
        }

        [Test]
        public void IncompatibleProxyDoesNotStartDragOrHideSource()
        {
            var canvasObject = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas));
            var source = CreateIconAmount("Source", canvasObject.transform, out var sourceImage);
            var proxyObject = new GameObject("Proxy", typeof(RectTransform), typeof(IncompatibleVisual));
            proxyObject.transform.SetParent(canvasObject.transform, false);
            var proxy = proxyObject.GetComponent<IncompatibleVisual>();
            var item = source.gameObject.AddComponent<DraggableItem>();
            var serializedItem = new SerializedObject(item);
            serializedItem.FindProperty("proxyVisualComponent").objectReferenceValue = proxy;
            serializedItem.ApplyModifiedPropertiesWithoutUndo();
            source.Assign(null, "1");
            source.SetActive(true);

            item.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.IsFalse(item.Dragging);
            Assert.IsTrue(sourceImage.gameObject.activeSelf);
            Assert.IsFalse(proxy.Active);

            Object.DestroyImmediate(canvasObject);
        }

        [Test]
        public void IconAmountSourceCanPopulateIconTextProxyFromConfiguredPresentation()
        {
            var canvasObject = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas));
            var source = CreateIconAmount("Source", canvasObject.transform, out _);
            var proxy = CreateIconText("Proxy", canvasObject.transform);
            var item = source.gameObject.AddComponent<DraggableItem>();
            var texture = new Texture2D(1, 1);
            var sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.zero);
            var serializedItem = new SerializedObject(item);
            serializedItem.FindProperty("proxyVisualComponent").objectReferenceValue = proxy;
            serializedItem.ApplyModifiedPropertiesWithoutUndo();
            source.Assign(sprite, "3");
            source.SetActive(true);
            item.Configure(
                "potion",
                source,
                new DragPresentation(sprite, "3", "Red Potion"));

            item.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.IsTrue(item.Dragging);
            Assert.AreSame(sprite, proxy.Sprite);
            Assert.AreEqual("3", proxy.Amount);
            Assert.AreEqual("Red Potion", proxy.Text);

            item.OnEndDrag(CreatePointerEventData(PointerEventData.InputButton.Left));
            Object.DestroyImmediate(canvasObject);
            Object.DestroyImmediate(sprite);
            Object.DestroyImmediate(texture);
        }

        [Test]
        public void ReleaseDuringActiveDragRejectsUnresolvedDrop()
        {
            var fixture = CreateFixture();
            var accepted = 0;
            var rejected = 0;
            var ended = 0;
            fixture.Item.onDropAccepted.AddListener(_ => accepted++);
            fixture.Item.onDropRejected.AddListener(_ => rejected++);
            fixture.Item.onEndDrag.AddListener(_ => ended++);

            fixture.Item.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left, new Vector2(20f, 10f)));
            fixture.Item.OnEndDrag(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.IsFalse(fixture.Item.Dragging);
            Assert.IsFalse(fixture.Item.CanResolveDrop);
            Assert.AreEqual(0, accepted);
            Assert.AreEqual(1, rejected);
            Assert.AreEqual(1, ended);
            Assert.IsTrue(fixture.SourceImage.gameObject.activeSelf);
            Assert.IsFalse(fixture.ProxyImage.gameObject.activeSelf);

            fixture.Dispose();
        }

        private static DragFixture CreateFixture(bool assignProxy = true)
        {
            var canvasObject = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas));
            var source = CreateIconAmount("Source", canvasObject.transform, out var sourceImage);
            var proxy = CreateIconAmount("Proxy", canvasObject.transform, out var proxyImage);
            var item = source.gameObject.AddComponent<DraggableItem>();
            var texture = new Texture2D(1, 1);
            var sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.zero);

            if (assignProxy)
            {
                var serializedItem = new SerializedObject(item);
                serializedItem.FindProperty("proxyVisualComponent").objectReferenceValue = proxy;
                serializedItem.ApplyModifiedPropertiesWithoutUndo();
            }

            source.Assign(sprite, "3");
            source.SetActive(true);
            proxy.SetActive(false);
            item.Configure("potion", source);

            return new DragFixture(
                canvasObject,
                item,
                sourceImage,
                proxyImage,
                proxy.transform as RectTransform,
                sprite,
                texture);
        }

        private static IconAmount CreateIconAmount(string name, Transform parent, out Image image)
        {
            var root = new GameObject(name, typeof(RectTransform), typeof(IconAmount));
            root.transform.SetParent(parent, false);

            var imageObject = new GameObject("Icon", typeof(RectTransform), typeof(Image));
            imageObject.transform.SetParent(root.transform, false);
            image = imageObject.GetComponent<Image>();

            var textObject = new GameObject("Amount", typeof(RectTransform));
            textObject.transform.SetParent(root.transform, false);
            var text = textObject.AddComponent(GetTextComponentType());

            var iconAmount = root.GetComponent<IconAmount>();
            var serializedIcon = new SerializedObject(iconAmount);
            serializedIcon.FindProperty("iconSprite").objectReferenceValue = image;
            serializedIcon.FindProperty("iconAmount").objectReferenceValue = text;
            serializedIcon.ApplyModifiedPropertiesWithoutUndo();
            return iconAmount;
        }

        private static IconText CreateIconText(string name, Transform parent)
        {
            var root = new GameObject(
                name,
                typeof(RectTransform),
                typeof(HorizontalLayoutGroup),
                typeof(IconText));
            root.transform.SetParent(parent, false);

            var imageObject = new GameObject("Icon", typeof(RectTransform), typeof(Image));
            imageObject.transform.SetParent(root.transform, false);
            var image = imageObject.GetComponent<Image>();

            var amountObject = new GameObject("Amount", typeof(RectTransform));
            amountObject.transform.SetParent(root.transform, false);
            var amount = amountObject.AddComponent(GetTextComponentType());

            var textObject = new GameObject("Text", typeof(RectTransform));
            textObject.transform.SetParent(root.transform, false);
            var text = textObject.AddComponent(GetTextComponentType());

            var iconText = root.GetComponent<IconText>();
            var serializedIcon = new SerializedObject(iconText);
            serializedIcon.FindProperty("iconSprite").objectReferenceValue = image;
            serializedIcon.FindProperty("iconAmount").objectReferenceValue = amount;
            serializedIcon.FindProperty("iconText").objectReferenceValue = text;
            serializedIcon.ApplyModifiedPropertiesWithoutUndo();
            return iconText;
        }

        private static System.Type GetTextComponentType()
        {
            return System.Type.GetType("TMPro.TextMeshProUGUI, Unity.TextMeshPro", true);
        }

        private static PointerEventData CreatePointerEventData(
            PointerEventData.InputButton button,
            Vector2 position = default)
        {
            return new PointerEventData(null)
            {
                button = button,
                position = position,
            };
        }

        private sealed class AcceptingDropZone : IDropZone
        {
            public bool CanDrop(DragPayload payload) => true;
            public DropResult Drop(DragPayload payload) => DropResult.Move;
        }

        private sealed class RejectingDropZone : IDropZone
        {
            public bool CanDrop(DragPayload payload) => false;
            public DropResult Drop(DragPayload payload) => DropResult.Move;
        }

        private sealed class CopyingDropZone : IDropZone
        {
            public bool CanDrop(DragPayload payload) => true;
            public DropResult Drop(DragPayload payload) => DropResult.Copy;
        }

        private sealed class SwappingDropZone : IDropZone
        {
            private readonly Sprite replacement;

            public SwappingDropZone(Sprite replacement)
            {
                this.replacement = replacement;
            }

            public bool CanDrop(DragPayload payload) => true;

            public DropResult Drop(DragPayload payload)
            {
                var source = (IconAmount)payload.SourceVisual;
                source.Assign(replacement, "1");
                payload.Draggable.Configure("shield", payload.Context);
                return DropResult.Swap;
            }
        }

        private sealed class IncompatibleVisual : MonoBehaviour, IDragVisual
        {
            public RectTransform RectTransform => transform as RectTransform;
            public bool IsVisible => true;
            public bool Active { get; private set; }
            public bool EnsureReferences() => RectTransform != null;
            public DragPresentation CapturePresentation() => default;
            public bool TryApplyPresentation(DragPresentation presentation) => false;
            public void SetActive(bool value) => Active = value;
            public void Clear() { }
        }

        private readonly struct DragFixture
        {
            public DragFixture(
                GameObject root,
                DraggableItem item,
                Image sourceImage,
                Image proxyImage,
                RectTransform proxyRect,
                Sprite sprite,
                Texture2D texture)
            {
                Root = root;
                Item = item;
                SourceImage = sourceImage;
                ProxyImage = proxyImage;
                ProxyRect = proxyRect;
                Sprite = sprite;
                Texture = texture;
            }

            public GameObject Root { get; }
            public DraggableItem Item { get; }
            public Image SourceImage { get; }
            public Image ProxyImage { get; }
            public RectTransform ProxyRect { get; }
            public Sprite Sprite { get; }
            private Texture2D Texture { get; }

            public void Dispose()
            {
                Object.DestroyImmediate(Root);
                Object.DestroyImmediate(Sprite);
                Object.DestroyImmediate(Texture);
            }
        }
    }
}
