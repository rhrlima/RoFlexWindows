using UnityEngine;
using UnityEngine.UI;

public class ItemEntry : MonoBehaviour
{
    private Image itemSprite;
    public string itemName;
    public int itemAmount;
    public void Start()
    {
        itemSprite = GetComponentInChildren<Image>(true);

        itemName = "";
        itemAmount = 0;
    }
}
