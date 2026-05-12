using RO_Flex_UI.Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Panels
{
    public class GearPanel : IPanel
    {
        [SerializeField] private GearSlot template;
        [SerializeField] private RectTransform leftPanel;
        [SerializeField] private RectTransform rightPanel;
        public void Start()
        {
            for (var i = 0; i < 10; i++)
            {
                var instance = Instantiate(template, (i < 5) ? leftPanel : rightPanel);
                instance.gameObject.SetActive(true);

                if (i > 4)
                    instance.GetComponent<HorizontalLayoutGroup>().reverseArrangement = true;
            }
        }
    }
}