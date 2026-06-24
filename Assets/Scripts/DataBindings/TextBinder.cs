using UnityEngine;
using RO_Flex_UI.Components;

namespace RO_Flex_UI.Binding
{
    public class TextBinder : MonoBehaviour
    {
        [SerializeField] private RO_Text target;

        private IReadOnlyBindable<string> source;

        public void Bind(IReadOnlyBindable<string> source)
        {
            Unbind();

            this.source = source;

            if (this.source == null)
                return;

            target.Text = this.source.Value;
            this.source.ValueChanged += OnValueChanged;
        }

        public void Unbind()
        {
            if (source != null)
                source.ValueChanged -= OnValueChanged;

            source = null;
        }

        private void OnValueChanged(string value)
        {
            target.Text = value;
        }

        private void OnDisable()
        {
            Unbind();
        }
    }
}