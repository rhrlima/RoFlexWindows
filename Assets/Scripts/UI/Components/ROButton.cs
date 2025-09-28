using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ROButton : MonoBehaviour
{
    private IWindow window;

    private void Awake()
    {
        window = GetComponentInParent<IWindow>(true);

        if (window == null)
        {
            Debug.LogError("BUtton failed to find IWindow component in parent.");
        }

        GetComponent<Button>()?.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        window?.HideWindow();
    }
}
