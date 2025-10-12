using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemEntry : MonoBehaviour
{
    [SerializeField]
    private Image itemSprite;
    [SerializeField]
    private TextMeshProUGUI itemAmountText;
    public int itemAmount;

    public void OnValidate()
    {
        itemAmountText.text = itemAmount.ToString();
        itemSprite.gameObject.SetActive(itemAmount > 0);
        itemAmountText.gameObject.SetActive(itemAmount > 0);
    }
}
