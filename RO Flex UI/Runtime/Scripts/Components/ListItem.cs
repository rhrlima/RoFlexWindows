using RO_Flex_UI.Panels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    [RequireComponent(typeof(Button))]
    public class ListItem : MonoBehaviour, IPointerEnterHandler, ISelectHandler, ISubmitHandler, IPointerClickHandler
    {
        [Header("Simplified Events")]
        public UnityEvent OnItemFocused = new UnityEvent();
        public UnityEvent OnItemActivated = new UnityEvent();

        [Header("Double Click Timing")]
        [SerializeField] private float doubleClickLimit = 0.25f;
        private float lastClickTime;

        public Button TargetButton { get; private set; }
        protected ListPanel parentPanel;

        protected virtual void Awake()
        {
            EnsureButtonCached();
        }

        public void EnsureButtonCached()
        {
            if (TargetButton == null)
            {
                TargetButton = GetComponent<Button>();
                TargetButton.onClick.RemoveAllListeners();
                TargetButton.onClick.AddListener(HandleSingleClickFocus);
            }
        }

        public virtual void BindToPanel(ListPanel panel)
        {
            parentPanel = panel;
        }

        // --- Focus / Selection Pipeline ---
        public void OnPointerEnter(PointerEventData eventData) => FocusItem();
        public void OnSelect(BaseEventData eventData) => FocusItem();

        private void HandleSingleClickFocus()
        {
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != gameObject)
            {
                TargetButton.Select();
            }
            FocusItem();
        }

        private void FocusItem()
        {
            OnItemFocused?.Invoke();
            parentPanel?.NotifyItemFocused(this);
        }

        // --- Activation Pipeline ---
        public void OnPointerClick(PointerEventData eventData)
        {
            if (Time.time - lastClickTime < doubleClickLimit)
            {
                ActivateItem();
            }
            lastClickTime = Time.time;
        }

        public void OnSubmit(BaseEventData eventData)
        {
            ActivateItem();
        }

        private void ActivateItem()
        {
            OnItemActivated?.Invoke();
            parentPanel?.NotifyItemActivated(this);
        }
    }
}