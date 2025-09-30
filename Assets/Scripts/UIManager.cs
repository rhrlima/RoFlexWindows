using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public Window baseWindow;
    public PartyModal partyModal;
    public FriendModal friendModal;
    public DealModal dealModal;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            baseWindow.ToggleVisibility();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            partyModal.ShowModal("Greatest Group");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            friendModal.ShowModal("Faker");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            dealModal.ShowModal("Faker", "ABC123456", 89);
        }
    }
}
