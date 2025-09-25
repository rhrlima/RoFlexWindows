
using UnityEngine;

public class ModalWindow : WinBase
{
    public virtual void OnButtonLeft()
    {
        Debug.Log("Button Left");
        HideWindow();
    }

    public virtual void OnButtonMiddle()
    {
        Debug.Log("Button Middle");
        HideWindow();
    }
    
    public virtual void OnButtonRight()
    {
        Debug.Log("Button Right");
        HideWindow();
    }
}
