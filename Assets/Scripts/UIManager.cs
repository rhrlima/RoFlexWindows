using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PartyModal partyModal;
    public FriendModal friendModal;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            friendModal.SendFriendInvite("Faker");
            friendModal.ToggleVisibility();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            partyModal.SendPartyInvite("My awesome group");
            partyModal.ToggleVisibility();
        }
    }
}
