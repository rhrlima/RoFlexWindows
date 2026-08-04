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

        [SerializeField] protected RoButton spriteButton;
        [SerializeField] private TextMeshProUGUI skillNameText;
        [SerializeField] private TextMeshProUGUI skillLevelText;
        [SerializeField] private TextMeshProUGUI skillCostText;
        [SerializeField] private Button skillLevelDown; //FIXME use RoButton
        [SerializeField] private Button skillLevelUp; //FIXME use RoButton
        [SerializeField] private bool isPassive;
        [SerializeField] private bool isFixedLevel;

        public SkillEvent onSkillClick;
        public SkillEvent onSkillLevelUp;
        public SkillEvent onIncreaseLevel;
        public SkillEvent onDecreaseLevel;

        private void Awake()
        {
            if (!EnsureReferences()) return;
        }

        public bool EnsureReferences()
        {
            if (!Tools.IsValid(this, skillNameText)) return false;
            if (!Tools.IsValid(this, skillLevelText)) return false;
            if (!Tools.IsValid(this, skillCostText)) return false;
            if (!Tools.IsValid(this, skillLevelDown)) return false;
            if (!Tools.IsValid(this, skillLevelUp)) return false;
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

        public void Assign(Sprite sprite, string name, int currLevel, int maxLevel, int cost, bool passive, bool fixedLevel)
        {
            spriteButton.image.sprite = sprite;
            skillNameText.text = name;

            if (fixedLevel)
            {
                skillLevelText.text = $"{currLevel}";
            }
            else
            {
                skillLevelText.text = $"{currLevel}/{maxLevel}";
            }

            skillLevelDown.gameObject.SetActive(!fixedLevel);
            skillLevelUp.gameObject.SetActive(!fixedLevel);

            skillCostText.text = passive ? "Passive" : cost.ToString();
        }

        #region Getter & Setter
        public Sprite Sprite => spriteButton.image.sprite;
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