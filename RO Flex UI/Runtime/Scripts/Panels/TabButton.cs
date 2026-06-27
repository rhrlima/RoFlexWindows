using RO_Flex_UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Panels
{
    [RequireComponent(typeof(Image))]
    public class TabButton : RoButton, IComponent
    {
        [SerializeField] private Sprite tabActive;
        [SerializeField] private Sprite tabIdle;

        private bool isActive;
        public TabGroup TabGroup { get; set; }

        protected override void Start()
        {
            base.Start();
            if (!EnsureReferences()) return;
        }

        public override bool EnsureReferences()
        {
            if (image == null)
                image = GetComponent<Image>();

            return image != null;
        }

        public void SetActive(bool value)
        {
            isActive = value;

            if (!EnsureReferences()) return;

            image.sprite = isActive ? tabActive : tabIdle;
        }
    }
}
