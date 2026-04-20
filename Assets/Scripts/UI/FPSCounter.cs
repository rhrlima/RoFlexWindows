using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TMP_Text fpsText;
    public void Update()
    {
        float currentFPS = 1.0f / Time.unscaledDeltaTime;
        fpsText.text = Mathf.RoundToInt(currentFPS).ToString() + " FPS";
    }
}
