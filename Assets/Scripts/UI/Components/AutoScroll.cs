using UnityEngine;

public class AutoScroll : MonoBehaviour
{
    public RectTransform viewport;
    public RectTransform content;

    public void FitOptiontoView(int index)
    {
        var itemRect = transform.GetChild(index).transform as RectTransform;

        float itemHeight = itemRect.rect.height;
        float itemYPos = itemRect.localPosition.y;

        float viewportHeight = viewport.rect.height;

        // makes sure the content default position is zero (top aligned)
        float currentContentY = content.localPosition.y - viewportHeight / 2;

        float targetTopY = currentContentY + itemYPos;
        float targetBottomY = -itemYPos + itemHeight - viewportHeight - currentContentY;

        if (targetBottomY > 0)
        {
            content.localPosition += new Vector3(0, targetBottomY, 0);
        }

        if (targetTopY > 0)
        {
            content.localPosition -= new Vector3(0, targetTopY, 0);
        }
    }
}
