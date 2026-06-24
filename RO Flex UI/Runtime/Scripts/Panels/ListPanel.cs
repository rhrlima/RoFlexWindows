using RO_Flex_UI.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

//TODO small flick when passing mouse over list (adjust height to match items in it)
//FIXME disable scroll dragging the panel
namespace RO_Flex_UI.Panels
{
    public class ListPanel : MonoBehaviour, IPanel
    {
        [Tooltip("The viewport mask window area of the ScrollRect.")]
        [SerializeField] private RectTransform viewport;
        [FormerlySerializedAs("defaultTemplate")]
        [SerializeField] private ListItem template;
        [SerializeField] private bool loopNavigation = true;
        [SerializeField] private bool autoScroll = true;
        [FormerlySerializedAs("listItems")]
        [FormerlySerializedAs("initialItems")]
        [SerializeField] private List<ListItem> items = new();

        private RectTransform contentTransform;
        public ListItem FocusedItem { get; private set; }
        public ListItem ActivatedItem { get; private set; }

        private void Start()
        {
            if (!EnsureReferences()) return;

            ValidateItems();
            UpdateNavigation();
        }

        public bool EnsureReferences()
        {
            if (contentTransform == null)
                contentTransform = transform as RectTransform;

            if (template != null)
                template.gameObject.SetActive(false);

            return contentTransform != null;
        }

        private void OnDisable()
        {
            FocusedItem = null;
            ActivatedItem = null;
        }

        private bool ValidateItem(ListItem item)
        {
            if (item == null) return false;

            if (item.transform.parent != contentTransform)
                item.transform.SetParent(contentTransform, false);

            item.gameObject.SetActive(true);
            item.EnsureButtonCached();
            item.BindToPanel(this);
            return true;
        }

        private void ValidateItems()
        {
            var uniqueItems = new HashSet<ListItem>();

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (uniqueItems.Add(item) && ValidateItem(item)) continue;

                items.RemoveAt(i);
                i--;
            }
        }

        public void Clear()
        {
            //TODO should I really destroy GameObjects here?
            foreach (var item in items)
            {
                if (item != null) Destroy(item.gameObject);
            }
            items.Clear();
            FocusedItem = null;
            ActivatedItem = null;
        }

        public void AddItem(ListItem item)
        {
            if (!EnsureReferences() || items.Contains(item) || !ValidateItem(item)) return;

            items.Add(item);
            UpdateNavigation();
        }

        public void AddItems(IEnumerable<ListItem> newItems)
        {
            if (!EnsureReferences() || newItems == null) return;

            AddItemsInternal(newItems);
            UpdateNavigation();
        }

        public void AddItems<TData>(IEnumerable<TData> data, Action<ListItem, TData> bind, ListItem itemTemplate = null)
        {
            if (!EnsureReferences() || data == null) return;

            var sourceTemplate = itemTemplate != null ? itemTemplate : template;
            if (sourceTemplate == null)
            {
                Debug.LogError($"Failed to add items because Item Template is Null.", this);
                return;
            }

            foreach (var dataEntry in data)
            {
                var instance = Instantiate(sourceTemplate, contentTransform);
                if (!ValidateItem(instance)) continue;

                items.Add(instance);
                bind?.Invoke(instance, dataEntry);
            }
            UpdateNavigation();
        }

        private void AddItemsInternal(IEnumerable<ListItem> newItems)
        {
            if (newItems == null) return;

            foreach (var item in newItems)
            {
                if (items.Contains(item) || !ValidateItem(item)) continue;

                items.Add(item);
            }
        }

        // --- Dynamic Navigation & Scrolling Layout Systems ---

        private void UpdateNavigation()
        {
            items.RemoveAll(item => item == null);

            var count = items.Count;

            for (var i = 0; i < count; i++)
            {
                var item = items[i];
                var currentButton = item.TargetButton;

                if (currentButton == null) continue;

                var cleanNav = new Navigation
                {
                    mode = Navigation.Mode.Explicit,
                };

                if (count > 1)
                {
                    cleanNav.selectOnUp = i > 0
                        ? items[i - 1].TargetButton
                        : loopNavigation ? items[count - 1].TargetButton : null;
                    cleanNav.selectOnDown = i < count - 1
                        ? items[i + 1].TargetButton
                        : loopNavigation ? items[0].TargetButton : null;
                }

                currentButton.navigation = cleanNav;
            }
        }

        public void NotifyItemFocused(ListItem item)
        {
            FocusedItem = item;
            FitOptionToView(item);
        }

        public void NotifyItemActivated(ListItem item)
        {
            ActivatedItem = item;
            Debug.Log($"[ListPanel] Activated UI Item: {item.name}");
        }

        public void FitOptionToView(ListItem item)
        {
            if (!autoScroll || viewport == null || contentTransform == null) return;

            var itemRect = item.transform as RectTransform;
            if (itemRect == null) return;

            var itemHeight = itemRect.rect.height;
            var itemYPos = itemRect.localPosition.y;
            var viewportHeight = viewport.rect.height;

            var currentContentY = contentTransform.localPosition.y - viewportHeight / 2;

            var targetTopY = currentContentY + itemYPos;
            var targetBottomY = -itemYPos + itemHeight - viewportHeight - currentContentY;

            if (targetBottomY > 0)
            {
                contentTransform.localPosition += new Vector3(0, targetBottomY, 0);
            }

            if (targetTopY > 0)
            {
                contentTransform.localPosition -= new Vector3(0, targetTopY, 0);
            }
        }

        public void SelectOption(int index)
        {
            if (index < 0 || index >= items.Count) return;

            var button = items[index].TargetButton;
            if (button == null || !button.IsActive() || !button.IsInteractable()) return;

            button.Select();
        }

        #region Getter & Setter
        public bool LoopNavigation
        {
            get => loopNavigation;
            set
            {
                loopNavigation = value;
                UpdateNavigation();
            }
        }
        public bool AutoScroll
        {
            get => autoScroll;
            set => autoScroll = value;
        }
        #endregion
    }
}