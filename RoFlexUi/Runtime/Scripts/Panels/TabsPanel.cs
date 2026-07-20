using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace RO_Flex_UI.Panels
{
    public class TabsPanel : MonoBehaviour
    {
        [Serializable]
        public class TabEvent : UnityEvent<int> { }

        [Serializable]
        private class Entry
        {
            public bool active;
            public TabButton tabButton;
            public GameObject tabPanel;
            [FormerlySerializedAs("onTabEnter")]
            public TabEvent onPanelEnter = new();
            [FormerlySerializedAs("onTabExit")]
            public TabEvent onPanelExit = new();
        }

        [SerializeField] private List<Entry> entries = new();
        [SerializeField] private int defaultTabIndex;
        [SerializeField] private bool selectDefaultOnStart = true;

        private int currentIndex = -1;
        public int CurrentIndex => currentIndex;

        private void Start()
        {
            RegisterTabButtons();
            InitializeTabs();
        }

        private void RegisterTabButtons()
        {
            if (entries == null)
                return;

            for (var i = 0; i < entries.Count; i++)
            {
                var index = i;
                var entry = entries[index];

                if (entry?.tabButton == null)
                    continue;

                entry.tabButton.onClick.AddListener(() => SetActiveTab(index));
            }
        }

        private void InitializeTabs()
        {
            currentIndex = -1;

            if (entries == null)
                return;

            foreach (var entry in entries)
                SetEntryActive(entry, false);

            if (selectDefaultOnStart)
                SetActiveTab(defaultTabIndex);
        }

        public void SetActiveTab(int index)
        {
            if (entries == null)
                return;

            if (index < 0 || index >= entries.Count || index == currentIndex)
                return;

            if (currentIndex >= 0 && currentIndex < entries.Count)
            {
                var currentEntry = entries[currentIndex];
                currentEntry?.onPanelExit?.Invoke(currentIndex);
                SetEntryActive(currentEntry, false);
            }

            currentIndex = index;

            var nextEntry = entries[currentIndex];
            SetEntryActive(nextEntry, true);
            nextEntry?.onPanelEnter?.Invoke(currentIndex);
        }

        private static void SetEntryActive(Entry entry, bool active)
        {
            if (entry == null)
                return;

            entry.active = active;

            if (entry.tabPanel != null)
                entry.tabPanel.SetActive(active);

            if (entry.tabButton != null)
                entry.tabButton.SetActive(active);
        }
    }
}