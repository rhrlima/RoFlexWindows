using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    public class ItemLine : MonoBehaviour, IComponent
    {
        [SerializeField] private List<IconAmount> sockets;
        [SerializeField] private TMP_Text text;
        public int numSockets => sockets == null ? 0 : sockets.Count;

        private void Awake()
        {
            if (!EnsureReferences()) return;
        }

        public bool EnsureReferences()
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