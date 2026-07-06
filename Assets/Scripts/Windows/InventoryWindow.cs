using RO_Flex_UI.Panels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RO_Flex_UI.Windows
{
    public class InventoryWindow : Window
    {

        [SerializeField] private TextMeshProUGUI itemsTotalText;
        [SerializeField] private List<FillPanel> panels;

        public void Start()
        {
            UpdateItemsAmount();
        }

        private void UpdateItemsAmount()
        {
            if (itemsTotalText == null)
                return;

            var totalItems = 0;
            foreach (var panel in panels)
            {
                // totalItems += panel.NumItems;
            }

            itemsTotalText.SetText("{0}/100", totalItems);
        }

        public void Update()
        {
            UpdateItemsAmount();
        }
    }
}