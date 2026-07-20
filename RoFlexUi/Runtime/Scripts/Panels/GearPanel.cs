using RO_Flex_UI.Components;
using RO_Flex_UI.Utils;
using UnityEngine;

namespace RO_Flex_UI.Panels
{
    public class GearPanel : MonoBehaviour, IPanel
    {
        [SerializeField] private int slotsPerPanel;
        [SerializeField] private IconText template;
        [SerializeField] private RectTransform leftPanel;
        [SerializeField] private RectTransform rightPanel;

        private void Awake()
        {
            if (!EnsureReferences()) return;

            InitializePanels();
        }

        public bool EnsureReferences()
        {
            if (template == null)
            {
                Tools.LogMissingReference(this, nameof(template));
                return false;
            }
            if (leftPanel == null)
            {
                Tools.LogMissingReference(this, nameof(leftPanel));
                return false;
            }
            if (rightPanel == null)
            {
                Tools.LogMissingReference(this, nameof(rightPanel));
                return false;
            }
            return true;
        }

        private void InitializePanels()
        {
            template.gameObject.SetActive(false);

            for (var i = 0; i < slotsPerPanel * 2; i++)
            {
                var instance = Instantiate(template, (i < slotsPerPanel) ? leftPanel : rightPanel);
                instance.gameObject.SetActive(true);
                instance.FlipElements(i >= slotsPerPanel);
            }
        }
    }
}