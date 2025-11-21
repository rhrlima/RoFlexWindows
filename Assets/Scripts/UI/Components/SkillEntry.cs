using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillEntry : MonoBehaviour
{
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillLevelText;
    public TextMeshProUGUI skillCostText;
    public Button skillLevelDown;
    public Button skillLevelUp;
    public bool isPassive;
    public bool isFixedLevel;

    public void SetSkillInfo(string name, int level, int maxLevel, int cost, bool isPassive, bool isFixedLevel)
    {
        skillNameText.text = name;
        skillCostText.text = isPassive ? "Passive " : $"Sp: {cost}";
        skillLevelText.text = isFixedLevel ? $"Lv: {level}" : $"Lv: {level} / {maxLevel}";
        skillLevelDown.gameObject.SetActive(!isFixedLevel);
        skillLevelUp.gameObject.SetActive(!isFixedLevel);
    }
}
