using System;

namespace RO_Flex_UI.Binding
{
    public interface IReadOnlyBindable<out T>
    {
        T Value { get; }
        event Action<T> ValueChanged;
    }

    [Serializable]
    public class Bindable<T> : IReadOnlyBindable<T>
    {
        private T value;

        public T Value
        {
            get => value;
            set
            {
                if (Equals(this.value, value))
                    return;

                this.value = value;
                ValueChanged?.Invoke(this.value);
            }
        }

        public event Action<T> ValueChanged;

        public Bindable(T initialValue = default)
        {
            value = initialValue;
        }
    }
}