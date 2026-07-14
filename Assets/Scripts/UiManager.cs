using Assets.Scripts.UI.ConfigWindow;
using Assets.Scripts.UI.Hud;
using RO_Flex_UI.Panels;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Assets.Scripts.UI.Classic
{
    public enum WindowID
    {
        MAIN_MENU,
        PLAYER_INFO,
        INVENTORY,
        EQUIPMENT,
    }

    public struct WindowEntry
    {
        public WindowID id;
        public WindowBase modern;
        public Window classic;
        public KeyCode trigger;
    }

    public class UiManagerV2 : MonoBehaviorSingleton<UiManagerV2>
    {
        // TODO Using theses references only for testing the Style swap option
        // Ideally we would maintain a dict with both modern and classic windows
        // and use them according to active style
        public CharacterDetailBox playerInfoModern;
        public PlayerInfoWindow playerInfoClassic;

        private readonly Dictionary<WindowID, Window> windows = new();
        private readonly Dictionary<WindowID, WindowEntry> windows2 = new();
        private readonly List<Window> openWindows = new();

        private void Start()
        {
            Debug.Log("[UI MANAGER] Initialize.");
            GameConfig.OnGameConfigChanged += RefreshWindows;
            RefreshWindows();
        }

        public void RegisterWindow(WindowID windowId, Window window, KeyCode trigger = KeyCode.None)
        {
            if (windows.ContainsKey(windowId))
                Debug.LogWarning($"Overwriting existing window with id: {windowId}");

            windows[windowId] = window;

            windows2[windowId] = new WindowEntry
            {
                id = windowId,
                classic = window,
                trigger = trigger,
            };
        }

        public Window Get(WindowID windowId)
        {
            windows.TryGetValue(windowId, out var window);
            if (window == null)
                Debug.LogWarning($"Window not registered with id: {windowId}");

            return window;
        }

        private void Push(Window window)
        {
            openWindows.Add(window);
        }

        private Window Pop()
        {
            var last = openWindows.Count - 1;
            var window = openWindows[last];
            openWindows.RemoveAt(openWindows.Count - 1);
            return window;
        }

        private Window Remove(WindowID windowId)
        {
            var window = Get(windowId);
            openWindows.Remove(window);

            return window;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && openWindows.Count > 0)
            {
                Pop().HideWindow();
                return;
            }

            foreach (var window in windows2.Values)
            {
                if (Input.GetKeyDown(window.trigger))
                {
                    Get(window.id).ToggleVisibility();

                    if (!openWindows.Contains(window.classic))
                        Remove(window.id);
                    else
                        Push(window.classic);

                    Debug.Log($"STACK {openWindows.Count}");
                }
            }
        }

        private void RefreshWindows()
        {
            if (GameConfig.Data == null) return;

            var style = GameConfig.Data.UiStyle;
            playerInfoModern.gameObject.SetActive(style == UiStyle.Modern);
            playerInfoClassic.gameObject.SetActive(style == UiStyle.Classic);
        }
    }
}