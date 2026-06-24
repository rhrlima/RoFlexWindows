using RO_Flex_UI.Components;
using RO_Flex_UI.Utils;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Panels
{
    public class SkillBarPanel : MonoBehaviour, IPanel
    {
        [Serializable]
        private class SkillBarLine
        {
            public int id;
            // public bool visible;
            public GameObject bar;
            public List<IconAmount> slots;
        }

        [Header("References")]
        [SerializeField] private IconAmount slotTemplate;
        [SerializeField] private GameObject barTemplate; // FIXME enforce type
        [SerializeField] private GameObject rightPanel;
        private Resizable resizable;

        [Header("Configurations")]
        [SerializeField, Min(1)] private int numSlots;
        [SerializeField, Min(1)] private int numBars;
        [SerializeField] private List<SkillBarLine> bars;

        private void Start()
        {
            if (!EnsureReferences()) return;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            slotTemplate.gameObject.SetActive(false);
            barTemplate.gameObject.SetActive(false);

            for (var i = 0; i < numBars; i++)
            {
                var bar = Instantiate(barTemplate, transform);
                bar.gameObject.SetActive(true);

                bars.Add(new SkillBarLine()
                {
                    id = i,
                    bar = bar,
                    slots = new()
                });

                var label = bar.GetComponentInChildren<TMP_Text>(true);
                if (label != null)
                    label.text = string.Format("{0}", i + 1);

                for (var j = 0; j < numSlots; j++)
                {
                    var slot = Instantiate(slotTemplate, bars[i].bar.transform);
                    slot.gameObject.SetActive(true);
                    bars[i].slots.Add(slot);
                }
            }

            rightPanel.transform.SetAsLastSibling();

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(bars[0].bar.transform as RectTransform);

            var minSize = Tools.GetRectSize(bars[0].bar.transform as RectTransform);

            resizable.MinSize = minSize;
            resizable.MaxSize = new Vector2(minSize.x, minSize.y * bars.Count);
            resizable.StepSize = new Vector2(0, minSize.y);
        }

        private bool EnsureReferences()
        {
            if (slotTemplate == null)
            {
                Debug.Log($"[{name}] Missing prefab assigned to slotTemplate.");
                return false;
            }
            if (barTemplate == null)
            {
                Debug.Log($"[{name}] Missing prefab assigned to barTemplate.");
                return false;
            }
            if (rightPanel == null)
            {
                Debug.Log($"[{name}] Missing component assigned to rightPanel.");
                return false;
            }
            if (barTemplate.GetComponent<Transform>() == null)
            {
                Debug.Log($"[{name}] Prefab assigned to barTemplate does not have a Transform component.");
                return false;
            }
            resizable = GetComponentInChildren<Resizable>(true);
            if (resizable == null)
            {
                Tools.LogMissingReference(this, nameof(resizable));
                return false;
            }
            return true;
        }

        private void Update()
        {
            var rectSize = Tools.GetRectSize(transform as RectTransform);
            foreach (var bar in bars)
            {
                bar.bar.gameObject.SetActive(bar.id < rectSize.y / 34);
            }
        }

        bool IPanel.EnsureReferences()
        {
            return EnsureReferences();
        }
    }
}