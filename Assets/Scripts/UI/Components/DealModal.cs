using UnityEngine;

public class DealModal : ModalWindow
{
    public void ShowModal(string playerName, string playerID, int playerLevel)
    {
        messageText.text = messageText.text
            .Replace("$playerName", playerName)
            .Replace("$playerID", playerID)
            .Replace("$playerLevel", playerLevel.ToString());
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
