using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// [ExecuteAlways]
public class BasicInfo : Window
{
    [Header("Swap Panels")]
    public List<GameObject> panelsA;
    public List<GameObject> panelsB;
    public bool isMinimized = false;

    [Header("Maximized Components")]
    public TextMeshProUGUI playerNameTextMax;
    public TextMeshProUGUI jobNameTextMax;
    public TextMeshProUGUI baseLevelTextMax;
    public TextMeshProUGUI jobLevelTextMax;
    public Slider baseExpSlider;
    public Slider jobExpSlider;
    public Slider hpSlider;
    public TextMeshProUGUI hpPercentageText;
    public Slider spSlider;
    public TextMeshProUGUI spPercentageText;
    public TextMeshProUGUI weightText;
    public TextMeshProUGUI zenyText;

    [Header("Minimized Components")]
    public TextMeshProUGUI playerNameTextMin;
    public TextMeshProUGUI basicInfoText;

    [Header("Player Info")]   
    public string playerName;
    public string jobName;
    public int baseLevel;
    public int jobLevel;
    public int currentBaseExp;
    public int maxBaseExp;
    public int currentJobExp;
    public int maxJobExp;
    public int currentHP;
    public int maxHP;
    public int currentSP;
    public int maxSP;
    public int currentWeight;
    public int maxWeight;
    public int currentZeny;

    public int BaseLevel
    {
        get { return baseLevel; }
        set
        {
            baseLevel = value;
            UpdateUI();
        }
    }

    public void Start()
    {
        SwapPanels();
    }

    public void SwapPanels()
    {

        var count = Mathf.Max(panelsA.Count, panelsB.Count);

        for (int i = 0; i < count; i++)
        {
            bool isActive = panelsA[i].activeSelf;

            if (i < panelsA.Count) panelsA[i].SetActive(!isActive);
            if (i < panelsB.Count) panelsB[i].SetActive(isActive);
        }

        isMinimized = !isMinimized;
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Maximized
        playerNameTextMax.text = playerName;
        jobNameTextMax.text = jobName;
        baseLevelTextMax.text = $"Base Lv. {baseLevel}";
        baseExpSlider.value = (float) currentBaseExp / maxBaseExp;
        jobLevelTextMax.text = $"Class Lv. {jobLevel}";
        jobExpSlider.value = (float) currentJobExp / maxJobExp;
        var hpPerc = (float) currentHP / maxHP;
        // hpSlider.value = hpPerc;
        hpSlider.SetValueWithoutNotify(hpPerc);
        hpPercentageText.text = $"{Mathf.RoundToInt(hpPerc * 100)}%";
        var spPerc = (float) currentSP / maxSP;
        // spSlider.value = spPerc;
        spSlider.SetValueWithoutNotify(spPerc);
        spPercentageText.text = $"{Mathf.RoundToInt(spPerc * 100)}%";
        weightText.text = $"Weight: {currentWeight} / {maxWeight}";
        zenyText.text = $"Zeny: {currentZeny}";

        // Minimized
        playerNameTextMin.text = isMinimized ? playerName : "Basic Info";
        basicInfoText.text = $"Lv. {baseLevel} / {jobName} / Lv. {jobLevel} / {(float)currentBaseExp/maxBaseExp*100}%\nHP. {currentHP} / {maxHP} | SP. {currentSP} / {maxSP}";
    }
}
