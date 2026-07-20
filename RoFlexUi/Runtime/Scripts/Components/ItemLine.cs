using RO_Flex_UI.Utils;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

        [SerializeField] private GameObject socketTemplate;
        [SerializeField] private TMP_Text text;
        [SerializeField] private List<Socket> sockets = new();
        public int numSockets => sockets == null ? 0 : sockets.Count;

        private void Awake()
        {
            if (!EnsureReferences()) return;
            Setup();
        }

        public bool EnsureReferences()
        {
            if (!Tools.IsValid(this, socketTemplate)) return false;
            if (!Tools.IsValid(this, text)) return false;
            return true;
        }

        private void Setup()
        {
            socketTemplate.gameObject.SetActive(false);
        }

        #region Getter & Setter
        public string Text
        {
            get => text.text;
            set => text.text = value;
        }
        #endregion
    }
}