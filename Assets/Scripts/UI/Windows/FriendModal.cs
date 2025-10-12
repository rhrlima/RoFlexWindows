using UnityEngine;

public class FriendModal : ModalWindow
{
    public void ShowModal(string playerName)
    {
        messageText.text = messageText.text.Replace("$playerName", playerName);
        ShowWindow();
    }

    public void OnAccept()
    {
        Debug.Log("Accept.");
        HideWindow();
    }

    public void OnCancel()
    {
        Debug.Log("Rejected.");
        HideWindow();
    }
}
