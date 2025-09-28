using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public Window baseWindow;
    public ModalWindow modalWindow;
    public PartyModal partyModal;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            baseWindow.ToggleVisibility();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            modalWindow.ShowModal(
                "Message",
                "<color=#007700>Faker</color> wants to be friends with you.",
                new List<string> { "OK", "NO", "WTF" },
                new List<UnityAction> {
                    modalWindow.HideWindow,
                    modalWindow.HideWindow,
                    modalWindow.HideWindow,
                }
            );
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            partyModal.ShowModal(
                "Message",
                "<color=#007700>(WTF Group)</color> is sending you an invite. Accept?",
                new List<string> { "Yes", "cancel", "???" },
                new List<UnityAction> {
                    partyModal.HideWindow,
                    partyModal.HideWindow,
                    partyModal.HideWindow,
                }
            );
        }
    }
}
