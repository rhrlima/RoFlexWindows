using RO_Flex_UI.Panels;
using UnityEngine;

namespace RO_Flex_UI.Windows
{
    public class SkillBar : Window
    {
        [SerializeField] private SkillBarPanel skillBarPanel;
        public int numBars;

        private void Start()
        {
            if (skillBarPanel != null)
                skillBarPanel.gameObject.SetActive(false);

            for (int i = 0; i < numBars - 1; i++)
            {
                var bar = Instantiate(skillBarPanel, transform);
            }
        }
    }
}