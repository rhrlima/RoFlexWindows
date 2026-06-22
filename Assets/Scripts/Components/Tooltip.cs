using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private RectTransform background;
    [SerializeField, Min(0f)] private float horizontalPadding = 8f;
    [SerializeField, Min(0f)] private float verticalPadding = 8f;
    [SerializeField, Min(0f)] private float maxWidth = 300f;
    [SerializeField, Min(0f)] private float maxHeight = 200f;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

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
        RefreshSize();
    }

    public void ShowTooltip()
    {
        gameObject.SetActive(true);
        RefreshSize();
        canvasGroup.blocksRaycasts = false;
    }

    public void HideTooltip()
    {
        canvasGroup.blocksRaycasts = true;
        gameObject.SetActive(false);
    }

    private void RefreshSize()
    {
        Vector2 unconstrainedSize = text.GetPreferredValues(
            text.text,
            Mathf.Infinity,
            Mathf.Infinity);
        float backgroundWidth = Mathf.Min(
            unconstrainedSize.x + horizontalPadding,
            maxWidth);
        float availableTextWidth = Mathf.Max(
            0f,
            backgroundWidth - horizontalPadding);
        float preferredHeight = text.GetPreferredValues(
            text.text,
            availableTextWidth,
            Mathf.Infinity).y;
        float backgroundHeight = Mathf.Min(
            preferredHeight + verticalPadding,
            maxHeight);

        background.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            backgroundWidth);
        background.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            backgroundHeight);
        text.ForceMeshUpdate();
    }
}
