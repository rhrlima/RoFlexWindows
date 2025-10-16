using System.Collections;
using System.Collections.Generic;

public sealed class WindowManager
{
    private static WindowManager instance;
    private Stack<Window> windowStack;
    private WindowManager()
    {
        windowStack = new Stack<Window>();
    }

    public static WindowManager GetInstance()
    {
        if (instance == null)
        {
            instance = new WindowManager();
        }
        return instance;
    }

    public bool Empty()
    {
        return windowStack.Count == 0;
    }

    public bool Contains(Window window)
    {
        return windowStack.Contains(window);
    }

    public void PushWindow(Window window)
    {
        windowStack.Push(window);
    }

    public Window PopWindow()
    {
        return windowStack.Pop();
    }
}