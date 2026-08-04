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
            var payload = new DragPayload(null, new Vector2(2f, 3f), data, source, null);

            Assert.IsTrue(payload.TryGetData<object>(out var typedData));
            Assert.AreSame(data, typedData);
            Assert.IsFalse(payload.TryGetData<string>(out _));
            Assert.IsTrue(payload.TryGetContext<string>(out var typedSource));
            Assert.AreEqual(source, typedSource);
            Assert.IsFalse(payload.TryGetContext<int>(out _));
        }

        [Test]
        public void PayloadProvidesBoxedValueTypeData()
        {
            var payload = new DragPayload(null, Vector2.zero, 7, null, null);

            Assert.IsTrue(payload.TryGetData<int>(out var value));
            Assert.AreEqual(7, value);
            Assert.IsFalse(payload.TryGetData<string>(out _));
        }

        [Test]
        public void ItemExampleProvidesNameSpriteAndAmountPresentation()
        {
            var itemType = System.Type.GetType("ItemExample, Assembly-CSharp", true);
            var itemObject = new GameObject("Item Example");
            var item = itemObject.AddComponent(itemType);
            var texture = new Texture2D(1, 1);
            var sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.zero);
            var serializedItem = new SerializedObject(item);
            var itemData = serializedItem.FindProperty("item");
            itemData.FindPropertyRelative("name").stringValue = "Red Potion";
            itemData.FindPropertyRelative("sprite").objectReferenceValue = sprite;
            itemData.FindPropertyRelative("amount").intValue = 2;
            serializedItem.ApplyModifiedPropertiesWithoutUndo();

            var presentation = (DragPresentation)itemType
                .GetProperty("Presentation")
                .GetValue(item);

            Assert.AreSame(sprite, presentation.Sprite);
            Assert.AreEqual("2", presentation.Amount);
            Assert.AreEqual("Red Potion", presentation.Text);

            Object.DestroyImmediate(itemObject);
            Object.DestroyImmediate(sprite);
            Object.DestroyImmediate(texture);
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
            Assert.IsFalse(fixture.SourceImage.gameObject.activeSelf);
            Assert.IsFalse(fixture.ProxyImage.gameObject.activeSelf);

            Object.DestroyImmediate(zoneObject);
            fixture.Dispose();
        }

        [Test]
        public void DropZoneResolvesIDraggableWithoutDraggableItem()
        {
            var dragObject = new GameObject("Fake Draggable", typeof(RectTransform));
            var fakeDraggable = dragObject.AddComponent<FakeDraggable>();
            var zoneObject = new GameObject("Zone", typeof(RectTransform));
            var zone = zoneObject.AddComponent<DropZone>();
            var accepted = 0;
            MonoBehaviour notifiedDraggable = null;
            zone.onDropAccepted.AddListener(value =>
            {
                accepted++;
                notifiedDraggable = value;
            });

            fakeDraggable.BeginDrag(new DragPayload(null, Vector2.zero, "skill", "skill-bar", null));

            zone.OnDrop(CreateDropEventData(dragObject));

            Assert.IsTrue(fakeDraggable.TryDropCalled);
            Assert.IsFalse(fakeDraggable.CanResolveDrop);
            Assert.IsFalse(fakeDraggable.Dragging);
            Assert.AreEqual(1, accepted);
            Assert.AreSame(fakeDraggable, notifiedDraggable);

            Object.DestroyImmediate(zoneObject);
            Object.DestroyImmediate(dragObject);
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

            iconAmount.Assign(sprite, "2");

            Assert.IsTrue(image.gameObject.activeSelf);
            Assert.IsTrue(amountObject.activeSelf);

            iconAmount.Clear();

            Assert.IsFalse(iconAmount.IsVisible);
            Assert.IsNull(iconAmount.Sprite);
            Assert.AreEqual(string.Empty, iconAmount.Amount);
            Assert.IsFalse(image.gameObject.activeSelf);
            Assert.IsFalse(amountObject.activeSelf);

            Object.DestroyImmediate(root);
            Object.DestroyImmediate(sprite);
            Object.DestroyImmediate(texture);
        }

        [Test]
        public void IconTextCapturesAndAppliesSpriteAmountAndText()
        {
            var root = new GameObject("Root", typeof(RectTransform));
            var source = CreateIconText("Source", root.transform);
            var proxy = CreateIconText("Proxy", root.transform);
            var texture = new Texture2D(1, 1);
            var sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.zero);
            source.Assign(sprite, "Sword", "2");

            Assert.IsTrue(proxy.TryApplyPresentation(source.CapturePresentation()));
            Assert.AreSame(sprite, proxy.Sprite);
            Assert.AreEqual("2", proxy.Amount);
            Assert.AreEqual("Sword", proxy.Text);

            Object.DestroyImmediate(root);
            Object.DestroyImmediate(sprite);
            Object.DestroyImmediate(texture);
        }

        [Test]
        public void IconAmountAndIconTextApplyTheSamePayloadPresentation()
        {
            var root = new GameObject("Root", typeof(RectTransform));
            var iconAmount = CreateIconAmount("Amount", root.transform, out _);
            var iconText = CreateIconText("Text", root.transform);
            var texture = new Texture2D(1, 1);
            var sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.zero);
            var presentation = new DragPresentation(sprite, "4", "Potion");
            var payload = new DragPayload(null, Vector2.zero, "item", null, iconAmount, presentation);

            Assert.IsTrue(iconAmount.TryApplyPresentation(payload.Presentation));
            Assert.IsTrue(iconText.TryApplyPresentation(payload.Presentation));
            Assert.AreSame(sprite, iconAmount.Sprite);
            Assert.AreEqual("4", iconAmount.Amount);
            Assert.AreSame(sprite, iconText.Sprite);
            Assert.AreEqual("4", iconText.Amount);
            Assert.AreEqual("Potion", iconText.Text);

            Object.DestroyImmediate(root);
            Object.DestroyImmediate(sprite);
            Object.DestroyImmediate(texture);
        }

        [Test]
        public void TypedDropZoneRoutesOnlyMatchingNonNullData()
        {
            var zoneObject = new GameObject("Typed Zone", typeof(RectTransform));
            var zone = zoneObject.AddComponent<StringDropZone>();
            var valid = new DragPayload(null, Vector2.zero, "potion", null, null);
            var wrong = new DragPayload(null, Vector2.zero, 3, null, null);
            var empty = new DragPayload(null, Vector2.zero, null, null, null);

            Assert.IsTrue(zone.CanDrop(valid));
            Assert.AreEqual(DropResult.Copy.Accepted, zone.Drop(valid).Accepted);
            Assert.AreEqual("potion", zone.LastData);
            Assert.IsFalse(zone.CanDrop(wrong));
            Assert.IsFalse(zone.Drop(wrong).Accepted);
            Assert.IsFalse(zone.CanDrop(empty));
            Assert.IsFalse(zone.Drop(empty).Accepted);

            Object.DestroyImmediate(zoneObject);
        }

        private static DragFixture CreateFixture()
        {
            var canvasObject = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas));
            var source = CreateIconAmount("Source", canvasObject.transform, out var sourceImage);
            var proxy = CreateIconAmount("Proxy", canvasObject.transform, out var proxyImage);
            var item = source.gameObject.AddComponent<DraggableItem>();
            var serializedItem = new SerializedObject(item);
            serializedItem.FindProperty("proxyVisualComponent").objectReferenceValue = proxy;
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

        private static PointerEventData CreateDropEventData(DraggableItem item)
        {
            return CreateDropEventData(item.gameObject);
        }

        private static PointerEventData CreateDropEventData(GameObject pointerDrag)
        {
            return new PointerEventData(null)
            {
                pointerDrag = pointerDrag,
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
            public override DropResult Drop(DragPayload payload)
            {
                return DropResult.Rejected;
            }
        }

        private sealed class StringDropZone : DropZone<string>
        {
            public string LastData { get; private set; }

            protected override DropResult Drop(string data, DragPayload payload)
            {
                LastData = data;
                return DropResult.Copy;
            }
        }

        private sealed class FakeDraggable : MonoBehaviour, IDraggable
        {
            private DragPayload currentPayload;
            private bool dragging;
            private bool dropResolved;

            public bool TryDropCalled { get; private set; }
            public bool Dragging => dragging;
            public bool CanResolveDrop => dragging && !dropResolved;
            public DragPayload CurrentPayload => currentPayload;

            public void Configure(
                object data,
                object context = null,
                DragPresentation? presentation = null)
            {
                currentPayload = new DragPayload(
                    this,
                    Vector2.zero,
                    data,
                    context,
                    null,
                    presentation ?? default);
            }

            public void BeginDrag(DragPayload payload)
            {
                currentPayload = payload;
                dragging = true;
                dropResolved = false;
                TryDropCalled = false;
            }

            public bool TryDrop(IDropZone dropZone)
            {
                TryDropCalled = true;

                if (!CanResolveDrop || dropZone == null)
                    return false;

                var accepted = dropZone.CanDrop(currentPayload)
                    && dropZone.Drop(currentPayload).Accepted;
                dropResolved = true;
                dragging = false;
                return accepted;
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
