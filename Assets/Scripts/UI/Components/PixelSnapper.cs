using UnityEngine;

// Add RequireComponent to ensure it's always on a RectTransform
[RequireComponent(typeof(RectTransform))]
public class PixelSnapper : MonoBehaviour
{
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Use LateUpdate to ensure all movement/animation scripts
    // (which usually run in Update) have finished their calculations
    void LateUpdate()
    {
        // Get the current position (which might be a float)
        Vector2 currentAnchoredPosition = rectTransform.anchoredPosition;

        // Round both X and Y to the nearest integer
        float roundedX = Mathf.Round(currentAnchoredPosition.x);
        float roundedY = Mathf.Round(currentAnchoredPosition.y);

        // Check if the rounding actually changed the value 
        // to avoid unnecessary assignment (minor optimization)
        if (currentAnchoredPosition.x != roundedX || currentAnchoredPosition.y != roundedY)
        {
            // Apply the new, forced integer position
            rectTransform.anchoredPosition = new Vector2(roundedX, roundedY);
        }
    }
}
