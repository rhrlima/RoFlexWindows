using NUnit.Framework;
using RO_Flex_UI.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Tests
{
    public class FlexPanelTests
    {
        [Test]
        public void EnablingPanelDoesNotOverwriteLayoutGroupSpacing()
        {
            var root = new GameObject("Flex Panel", typeof(RectTransform), typeof(HorizontalLayoutGroup));
            var layoutGroup = root.GetComponent<HorizontalLayoutGroup>();
            layoutGroup.spacing = 12f;

            var child = new GameObject("Child", typeof(RectTransform));
            child.transform.SetParent(root.transform, false);

            root.AddComponent<FlexPanel>();

            Assert.AreEqual(12f, layoutGroup.spacing);

            Object.DestroyImmediate(root);
        }
    }
}