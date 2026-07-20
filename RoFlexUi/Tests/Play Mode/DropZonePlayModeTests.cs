// using System.Collections;
// using System.Reflection;
// using NUnit.Framework;
// using RO_Flex_UI.Components;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.TestTools;
// using UnityEngine.UI;

// namespace RO_Flex_UI.Tests
// {
//     public class DropZonePlayModeTests
//     {
//         [UnityTest]
//         public IEnumerator DropHandlerResolvesItemBeforeEndDrag()
//         {
//             var canvasObject = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas));
//             var eventSystemObject = new GameObject("EventSystem", typeof(EventSystem));
//             var source = CreateIconAmount("Source", canvasObject.transform, out var sourceImage);
//             var proxy = CreateIconAmount("Proxy", canvasObject.transform, out var proxyImage);
//             var item = source.gameObject.AddComponent<DraggableItem>();
//             SetField(item, "target", proxy);
//             item.Configure("potion", source, "1");

//             var zoneObject = new GameObject("Drop Zone", typeof(RectTransform));
//             zoneObject.transform.SetParent(canvasObject.transform, false);
//             var zone = zoneObject.AddComponent<DropZone>();
//             var accepted = 0;
//             item.onDropAccepted.AddListener(_ => accepted++);

//             yield return null;

//             var beginEvent = new PointerEventData(EventSystem.current)
//             {
//                 button = PointerEventData.InputButton.Left,
//                 position = new Vector2(20f, 20f),
//             };
//             item.OnBeginDrag(beginEvent);

//             var dropEvent = new PointerEventData(EventSystem.current)
//             {
//                 pointerDrag = item.gameObject,
//             };
//             zone.OnDrop(dropEvent);

//             Assert.AreEqual(1, accepted);
//             Assert.IsTrue(sourceImage.gameObject.activeSelf);
//             Assert.IsFalse(proxyImage.gameObject.activeSelf);

//             item.OnEndDrag(beginEvent);
//             Assert.IsFalse(item.Dragging);

//             Object.Destroy(canvasObject);
//             Object.Destroy(eventSystemObject);
//             yield return null;
//         }

//         private static IconAmount CreateIconAmount(string name, Transform parent, out Image image)
//         {
//             var root = new GameObject(name, typeof(RectTransform), typeof(IconAmount));
//             root.transform.SetParent(parent, false);

//             var imageObject = new GameObject("Icon", typeof(RectTransform), typeof(Image));
//             imageObject.transform.SetParent(root.transform, false);
//             image = imageObject.GetComponent<Image>();

//             var textObject = new GameObject("Amount", typeof(RectTransform));
//             textObject.transform.SetParent(root.transform, false);
//             var textType = System.Type.GetType("TMPro.TextMeshProUGUI, Unity.TextMeshPro", true);
//             var text = textObject.AddComponent(textType);

//             var iconAmount = root.GetComponent<IconAmount>();
//             SetField(iconAmount, "iconSprite", image);
//             SetField(iconAmount, "iconText", text);
//             return iconAmount;
//         }

//         private static void SetField(object target, string fieldName, object value)
//         {
//             target.GetType()
//                 .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
//                 .SetValue(target, value);
//         }
//     }
// }
