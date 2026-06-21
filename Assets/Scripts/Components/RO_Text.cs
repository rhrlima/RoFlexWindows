using TMPro;
using UnityEngine;

namespace RO_Flex_UI.Components
{
    public class RO_Text : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private void Start()
        {
            if (!EnsureReferences()) return;
        }

        private bool EnsureReferences()
        {
            if (text == null)
            {
                Debug.Log($"[{name}] Text component not assigned.");
                return false;
            }

            return true;
        }

        public string Text
        {
            get => text.text;
            set => text.text = value;
        }
    }
}