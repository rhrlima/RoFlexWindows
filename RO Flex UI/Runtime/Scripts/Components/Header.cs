using RO_Flex_UI.Utils;
using TMPro;
using UnityEngine;

namespace RO_Flex_UI.Components
{
    public class Header : MonoBehaviour, IComponent
    {
        [SerializeField] private RoButton funButton;
        [SerializeField] private RoButton minButton;
        [SerializeField] private RoButton closeButton;
        [SerializeField] private TMP_Text title;

        private void Awake()
        {
            if (!EnsureReferences()) return;
        }

        public bool EnsureReferences()
        {
            if (!Tools.IsValid(this, funButton)) return false;
            if (!Tools.IsValid(this, minButton)) return false;
            if (!Tools.IsValid(this, closeButton)) return false;
            if (!Tools.IsValid(this, title)) return false;
            return true;
        }
    }
}