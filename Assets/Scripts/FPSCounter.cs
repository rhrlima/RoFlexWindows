using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TMP_Text fpsText;
    public void Update()
    {
        var currentFPS = 1.0f / Time.unscaledDeltaTime;
        fpsText.text = $"FPS: {Mathf.RoundToInt(currentFPS)}";
    }
}
