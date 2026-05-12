using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public class GearSlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private Image _itemIcon;
        [SerializeField] private bool flipPanel;

        public string ItemName
        {
            get => _itemName.text;
            set => _itemName.text = value;
        }
        public Image ItemIcon
        {
            get => _itemIcon;
            set => _itemIcon = value;
        }
    }
}