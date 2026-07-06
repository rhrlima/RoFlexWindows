using NUnit.Framework;
using RO_Flex_UI.Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RO_Flex_UI.Tests
{
    public class DropZoneTests
    {
        [Test]
        public void PayloadProvidesTypedDataAndSource()
        {
            var data = new object();
            var source = "inventory";
            var payload = new DragPayload(null, new Vector2(2f, 3f), data, source, null, "4");

            Assert.IsTrue(payload.TryGetData<object>(out var typedData));
            Assert.AreSame(data, typedData);
            Assert.IsFalse(payload.TryGetData<string>(out _));
            Assert.IsTrue(payload.TryGetSource<string>(out var typedSource));
            Assert.AreEqual(source, typedSource);
            Assert.AreEqual(4, payload.Amount);
        }

        [Test]
        public void AcceptedDropInvokesAcceptedEventsOnce()
        {
            var fixture = CreateFixture();
            var zoneObject = new GameObject("Accepted Zone", typeof(RectTransform));
            var zone = zoneObject.AddComponent<DropZone>();
            var zoneAccepted = 0;
            var itemAccepted = 0;
            var itemRejected = 0;
            zone.onDropAccepted.AddListener(_ => zoneAccepted++);
            fixture.Item.onDropAccepted.AddListener(_ => itemAccepted++);
            fixture.Item.onDropRejected.AddListener(_ => itemRejected++);

            fixture.Item.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left, new Vector2(40f, 30f)));
            zone.OnDrop(CreateDropEventData(fixture.Item));
            zone.OnDrop(CreateDropEventData(fixture.Item));
            fixture.Item.OnEndDrag(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.AreEqual(1, zoneAccepted);
            Assert.AreEqual(1, itemAccepted);
            Assert.AreEqual(0, itemRejected);
            Assert.IsFalse(fixture.Item.Dragging);
            Assert.IsTrue(fixture.SourceImage.gameObject.activeSelf);
            Assert.IsFalse(fixture.ProxyImage.gameObject.activeSelf);

            Object.DestroyImmediate(zoneObject);
            fixture.Dispose();
        }

        [Test]
        public void RejectedDropReturnsProxyToOrigin()
        {
            var fixture = CreateFixture();
            var zoneObject = new GameObject("Rejected Zone", typeof(RectTransform));
            var zone = zoneObject.AddComponent<RejectingDropZone>();
            var zoneRejected = 0;
            var itemRejected = 0;
            zone.onDropRejected.AddListener(_ => zoneRejected++);
            fixture.Item.onDropRejected.AddListener(_ => itemRejected++);

            fixture.Item.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left, new Vector2(80f, 60f)));
            var origin = fixture.Item.CurrentPayload.OriginPosition;
            zone.OnDrop(CreateDropEventData(fixture.Item));
            fixture.Item.OnEndDrag(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.AreEqual(1, zoneRejected);
            Assert.AreEqual(1, itemRejected);
            Assert.AreEqual(origin, fixture.ProxyRect.anchoredPosition);
            Assert.IsTrue(fixture.SourceImage.gameObject.activeSelf);
            Assert.IsFalse(fixture.ProxyImage.gameObject.activeSelf);

            Object.DestroyImmediate(zoneObject);
            fixture.Dispose();
        }

        [Test]
        public void FailedDropIsRejected()
        {
            var fixture = CreateFixture();
            var zoneObject = new GameObject("Failing Zone", typeof(RectTransform));
            var zone = zoneObject.AddComponent<FailingDropZone>();
            var rejected = 0;
            zone.onDropRejected.AddListener(_ => rejected++);

            fixture.Item.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left));
            zone.OnDrop(CreateDropEventData(fixture.Item));
            fixture.Item.OnEndDrag(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.AreEqual(1, rejected);
            Assert.IsFalse(fixture.Item.Dragging);

            Object.DestroyImmediate(zoneObject);
            fixture.Dispose();
        }

        [Test]
        public void EndingWithoutDropZoneRejectsDrop()
        {
            var fixture = CreateFixture();
            var rejected = 0;
            fixture.Item.onDropRejected.AddListener(_ => rejected++);

            fixture.Item.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Left));
            fixture.Item.OnEndDrag(CreatePointerEventData(PointerEventData.InputButton.Left));

            Assert.AreEqual(1, rejected);
            Assert.IsFalse(fixture.Item.Dragging);
            Assert.IsTrue(fixture.SourceImage.gameObject.activeSelf);
            Assert.IsFalse(fixture.ProxyImage.gameObject.activeSelf);

            fixture.Dispose();
        }

        [Test]
        public void NonLeftButtonDoesNotStartDrag()
        {
            var fixture = CreateFixture();

            fixture.Item.OnBeginDrag(CreatePointerEventData(PointerEventData.InputButton.Right));

            Assert.IsFalse(fixture.Item.Dragging);
            Assert.IsTrue(fixture.SourceImage.gameObject.activeSelf);
            Assert.IsFalse(fixture.ProxyImage.gameObject.activeSelf);

            fixture.Dispose();
        }

        [Test]
        public void IconAmountAssignsAndClearsPresentation()
        {
            var root = new GameObject("Root", typeof(RectTransform));
            var iconAmount = CreateIconAmount("Icon Amount", root.transform, out var image);
            var amountObject = iconAmount.transform.Find("Amount").gameObject;
            var texture = new Texture2D(1, 1);
            var sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.zero);

            iconAmount.Assign(sprite, "3");

            Assert.IsTrue(iconAmount.IsVisible);
            Assert.AreSame(sprite, iconAmount.Sprite);
            Assert.AreEqual("3", iconAmount.Amount);
            Assert.IsTrue(image.gameObject.activeSelf);
            Assert.IsTrue(amountObject.activeSelf);

            iconAmount.Assign(sprite, "1");
            Assert.IsTrue(image.gameObject.activeSelf);
            Assert.IsFalse(amountObject.activeSelf);

            iconAmount.Clear();
            iconAmount.SetActive(true);

            Assert.IsNull(iconAmount.Sprite);
            Assert.IsFalse(iconAmount.IsVisible);
            Assert.AreEqual(string.Empty, iconAmount.Amount);
            Assert.IsFalse(image.gameObject.activeSelf);
            Assert.IsFalse(amountObject.activeSelf);

            Object.DestroyImmediate(root);
            Object.DestroyImmediate(sprite);
            Object.DestroyImmediate(texture);
        }

        private static DragFixture CreateFixture()
        {
            var canvasObject = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas));
            var source = CreateIconAmount("Source", canvasObject.transform, out var sourceImage);
            var proxy = CreateIconAmount("Proxy", canvasObject.transform, out var proxyImage);
            var item = source.gameObject.AddComponent<DraggableItem>();
            var serializedItem = new SerializedObject(item);
            serializedItem.FindProperty("target").objectReferenceValue = proxy;
            serializedItem.ApplyModifiedPropertiesWithoutUndo();
            item.Configure("potion", source);
            Assert.IsTrue(item.EnsureReferences());
            proxy.SetActive(false);

            return new DragFixture(
                canvasObject,
                item,
                sourceImage,
                proxyImage,
                proxy.transform as RectTransform);
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
            serializedIcon.FindProperty("iconText").objectReferenceValue = text;
            serializedIcon.ApplyModifiedPropertiesWithoutUndo();
            return iconAmount;
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

        private static PointerEventData CreateDropEventData(DraggableItem item)
        {
            return new PointerEventData(null)
            {
                pointerDrag = item.gameObject,
            };
        }

        private sealed class RejectingDropZone : DropZone
        {
            public override bool CanDrop(DragPayload payload)
            {
                return false;
            }
        }

        private sealed class FailingDropZone : DropZone
        {
            public override bool Drop(DragPayload payload)
            {
                return false;
            }
        }

        private readonly struct DragFixture
        {
            public DragFixture(
                GameObject root,
                DraggableItem item,
                Image sourceImage,
                Image proxyImage,
                RectTransform proxyRect)
            {
                Root = root;
                Item = item;
                SourceImage = sourceImage;
                ProxyImage = proxyImage;
                ProxyRect = proxyRect;
            }

            public GameObject Root { get; }
            public DraggableItem Item { get; }
            public Image SourceImage { get; }
            public Image ProxyImage { get; }
            public RectTransform ProxyRect { get; }

            public void Dispose()
            {
                Object.DestroyImmediate(Root);
            }
        }
    }
}
