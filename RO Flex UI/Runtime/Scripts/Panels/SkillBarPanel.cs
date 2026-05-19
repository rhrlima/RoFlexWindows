using RO_Flex_UI.Components;
using UnityEngine;

namespace RO_Flex_UI.Panels
{
    public class SkillBarPanel : MonoBehaviour
    {
        [SerializeField] private ItemEntry skillSlot;
        public int numSlots;

        private void Start()
        {
            for (var i = 0; i < numSlots - 1; i++)
            {
                var bar = Instantiate(skillSlot, transform);
            }
        }
    }
}