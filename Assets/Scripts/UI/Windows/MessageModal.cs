public class MessageModal : ModalWindow
{
    public void ShowModal(string message)
    {
        messageText.text = message;
        ShowWindow();
    }

    public void OnAccept()
    {
        HideWindow();
    }
}
