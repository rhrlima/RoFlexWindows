using RO_Flex_UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Panels
{
    public class GearPanel : IPanel
    {
        [SerializeField] private int slotsPerPanel;
        [SerializeField] private GearSlot template;
        [SerializeField] private RectTransform leftPanel;
        [SerializeField] private RectTransform rightPanel;

        private void Awake()
        {
            if (template == null)
                Debug.LogError("No prefab of type GearSlot was assigned to template.");

            if (leftPanel == null || rightPanel == null)
            {
                Debug.LogError("Left and Right Panels cannot be null.");
                return;
            }

            InitializePanels();
        }

        private void InitializePanels()
        {
            var placeholder = leftPanel.GetComponentInChildren<GearSlot>();
            if (placeholder != null) placeholder.gameObject.SetActive(false);

            placeholder = rightPanel.GetComponentInChildren<GearSlot>();
            if (placeholder != null) placeholder.gameObject.SetActive(false);

            for (var i = 0; i < slotsPerPanel * 2; i++)
            {
                var instance = Instantiate(template, (i < slotsPerPanel) ? leftPanel : rightPanel);
                instance.gameObject.SetActive(true);

                if (i >= slotsPerPanel)
                    instance.GetComponent<HorizontalLayoutGroup>().reverseArrangement = true;
            }
        }
    }
}