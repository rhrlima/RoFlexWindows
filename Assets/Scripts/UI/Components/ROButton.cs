using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ROButton : MonoBehaviour
{
    private IWindow window;

    private void Awake()
    {
        window = GetComponentInParent<IWindow>(true);

        if (window == null)
        {
            Debug.LogError("Button failed to find IWindow component in parent.");
        }

        GetComponent<Button>()?.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        window?.HideWindow();
    }
}
