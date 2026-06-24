using RO_Flex_UI.Panels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Components
{
    [RequireComponent(typeof(RoButton))]
    public class ListItem : MonoBehaviour, IPointerEnterHandler, ISelectHandler, ISubmitHandler, IPointerClickHandler
    {
        public class ListEvent : UnityEvent { }

        [Header("Simplified Events")]
        public ListEvent OnItemFocused = new();
        public ListEvent OnItemActivated = new();

        [Tooltip("Max time between clicks to register a double click (in Seconds).")]
        [SerializeField] private float doubleClickLimit = 0.25f;
        private float lastClickTime;

        public RoButton TargetButton { get; private set; }
        protected ListPanel parentPanel;

        protected virtual void Awake()
        {
            EnsureButtonCached();
        }

        public void EnsureButtonCached()
        {
            if (TargetButton == null)
            {
                TargetButton = GetComponent<RoButton>();
                TargetButton.onClick.RemoveListener(HandleSingleClickFocus);
                TargetButton.onClick.AddListener(HandleSingleClickFocus);
            }
        }

        public virtual void BindToPanel(ListPanel panel)
        {
            parentPanel = panel;
        }

        #region Focus / Selection
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

        public void FocusItem()
        {
            OnItemFocused?.Invoke();
            parentPanel?.NotifyItemFocused(this);
        }
        #endregion

        #region Submit
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
        #endregion
    }
}