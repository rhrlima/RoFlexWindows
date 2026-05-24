using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    public class ItemLine : MonoBehaviour
    {
        [SerializeField] private bool isSockets = false;
        // [SerializeField] private GameObject socketPanel;
        [SerializeField] private Transform socketsContainer;
        [SerializeField] private GameObject socketTemplate;
        [SerializeField] private TextMeshProUGUI textObj;
        [SerializeField] private Sprite openSocket;
        [SerializeField] private Sprite closedSocket;
        [SerializeField] private int totalSockets;
        [SerializeField] private int openSockets;
        [SerializeField] private string text;

        private void Awake()
        {
            if (socketsContainer != null)
            {
                socketsContainer.gameObject.SetActive(false);
                socketTemplate.SetActive(false);
            }

            if (textObj != null)
                textObj.gameObject.SetActive(false);

            if (isSockets)
            {
                SetNumSockets(totalSockets, openSockets);
            }
            else
            {
                SetText(text);
            }
        }

        public void SetText(string text)
        {
            textObj.text = text;
            textObj.gameObject.SetActive(true);
        }

        public void SetNumSockets(int totalSockets, int openSockets = 0)
        {
            this.totalSockets = totalSockets;
            this.openSockets = openSockets;

            for (int i = 0; i < totalSockets; i++)
            {
                var socket = Instantiate(socketTemplate, socketsContainer);
                socket.SetActive(isSockets);

                if (i < openSockets)
                    socket.GetComponent<Image>().sprite = openSocket;
            }

            socketsContainer.gameObject.SetActive(true);
        }
    }
}