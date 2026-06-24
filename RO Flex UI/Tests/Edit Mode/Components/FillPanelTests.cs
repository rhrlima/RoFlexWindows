using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace RO_Flex_UI.Tests
{
    public class FillPanel2Tests
    {
        private static readonly Type PanelType = Type.GetType("FillPanel, Assembly-CSharp");

        [Test]
        public void RefreshCalculatesBaseGridFromViewport()
        {
            var fixture = CreateFixture(new Vector2(50f, 50f));

            Invoke(fixture.Panel, "Refresh");

            Assert.AreEqual(5, GetIntProperty(fixture.Panel, "Columns"));
            Assert.AreEqual(5, GetIntProperty(fixture.Panel, "Rows"));
            Assert.AreEqual(25, GetIntProperty(fixture.Panel, "TotalCells"));
            Assert.AreEqual(new Vector2(50f, 50f), fixture.RootRect.sizeDelta);
            Assert.AreEqual(0, GetIntProperty(fixture.Panel, "FilledCells"));
            Assert.AreEqual(25, GetIntProperty(fixture.Panel, "EmptyCells"));
            Assert.AreEqual(25, GetCells(fixture.Panel).Count);

            UnityEngine.Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void RefreshDoesNotStretchWhenOverflowFitsBaseGrid()
        {
            var fixture = CreateFixture(new Vector2(50f, 50f));

            Invoke(fixture.Panel, "SetFilledCells", 10);
            Invoke(fixture.Panel, "Refresh");

            Assert.AreEqual(5, GetIntProperty(fixture.Panel, "Columns"));
            Assert.AreEqual(5, GetIntProperty(fixture.Panel, "Rows"));
            Assert.AreEqual(25, GetIntProperty(fixture.Panel, "TotalCells"));
            Assert.AreEqual(10, GetIntProperty(fixture.Panel, "FilledCells"));
            Assert.AreEqual(15, GetIntProperty(fixture.Panel, "EmptyCells"));
            Assert.AreEqual(new Vector2(50f, 50f), fixture.RootRect.sizeDelta);

            UnityEngine.Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void RefreshRoundsOverflowUpToFullGrid()
        {
            var fixture = CreateFixture(new Vector2(50f, 50f));

            Invoke(fixture.Panel, "SetFilledCells", 26);
            Invoke(fixture.Panel, "Refresh");

            Assert.AreEqual(30, GetIntProperty(fixture.Panel, "TotalCells"));
            Assert.AreEqual(26, GetIntProperty(fixture.Panel, "FilledCells"));
            Assert.AreEqual(4, GetIntProperty(fixture.Panel, "EmptyCells"));
            Assert.AreEqual(new Vector2(50f, 50f), fixture.RootRect.sizeDelta);

            UnityEngine.Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void RefreshRecalculatesAfterResize()
        {
            var fixture = CreateFixture(new Vector2(50f, 50f));
            Invoke(fixture.Panel, "Refresh");

            fixture.RootRect.sizeDelta = new Vector2(70f, 30f);
            Invoke(fixture.Panel, "Refresh");

            Assert.AreEqual(7, GetIntProperty(fixture.Panel, "Columns"));
            Assert.AreEqual(3, GetIntProperty(fixture.Panel, "Rows"));
            Assert.AreEqual(21, GetIntProperty(fixture.Panel, "TotalCells"));

            UnityEngine.Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void SetFilledCellsClampsNegativeValues()
        {
            var fixture = CreateFixture(new Vector2(50f, 50f));

            Invoke(fixture.Panel, "SetFilledCells", -5);

            Assert.AreEqual(0, GetIntProperty(fixture.Panel, "FilledCells"));

            UnityEngine.Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void RefreshLimitsPoolToMaxSlots()
        {
            var fixture = CreateFixture(new Vector2(50f, 50f));
            SetInt(fixture.Panel, "maxSlots", 12);

            Invoke(fixture.Panel, "SetFilledCells", 26);
            Invoke(fixture.Panel, "Refresh");

            Assert.AreEqual(30, GetIntProperty(fixture.Panel, "TotalCells"));
            Assert.AreEqual(12, GetCells(fixture.Panel).Count);

            UnityEngine.Object.DestroyImmediate(fixture.Root);
        }

        [Test]
        public void EnsureReferencesFailsWhenTemplateIsMissing()
        {
            Assert.IsNotNull(PanelType, "FillPanel2 type was not found in Assembly-CSharp.");

            var root = new GameObject("Fill Panel", typeof(RectTransform), typeof(GridLayoutGroup));
            var panel = root.AddComponent(PanelType);

            LogAssert.Expect(LogType.Error, "[Fill Panel] Missing reference: cellTemplate.");
            Assert.IsFalse((bool)Invoke(panel, "EnsureReferences"));

            UnityEngine.Object.DestroyImmediate(root);
        }

        private static PanelFixture CreateFixture(Vector2 size)
        {
            Assert.IsNotNull(PanelType, "FillPanel2 type was not found in Assembly-CSharp.");

            var root = new GameObject("Fill Panel", typeof(RectTransform), typeof(GridLayoutGroup));
            var rootRect = root.GetComponent<RectTransform>();
            rootRect.sizeDelta = size;

            var gridLayout = root.GetComponent<GridLayoutGroup>();
            gridLayout.cellSize = new Vector2(10f, 10f);
            gridLayout.spacing = Vector2.zero;
            gridLayout.padding = new RectOffset();

            var templateObject = new GameObject("Cell Template", typeof(RectTransform));
            templateObject.transform.SetParent(root.transform, false);

            var panel = root.AddComponent(PanelType);
            SetObjectReference(panel, "cellTemplate", templateObject);

            return new PanelFixture(root, rootRect, panel);
        }

        private static IList GetCells(Component panel)
        {
            return (IList)PanelType
                .GetField("cells", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(panel);
        }

        private static int GetIntProperty(Component panel, string propertyName)
        {
            return (int)PanelType
                .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)
                .GetValue(panel);
        }

        private static object Invoke(Component panel, string methodName, params object[] parameters)
        {
            return PanelType
                .GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Invoke(panel, parameters);
        }

        private static void SetObjectReference(Component panel, string propertyName, UnityEngine.Object value)
        {
            var serializedPanel = new SerializedObject(panel);
            serializedPanel.FindProperty(propertyName).objectReferenceValue = value;
            serializedPanel.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetInt(Component panel, string propertyName, int value)
        {
            var serializedPanel = new SerializedObject(panel);
            serializedPanel.FindProperty(propertyName).intValue = value;
            serializedPanel.ApplyModifiedPropertiesWithoutUndo();
        }

        private readonly struct PanelFixture
        {
            public PanelFixture(GameObject root, RectTransform rootRect, Component panel)
            {
                Root = root;
                RootRect = rootRect;
                Panel = panel;
            }

            public GameObject Root { get; }
            public RectTransform RootRect { get; }
            public Component Panel { get; }
        }
    }
}
