using RO_Flex_UI.Components;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Panels
{
    public class ListPanel : MonoBehaviour
    {
        [Header("Layout & Scroll Settings")]
        // [Tooltip("The rolling content container of the ScrollRect which holds the items.")]
        private RectTransform contentTransform;
        [Tooltip("The viewport mask window area of the ScrollRect.")]
        [SerializeField] private RectTransform viewport;

        [Header("Template Configuration")]
        [SerializeField] private ListItem defaultTemplate;

        [Header("Navigation Settings")]
        [SerializeField] private bool loopNavigation = true;
        [SerializeField] private bool autoScroll = true;

        // [Header("Runtime State")]
        private List<ListItem> currentItems = new List<ListItem>();

        public ListItem FocusedItem { get; private set; }
        public ListItem ActivatedItem { get; private set; }

        private void Awake()
        {
            // Fallback safety if references aren't dragged manually into the inspector
            if (contentTransform == null && transform is RectTransform)
                contentTransform = transform as RectTransform;

            // CRUCIAL: Safely disable the template at runtime so it doesn't skew layouts
            if (defaultTemplate != null)
            {
                defaultTemplate.gameObject.SetActive(false);
            }

            GrabExistingChildren();
        }

        public void GrabExistingChildren()
        {
            currentItems.Clear();
            if (contentTransform == null) return;

            foreach (Transform child in contentTransform)
            {
                // Only grab it if it's active (meaning it's a visible editor asset, not a sleeping template)
                if (!child.gameObject.activeInHierarchy) continue;

                var item = EnsureListItemRequirements(child.gameObject);
                if (item != null && !currentItems.Contains(item))
                {
                    item.BindToPanel(this);
                    currentItems.Add(item);
                }
            }
            UpdateNavigation();
        }

        private void RegisterItem(ListItem item)
        {
            if (!currentItems.Contains(item))
            {
                currentItems.Add(item);
                item.BindToPanel(this);
            }
        }

        private ListItem EnsureListItemRequirements(GameObject targetObj)
        {
            if (targetObj == null) return null;

            if (!targetObj.TryGetComponent<Button>(out var btn)) btn = targetObj.AddComponent<Button>();
            if (!targetObj.TryGetComponent<ListItem>(out var listItem)) listItem = targetObj.AddComponent<ListItem>();

            listItem.EnsureButtonCached();
            return listItem;
        }

        public void Clear()
        {
            foreach (var item in currentItems)
            {
                // Only destroy it if it lives inside an active scene context (prevents deleting pure assets)
                if (item != null && item.gameObject.scene.name != null)
                    Destroy(item.gameObject);
            }
            currentItems.Clear();
            FocusedItem = null;
            ActivatedItem = null;
        }

        // --- Simplified Generation APIs ---

        public void SetOptions<TData>(IEnumerable<TData> data, System.Action<ListItem, TData> onBind, ListItem specificTemplate = null)
        {
            Clear();
            ListItem template = specificTemplate != null ? specificTemplate : defaultTemplate;
            if (template == null || contentTransform == null) return;

            foreach (var dataEntry in data)
            {
                ListItem instance = Instantiate(template, contentTransform);
                instance.gameObject.SetActive(true); // Force instance on, keeping template off
                RegisterItem(instance);
                onBind?.Invoke(instance, dataEntry);
            }
            UpdateNavigation();
        }

        public void AddCustomObjects(IEnumerable<GameObject> objects)
        {
            if (contentTransform == null) return;

            foreach (var obj in objects)
            {
                if (obj == null) continue;
                if (obj.transform.parent != contentTransform) obj.transform.SetParent(contentTransform, false);

                obj.SetActive(true);
                ListItem itemComponent = EnsureListItemRequirements(obj);
                RegisterItem(itemComponent);
            }
            UpdateNavigation();
        }

        // --- Dynamic Navigation & Scrolling Layout Systems ---

        public void UpdateNavigation()
        {
            // Clean out any null references that might have been destroyed upstream
            currentItems.RemoveAll(item => item == null);

            int count = currentItems.Count;
            if (count < 2) return; // No navigation paths to map if there's only 1 or 0 items

            for (int i = 0; i < count; i++)
            {
                ListItem item = currentItems[i];
                Button currentButton = item.TargetButton;

                if (currentButton == null) continue;

                // Wipe out past configurations cleanly via a pristine struct instantiation
                Navigation cleanNav = new Navigation
                {
                    mode = Navigation.Mode.Explicit
                };

                // Link Previous (Up) based strictly on currentItems positioning
                if (i == 0)
                {
                    cleanNav.selectOnUp = loopNavigation ? currentItems[count - 1].TargetButton : null;
                }
                else
                {
                    cleanNav.selectOnUp = currentItems[i - 1].TargetButton;
                }

                // Link Next (Down) based strictly on currentItems positioning
                if (i == count - 1)
                {
                    cleanNav.selectOnDown = loopNavigation ? currentItems[0].TargetButton : null;
                }
                else
                {
                    cleanNav.selectOnDown = currentItems[i + 1].TargetButton;
                }

                // Hard lock horizontal pathways so selection stays bound inside this specific list panel
                cleanNav.selectOnLeft = null;
                cleanNav.selectOnRight = null;

                // Apply back onto the UI Button
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

            RectTransform itemRect = item.transform as RectTransform;
            if (itemRect == null) return;

            float itemHeight = itemRect.rect.height;
            float itemYPos = itemRect.localPosition.y;
            float viewportHeight = viewport.rect.height;

            float currentContentY = contentTransform.localPosition.y - viewportHeight / 2;

            float targetTopY = currentContentY + itemYPos;
            float targetBottomY = -itemYPos + itemHeight - viewportHeight - currentContentY;

            if (targetBottomY > 0)
            {
                contentTransform.localPosition += new Vector3(0, targetBottomY, 0);
            }

            if (targetTopY > 0)
            {
                contentTransform.localPosition -= new Vector3(0, targetTopY, 0);
            }
        }
    }
}