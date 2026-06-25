using RO_Flex_UI.Panels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Components
{
    [RequireComponent(typeof(RoButton))]
    public class ListItem : MonoBehaviour, IComponent, IPointerEnterHandler, ISelectHandler, ISubmitHandler, IPointerClickHandler
    {
        public class ListEvent : UnityEvent { }

        [Header("Simplified Events")]
        public ListEvent OnItemFocused = new();
        public ListEvent OnItemActivated = new();

        [Tooltip("Max time between clicks to register a double click (in Seconds).")]
        [SerializeField] private float doubleClickLimit = 0.25f;
        private float lastClickTime = -1f;

        public RoButton TargetButton { get; private set; }
        protected ListPanel parentPanel;

        protected virtual void Awake()
        {
            OnEnable();
        }

        public bool EnsureReferences()
        {
            TargetButton = GetComponent<RoButton>();
            if (TargetButton == null) return false;

            return true;
        }

        private void OnEnable()
        {
            if (!EnsureReferences()) return;

            TargetButton.onClick.AddListener(HandleSingleClickFocus);
        }

        private void OnDisable()
        {
            if (!EnsureReferences()) return;

            TargetButton.onClick.RemoveListener(HandleSingleClickFocus);
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
            if (eventData.button != PointerEventData.InputButton.Left) return;

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