using TMPro;

public class InventoryWindow : Window
{
    public TextMeshProUGUI itemsTotalText;
    private ItemsPanel usableItemsPanel;
    private ItemsPanel gearItemsPanel;
    private ItemsPanel etcItemsPanel;
    private ItemsPanel favItemsPanel;

    public void Start()
    {
        var panels = GetComponentsInChildren<ItemsPanel>(true);
        usableItemsPanel = panels[0];
        gearItemsPanel = panels[1];
        etcItemsPanel = panels[2];
        favItemsPanel = panels[3];
    }

    public void Update()
    {
        if (itemsTotalText == null)
            return;

        var totalItems = usableItemsPanel.numItems
            + gearItemsPanel.numItems
            + etcItemsPanel.numItems
            + favItemsPanel.numItems;
        itemsTotalText.text = $"{totalItems}/100";
    }
}
