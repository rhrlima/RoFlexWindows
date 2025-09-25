using TMPro;
using UnityEngine;

public class FriendModal : ModalWindow
{
    [SerializeField] private TextMeshProUGUI messagePanel;

    public void SendFriendInvite(string playerName)
    {
        messagePanel.text = messagePanel.text.Replace("{player}", playerName);
    }

    public override void OnButtonMiddle()
    {
        Debug.Log("Friend Request Accepted");
        base.OnButtonMiddle();
    }

    public override void OnButtonRight()
    {
        Debug.Log("Friend Request Declined");
        base.OnButtonRight();
    }
}
