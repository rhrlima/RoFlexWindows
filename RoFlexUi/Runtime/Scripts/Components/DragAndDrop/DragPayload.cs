using UnityEngine;

namespace RO_Flex_UI.Components.DragAndDrop
{
    public class DragPayload
    {
        public readonly IDragSource source;
        public readonly object data;
        public readonly Sprite sprite;
        public readonly string text;

        public DragPayload(IDragSource source, Sprite sprite, string text, object data)
        {
            this.source = source;
            this.sprite = sprite;
            this.text = text ?? string.Empty;
            this.data = data;
        }

        public T GetData<T>()
        {
            if (data is T tData)
            {
                return tData;
            }

            return default;
        }

        public bool TryGetData<T>(out T data)
        {
            if (this.data is T t)
            {
                data = t;
                return true;
            }

            data = default;
            return false;
        }
    }
}