using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    [RequireComponent(typeof(Image))]
    public class RoButton : Button, IComponent
    {
        protected override void Awake()
        {
            base.Awake();
            if (!EnsureReferences()) return;
        }

        public virtual bool EnsureReferences()
        {
            return true;
        }

    }
}