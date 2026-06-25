using RO_Flex_UI.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    public class SkillEntry : MonoBehaviour, IComponent
    {
        public class SkillEvent : UnityEvent { }

        [SerializeField] private TextMeshProUGUI skillNameText;
        [SerializeField] private TextMeshProUGUI skillLevelText;
        [SerializeField] private TextMeshProUGUI skillCostText;
        [SerializeField] private Button skillLevelDown;
        [SerializeField] private Button skillLevelUp;
        [SerializeField] private bool isPassive;
        [SerializeField] private bool isFixedLevel;

        public SkillEvent onSkillLevelUp;
        public SkillEvent onIncreaseLevel;
        public SkillEvent onDecreaseLevel;

        private void Awake()
        {
            if (!EnsureReferences()) return;
        }
        public bool EnsureReferences()
        {
            if (Tools.IsValid(this, skillNameText)) return false; //TODO good?

            if (skillLevelText == null)
            {
                Tools.LogMissingReference(this, nameof(skillLevelText));
                return false;
            }
            if (skillCostText == null)
            {
                Tools.LogMissingReference(this, nameof(skillCostText));
                return false;
            }
            if (skillLevelDown == null)
            {
                Tools.LogMissingReference(this, nameof(skillLevelDown));
                return false;
            }
            if (skillLevelUp == null)
            {
                Tools.LogMissingReference(this, nameof(skillLevelUp));
                return false;
            }
            return true;
        }

        private void OnEnable()
        {
            skillLevelUp.onClick.AddListener(HandleIncreaseLevel);
            skillLevelDown.onClick.AddListener(HandleDecreaseLevel);
        }
        private void OnDisable()
        {
            skillLevelUp.onClick.RemoveListener(HandleIncreaseLevel);
            skillLevelDown.onClick.RemoveListener(HandleDecreaseLevel);
        }
        public void HandleIncreaseLevel()
        {
            onIncreaseLevel?.Invoke();
        }

        public void HandleDecreaseLevel()
        {
            onDecreaseLevel?.Invoke();
        }

        #region Getter & Setter
        public string Name
        {
            get => skillNameText.text;
            set => skillNameText.text = value;
        }
        public string Level
        {
            get => skillLevelText.text;
            set => skillLevelText.text = value;
        }
        public string Cost
        {
            get
            {
                if (isPassive) return "Passive";

                return skillCostText.text;
            }
            set
            {
                if (isPassive)
                {
                    skillCostText.text = "Passive";
                    return;
                }

                skillCostText.text = value;
            }
        }
        public bool IsPassive
        {
            get => isPassive;
            set => isPassive = value;
        }
        public bool IsFixedLevel
        {
            get => isFixedLevel;
            set => isFixedLevel = value;
        }
        #endregion
    }
}