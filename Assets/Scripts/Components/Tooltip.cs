using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private RectTransform background;

    void Start()
    {
        HideTooltip();
    }

    void Update()
    {
        background.position = Input.mousePosition + Vector3.one;
    }

    public void SetText(string tooltipText)
    {
        text.text = tooltipText;
        background.sizeDelta = new Vector2(100, text.preferredHeight + 8);
    }

    public void ShowTooltip()
    {
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
