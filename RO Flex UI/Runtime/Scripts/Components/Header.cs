using RO_Flex_UI.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    public class Header : MonoBehaviour, IComponent
    {
        [SerializeField] private RoButton funButton;
        [SerializeField] private RoButton minButton;
        [SerializeField] private RoButton closeButton;
        [SerializeField] private TMP_Text title;

        [SerializeField] private Button.ButtonClickedEvent onFunButtonClick;
        [SerializeField] private Button.ButtonClickedEvent onMinButtonClick;
        [SerializeField] private Button.ButtonClickedEvent onCloseButtonClick;

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

        private void OnEnable()
        {
            funButton.onClick.AddListener(HandleFunButtonClick);
            minButton.onClick.AddListener(HandleMinButtonClick);
            closeButton.onClick.AddListener(HandleCloseButtonClick);
        }

        private void OnDisable()
        {
            funButton.onClick.RemoveListener(HandleFunButtonClick);
            minButton.onClick.RemoveListener(HandleMinButtonClick);
            closeButton.onClick.RemoveListener(HandleCloseButtonClick);
        }

        private void HandleFunButtonClick()
        {
            onFunButtonClick?.Invoke();
        }

        private void HandleMinButtonClick()
        {
            onMinButtonClick?.Invoke();
        }

        private void HandleCloseButtonClick()
        {
            onCloseButtonClick?.Invoke();
        }

        #region Getter & Setter
        public string Text
        {
            get => title.text;
            set => title.text = value;
        }
        public Button.ButtonClickedEvent OnFunButtonClick => onFunButtonClick;
        public Button.ButtonClickedEvent OnMinButtonClick => onMinButtonClick;
        public Button.ButtonClickedEvent OnCloseButtonClick => onCloseButtonClick;
        #endregion
    }
}