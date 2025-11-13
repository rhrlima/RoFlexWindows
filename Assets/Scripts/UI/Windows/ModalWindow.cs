
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalWindow : Window
{
    [SerializeField] protected TextMeshProUGUI titleText;
    [SerializeField] protected TextMeshProUGUI messageText;
    [SerializeField] protected List<Button> buttons;
    [SerializeField] protected List<UnityEvent> actions;

    public void Start()
    {
        var count = Mathf.Min(buttons.Count, actions.Count);

        for (int i = 0; i < count; i++)
        {
            if (i >= count) break;

            var buttonAction = actions[i];
            buttons[i].onClick.AddListener(() => { buttonAction.Invoke(); });
        }
    }

    public void ShowModal()
    {
        ShowWindow();
    }
}

//TODO maybe rework to have each modal assign the fields it has, instead of lists
// Most modals are not that flexible, and can be simplified