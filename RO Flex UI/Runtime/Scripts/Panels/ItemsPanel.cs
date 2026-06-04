using RO_Flex_UI.Components;
using RO_Flex_UI.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Panels
{
    // TODO add custom editor
    [RequireComponent(typeof(GridLayoutGroup), typeof(RectTransform))]
    public class ItemsPanel : MonoBehaviour
    {
        [Header("Panel References")]
        [SerializeField] private RectTransform windowRect;
        [Tooltip("Border size to the content panel.")]
        [SerializeField] private RectOffset windowOffset; // FIXME calculate this
        [SerializeField] private RectTransform panelRect;
        [SerializeField] private RectTransform viewportRect;
        private GridLayoutGroup gridLayout;
        private Resizable resizable;

        [Space]
        [SerializeField] public ItemEntry slotPrefab;

        [Space]
        [Header("Grid Config")]
        [SerializeField] private int maxSlots = 100;
        [SerializeField] private int numItems; //TODO wire to Scriptable objects on item info
        public int NumItems => numItems;
        [SerializeField] private int MinColumns = 5;
        [SerializeField] private int MaxColumns = 10;
        [SerializeField] private int MinRows = 5;
        [SerializeField] private int MaxRows = 10;

        private int numSlots;
        private List<ItemEntry> items;
        private bool isUpdatingGrid = false;
        private bool hasPendingGridChange;
        private Vector2 lastWindowSize;

        public void Start()
        {
            EnsureReferences();
            SetMinMaxSize();

            StartCoroutine(InitializeGrid());
        }

        private IEnumerator InitializeGrid()
        {
            yield return null;

            if (panelRect != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(panelRect);
            }

            OnGridChange();
        }

        private void Update()
        {
            if (panelRect != null && Tools.GetRectSize(panelRect) != lastWindowSize)
                hasPendingGridChange = true;

            if (!hasPendingGridChange)
                return;

            hasPendingGridChange = false;
            OnGridChange();
        }

        private void UpdateGrid()
        {
            if (slotPrefab == null)
                return;

            EnsureSlotPool();

            var visibleCount = Mathf.Min(items.Count, numSlots);
            for (var i = 0; i < visibleCount; i++)
            {
                items[i].gameObject.SetActive(true);
                items[i].itemAmount = i < numItems ? 1 : 0;
                items[i].Refresh();
            }

            for (var i = visibleCount; i < items.Count; i++)
            {
                items[i].gameObject.SetActive(false);
            }
        }

        public void OnGridChange()
        {
            // try to avoid redo all for disabled panels
            if (!isActiveAndEnabled || isUpdatingGrid)
                return;

            if (panelRect == null || !panelRect.gameObject.activeSelf)
                return;

            lastWindowSize = Tools.GetRectSize(panelRect);
            isUpdatingGrid = true;

            try
            {
                CalcTotalSlots();
                UpdateGrid();
            }
            finally
            {
                isUpdatingGrid = false;
            }
        }

        private void OnRectTransformDimensionsChange()
        {
            hasPendingGridChange = true;
        }

        private void CalcTotalSlots()
        {
            var viewportSize = Tools.GetRectSize(viewportRect);
            var padding = gridLayout.padding;
            var cellSize = gridLayout.cellSize;
            var spacing = gridLayout.spacing;

            var availableWidth = viewportSize.x - (padding.left + padding.right);
            var availableHeight = viewportSize.y - (padding.top + padding.bottom);

            var columns = Mathf.Clamp(Mathf.FloorToInt((availableWidth + spacing.x) / (cellSize.x + spacing.x)), MinColumns, MaxColumns);
            var rows = Mathf.FloorToInt((availableHeight + spacing.y) / (cellSize.y + spacing.y));

            // ensure there are enough slots to display full rows for all items
            var itemsFullRows = Mathf.CeilToInt((float)numItems / Mathf.Max(1, columns)) * columns;
            numSlots = Mathf.Max(rows * columns, itemsFullRows);
        }

        private void EnsureReferences()
        {
            items ??= new List<ItemEntry>();

            if (panelRect == null)
                panelRect = GetComponent<RectTransform>();

            if (gridLayout == null)
                gridLayout = GetComponent<GridLayoutGroup>();

            if (windowRect == null)
                windowRect = GetComponentInParent<IWindow>(true)?.transform;

            if (resizable == null && windowRect != null)
                resizable = windowRect.GetComponentInChildren<Resizable>(true);

            if (slotPrefab != null && slotPrefab.transform.parent == transform)
                slotPrefab.gameObject.SetActive(false);
        }

        private void EnsureSlotPool()
        {
            var targetSlots = Mathf.Min(GetMaxSlots(), Mathf.Max(1, numSlots));

            while (items.Count < targetSlots)
            {
                var item = Instantiate(slotPrefab, transform);
                item.gameObject.SetActive(false);
                items.Add(item);
            }
        }

        private int GetMaxSlots()
        {
            return Mathf.Max(1, maxSlots);
        }

        private void SetMinMaxSize()
        {
            if (windowRect == null || resizable == null)
                return;

            var padding = gridLayout.padding;
            var cellSize = gridLayout.cellSize;
            var spacing = gridLayout.spacing;

            var minWin = new Vector2(
                MinColumns * cellSize.x + (MinColumns - 1) * spacing.x + padding.horizontal + windowOffset.horizontal,
                MinRows * cellSize.y + (MinRows - 1) * spacing.y + padding.vertical + windowOffset.vertical);

            var maxWin = new Vector2(
                MaxColumns * cellSize.x + (MaxColumns - 1) * spacing.x + padding.horizontal + windowOffset.horizontal,
                MaxRows * cellSize.y + (MaxRows - 1) * spacing.y + padding.vertical + windowOffset.vertical);

            resizable.MinSize = minWin;
            resizable.MaxSize = maxWin;
            windowRect.sizeDelta = minWin;
        }
    }
}