using UnityEngine;

public class PartyModal : ModalWindow
{
    // TODO can use list of replacements to be more modular
    public void ShowModal(string partyName)
    {
        messageText.text = messageText.text.Replace("$partyName", partyName);
        ShowWindow();
    }

    public void OnAccept()
    {
        Debug.Log("Accepted.");
        HideWindow();
    }

    public void OnCancel()
    {
        Debug.Log("Rejected.");
        HideWindow();
    }
}
