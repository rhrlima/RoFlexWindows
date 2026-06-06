using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Components
{
    public class SkillEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI skillNameText;
        [SerializeField] private TextMeshProUGUI skillLevelText;
        [SerializeField] private TextMeshProUGUI skillCostText;
        [SerializeField] private Button skillLevelDown;
        [SerializeField] private Button skillLevelUp;
        [SerializeField] private bool isPassive;
        [SerializeField] private bool isFixedLevel;

        private int currSkillLevel = 0;
        private int maxSkillLevel = 10;

        public void SetSkillInfo(string name, int level, int maxLevel, int cost, bool isPassive, bool isFixedLevel)
        {
            skillNameText.text = name;
            skillCostText.text = isPassive ? "Passive " : $"Sp: {cost}";
            skillLevelText.text = isFixedLevel ? $"Lv: {level}" : $"Lv: {level} / {maxLevel}";
            skillLevelDown.gameObject.SetActive(!isFixedLevel);
            skillLevelUp.gameObject.SetActive(!isFixedLevel);
        }

        private void Start()
        {
            skillLevelUp.onClick.AddListener(SkillUp);
            skillLevelDown.onClick.AddListener(SkillDown);
        }

        private void Update()
        {
            skillCostText.text = isPassive ? "Passive " : $"Sp: 999";
            skillLevelText.text = isFixedLevel ? $"Lv: {currSkillLevel,2}" : $"Lv: {currSkillLevel,2}/{maxSkillLevel,2}";
            skillLevelDown.gameObject.SetActive(!isFixedLevel);
            skillLevelUp.gameObject.SetActive(!isFixedLevel);
        }

        public void SkillUp()
        {
            currSkillLevel = Math.Min(currSkillLevel + 1, maxSkillLevel);
        }

        public void SkillDown()
        {
            currSkillLevel = Math.Max(currSkillLevel - 1, 0);
        }
    }
}