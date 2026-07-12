using RO_Flex_UI.Components;
using NUnit.Framework;
using UnityEngine;

namespace RO_Flex_UI.Tests
{
    public class RoTextTests
    {
        private const string OutlineColorProperty = "_OutlineColor";

        private Shader outlineShader;

        [SetUp]
        public void SetUp()
        {
            outlineShader = Shader.Find("ROFlexUI/Fonts/Pixel Outline");
        }

        [Test]
        public void OutlineColorOverrideCreatesLocalMaterialWithoutChangingSharedMaterial()
        {
            Assert.IsNotNull(outlineShader, "Expected the pixel outline shader to be available.");

            var sharedMaterial = new Material(outlineShader);
            sharedMaterial.SetColor(OutlineColorProperty, Color.white);

            var first = CreateRoText("First", sharedMaterial);
            var second = CreateRoText("Second", sharedMaterial);

            first.OverrideOutlineColor = true;
            first.OutlineColor = Color.red;
            second.OverrideOutlineColor = true;
            second.OutlineColor = Color.blue;

            var firstMaterial = first.GetModifiedMaterial(sharedMaterial);
            var secondMaterial = second.GetModifiedMaterial(sharedMaterial);

            Assert.AreNotSame(sharedMaterial, firstMaterial);
            Assert.AreNotSame(sharedMaterial, secondMaterial);
            Assert.AreNotSame(firstMaterial, secondMaterial);
            Assert.AreEqual(Color.white, sharedMaterial.GetColor(OutlineColorProperty));
            Assert.AreEqual(Color.red, firstMaterial.GetColor(OutlineColorProperty));
            Assert.AreEqual(Color.blue, secondMaterial.GetColor(OutlineColorProperty));

            DestroyText(first);
            DestroyText(second);
            Object.DestroyImmediate(sharedMaterial);
        }

        [Test]
        public void ClearOutlineColorOverrideRestoresBaseMaterial()
        {
            Assert.IsNotNull(outlineShader, "Expected the pixel outline shader to be available.");

            var sharedMaterial = new Material(outlineShader);
            var text = CreateRoText("Restored", sharedMaterial);

            text.OverrideOutlineColor = true;
            text.OutlineColor = Color.yellow;
            Assert.AreNotSame(sharedMaterial, text.GetModifiedMaterial(sharedMaterial));

            text.ClearOutlineColorOverride();

            Assert.AreSame(sharedMaterial, text.GetModifiedMaterial(sharedMaterial));

            DestroyText(text);
            Object.DestroyImmediate(sharedMaterial);
        }

        [Test]
        public void UnsupportedMaterialsAreIgnored()
        {
            var uiShader = Shader.Find("UI/Default");
            Assert.IsNotNull(uiShader, "Expected Unity's default UI shader to be available.");

            var sharedMaterial = new Material(uiShader);
            var text = CreateRoText("Unsupported", sharedMaterial);

            text.OverrideOutlineColor = true;
            text.OutlineColor = Color.magenta;

            Assert.AreSame(sharedMaterial, text.GetModifiedMaterial(sharedMaterial));

            DestroyText(text);
            Object.DestroyImmediate(sharedMaterial);
        }

        [Test]
        public void MaterialChangeReappliesOutlineColorToNewLocalMaterial()
        {
            Assert.IsNotNull(outlineShader, "Expected the pixel outline shader to be available.");

            var firstSharedMaterial = new Material(outlineShader);
            var secondSharedMaterial = new Material(outlineShader);
            secondSharedMaterial.SetColor(OutlineColorProperty, Color.white);
            var text = CreateRoText("Material Change", firstSharedMaterial);

            text.OverrideOutlineColor = true;
            text.OutlineColor = Color.red;
            var firstLocalMaterial = text.GetModifiedMaterial(firstSharedMaterial);
            var secondLocalMaterial = text.GetModifiedMaterial(secondSharedMaterial);

            Assert.AreNotSame(firstLocalMaterial, secondLocalMaterial);
            Assert.AreNotSame(secondSharedMaterial, secondLocalMaterial);
            Assert.AreEqual(Color.red, secondLocalMaterial.GetColor(OutlineColorProperty));
            Assert.AreEqual(Color.white, secondSharedMaterial.GetColor(OutlineColorProperty));

            DestroyText(text);
            Object.DestroyImmediate(firstSharedMaterial);
            Object.DestroyImmediate(secondSharedMaterial);
        }

        private static RoText CreateRoText(string name, Material material)
        {
            var gameObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer));
            var text = gameObject.AddComponent<RoText>();
            text.fontSharedMaterial = material;
            return text;
        }

        private static void DestroyText(RoText text)
        {
            Object.DestroyImmediate(text.gameObject);
        }
    }
}
