using TMPro;
using UnityEngine;

public class GearSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private Sprite _itemIcon;

    public string ItemName
    {
        get => _itemName.text;
        set => _itemName.text = value;
    }
    public Sprite ItemIcon
    {
        get => _itemIcon;
        set => _itemIcon = value;
    }
}
