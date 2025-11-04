using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemLine : MonoBehaviour
{
    [SerializeField] private bool isSockets = false;
    // [SerializeField] private GameObject socketPanel;
    [SerializeField] private GameObject socketObj;
    [SerializeField] private TextMeshProUGUI textObj;
    [SerializeField] private Sprite openSocket;
    [SerializeField] private Sprite closedSocket;
    public int totalSockets;
    public int openSockets;
    public string text;

    private void Start()
    {
        if (isSockets) {
            SetNumSockets(totalSockets, openSockets);
        } else {
            SetText(text);
        }
    }

    public void SetText(string text)
    {
        textObj.text = text;
        textObj.gameObject.SetActive(!isSockets);
    }

    public void SetNumSockets(int totalSockets, int openSockets = 0)
    {
        this.totalSockets = totalSockets;
        this.openSockets = openSockets;

        for (int i = 0; i < totalSockets; i++)
        {
            var socket = Instantiate(socketObj, transform);
            socket.SetActive(isSockets);

            if (i < openSockets)
                socket.GetComponent<Image>().sprite = openSocket;
        }
    }
}
