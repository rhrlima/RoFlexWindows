using RO_Flex_UI.Config;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfigTest : MonoBehaviour
{
    [SerializeField] private GlobalUIConfigData configData;
    [SerializeField] private CanvasScaler canvasScaler;

    private void OnEnable()
    {
        configData.OnConfigChanged += SetUIScale;
        SetUIScale(configData);
    }

    private void OnDisable()
    {
        configData.OnConfigChanged -= SetUIScale;
    }

    private void SetUIScale(GlobalUIConfigData data)
    {
        canvasScaler.scaleFactor = data.uiScale;
    }
}
