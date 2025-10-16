using UnityEngine;

public sealed class UIManager : MonoBehaviour
{
    public InputWindow inputWindow;
    public MessageModal modal;

    void Update()
    {
        var mgr = WindowManager.GetInstance();

        if (Input.GetKeyDown(KeyCode.Escape) && !mgr.Empty())
        {
            var window = mgr.PopWindow();
            window.HideWindow();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            inputWindow.ShowWindow((value) => {});
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            inputWindow.ShowWindow((value) => {}, onlyNumbers: true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            modal.ShowModal("Simple modal window.");
        }
    }
}
