using TMPro;
using UnityEngine;

public class PartyModal : ModalWindow
{
    [SerializeField] private TextMeshProUGUI messagePanel;

    public void SendPartyInvite(string partyName)
    {
        messagePanel.text = messagePanel.text.Replace("{party}", partyName);
    }

    public override void OnButtonMiddle()
    {
        Debug.Log("Party Invite Accepted");
        base.OnButtonMiddle();
    }

    public override void OnButtonRight()
    {
        Debug.Log("Party Invite Declined");
        base.OnButtonRight();
    }
}
