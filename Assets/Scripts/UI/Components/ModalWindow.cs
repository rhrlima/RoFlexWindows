
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalWindow : Window
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private List<Button> buttonsText;

    public void ShowModal(string title, string message, List<string> buttons, List<UnityAction> actions)
    {
        titleText.text = title;
        messageText.text = message;

        for (int i = 0; i < buttonsText.Count; i++)
        {
            buttonsText[i].GetComponentInChildren<TextMeshProUGUI>(true).text = buttons[i];
            buttonsText[i].onClick.AddListener(actions[i]);
        }

        ShowWindow();
    }
}
