using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject contentPanel;

    public void Start()
    {
        if (!contentPanel)
            Debug.LogError("No Content Panel assigned!");
    }

    public void OnScrollValueChange()
    {
        Debug.Log($"{contentPanel.transform.position}");

        contentPanel.transform.position = new(
            contentPanel.transform.position.x,
            Mathf.Floor(contentPanel.transform.position.y)
        );
    }
}
