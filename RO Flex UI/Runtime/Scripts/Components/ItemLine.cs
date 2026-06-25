using RO_Flex_UI.Utils;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace RO_Flex_UI.Components
{
    public class ItemLine : MonoBehaviour, IComponent
    {
        [Serializable]
        public class Socket
        {
            public bool open;
            public IconAmount slot;
        }

        [SerializeField] private IconAmount socketTemplate;
        [SerializeField] private TMP_Text text;
        [SerializeField] private List<Socket> sockets = new();
        public int numSockets => sockets == null ? 0 : sockets.Count;

        public string Text
        {
            get => text.text;
            set => text.text = value;
        }

        private void Awake()
        {
            if (!EnsureReferences()) return;
            Setup();
        }

        public bool EnsureReferences()
        {
            if (text == null)
            {
                Tools.LogMissingReference(this, nameof(text));
                return false;
            }
            return true;
        }

        private void Setup()
        {
            socketTemplate.gameObject.SetActive(false);
        }
    }
}