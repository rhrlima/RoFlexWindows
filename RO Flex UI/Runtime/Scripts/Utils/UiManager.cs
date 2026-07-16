using RO_Flex_UI.Panels;
using RO_Flex_UI.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace RO_Flex_UI.Windows
{
    public enum WindowId
    {
        NONE = 0,
        MAIN_MENU,
        PLAYER_INFO,
        INVENTORY,
        EQUIPMENT,
    }

    public struct WindowEntry
    {
        public WindowId id;
        public Window window;
        public KeyCode trigger;
    }

    public class UiManager : Singleton<UiManager>
    {
        private readonly Dictionary<WindowId, WindowEntry> windowsEntries = new();
        private readonly List<WindowId> openWindows = new();

        public void RegisterWindow(WindowId windowId, Window window, KeyCode trigger = KeyCode.None)
        {
            if (windowsEntries.ContainsKey(windowId))
                Debug.LogWarning($"Overwriting existing window with id: {windowId}");

            windowsEntries[windowId] = new WindowEntry
            {
                id = windowId,
                window = window,
                trigger = trigger,
            };

            Debug.Log($"Registered window with id: {windowId} and trigger: {trigger}");
        }

        public Window Get(WindowId windowId)
        {
            windowsEntries.TryGetValue(windowId, out var entry);

            if (entry.window == null)
                Debug.LogWarning($"Window not registered with id: {windowId}");

            return entry.window;
        }

        private void Push(WindowId windowId)
        {
            if (openWindows.Contains(windowId))
            {
                Debug.LogWarning($"Window with id: {windowId} is already in the Stack.");
                return;
            }

            openWindows.Add(windowId);
        }

        private Window Pop()
        {
            var last = openWindows.Count - 1;
            var windowId = openWindows[last];

            openWindows.RemoveAt(openWindows.Count - 1);

            return Get(windowId);
        }

        private Window Remove(WindowId windowId)
        {
            var window = Get(windowId);

            if (window != null)
                openWindows.Remove(windowId);

            return window;
        }

        private void Update()
        {
            // Priotitize closing the last opened window
            if (Input.GetKeyDown(KeyCode.Escape) && openWindows.Count > 0)
            {
                Pop().HideWindow();
                Debug.Log($"STACK {openWindows.Count}");
                return;
            }

            foreach (var entry in windowsEntries.Values)
            {
                if (Input.GetKeyDown(entry.trigger))
                {
                    Get(entry.id).ToggleVisibility();

                    if (openWindows.Contains(entry.id))
                        Remove(entry.id);
                    else
                        Push(entry.id);

                    Debug.Log($"STACK {openWindows.Count}");
                }
            }
        }
    }
}