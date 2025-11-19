using System.Collections.Generic;
using UnityEngine;

public sealed class UIManager : MonoBehaviour
{
    public ListModal listWindow;
    public ListModal listModal;

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
            listWindow.ShowModal(new List<string>() {"Chaos", "Iris", "Loki", "Chaos", "Iris", "Loki", "Chaos", "Iris", "Loki"}, (item) =>
            {
                Debug.Log($"Selected option: {item.OptionText} at index {item.Index}");
                listWindow.HideWindow();
            });
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            listModal.ShowModal(new List<string>() {"Chaos", "Iris", "Loki", "Chaos", "Iris", "Loki", "Chaos", "Iris", "Loki"}, (item) =>
            {
                Debug.Log($"Selected option: {item.OptionText} at index {item.Index}");
                listModal.HideWindow();
            });
        }
    }
}
