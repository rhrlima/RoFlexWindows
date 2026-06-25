using UnityEngine;

/*
 * ROFlexUI - Tools
 * File: Tools.cs
 * Description: Utility helpers used across the ROFlexUI runtime code.
 */

namespace RO_Flex_UI.Utils
{
    /// <summary>
    /// Collection of utility helper methods for the ROFlexUI runtime.
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Returns the size of the provided RectTransform. If the rect's size is not
        /// available (<= 0) this method falls back to using sizeDelta for the missing
        /// component(s).
        /// </summary>
        /// <param name="target">The RectTransform to measure.</param>
        /// <returns>Vector2 containing width (x) and height (y).</returns>
        public static Vector2 GetRectSize(RectTransform target)
        {
            var size = target.rect.size;
            if (size.x <= 0)
                size.x = target.sizeDelta.x;

            if (size.y <= 0)
                size.y = target.sizeDelta.y;

            return size;
        }

        public static void LogMissingReference(MonoBehaviour caller, string referenceName)
        {
            Debug.LogError($"[{caller.name}] Missing reference: {referenceName}.", caller);
        }

        public static bool IsValid(MonoBehaviour caller, object obj)
        {
            if (obj == null)
            {
                LogMissingReference(caller, nameof(obj));
                return false;
            }
            return true;
        }
    }
}