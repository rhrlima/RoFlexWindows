using RO_Flex_UI.Components;
using TMPro;
using UnityEngine;

namespace RO_Flex_UI.Panels
{
    public class Header : MonoBehaviour, IComponent
    {
        [SerializeField] private RoButton funButton;
        [SerializeField] private RoButton minButton;
        [SerializeField] private RoButton closeButton;
        [SerializeField] private TMP_Text title;

        public bool EnsureReferences()
        {
            throw new System.NotImplementedException();
        }
    }
}