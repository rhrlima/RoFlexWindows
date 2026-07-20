using RO_Flex_UI.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    public class ItemDescriptionPanel : MonoBehaviour, IPanel
    {

        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image splashImage;
        [SerializeField] private RoButton previewButton;
        public bool EnsureReferences()
        {
            throw new System.NotImplementedException();
        }
    }
}