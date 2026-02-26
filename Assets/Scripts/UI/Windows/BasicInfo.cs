using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class BasicInfo : Window
{
    public enum WindowMode
    {
        Maximized,
        Minimized
    }
    public bool isMinimized;
    [SerializeField] private SwapPanel swapPanelController;

    public PlayerData playerData;

    [Header("Maximized Components")]
    public TextMeshProUGUI playerNameTextMax;
    public TextMeshProUGUI jobNameTextMax;
    public TextMeshProUGUI baseLevelTextMax;
    public TextMeshProUGUI jobLevelTextMax;
    public Slider baseExpSlider;
    public Slider jobExpSlider;
    public Slider hpSlider;
    public TMP_Text hpText;
    public TextMeshProUGUI hpPercentageText;
    public Slider spSlider;
    public TMP_Text spText;
    public TextMeshProUGUI spPercentageText;
    public TextMeshProUGUI weightText;
    public TextMeshProUGUI zenyText;

    [Header("Minimized Components")]
    public TMP_Text min_playerNameText;
    public TMP_Text min_jobNameText;
    public TMP_Text min_baseLvText;
    public TMP_Text min_jobLvText;
    public TMP_Text min_HPText;
    public TMP_Text min_SPText;
    public TMP_Text min_baseExpText;

    private void Start()
    {
        isMinimized = false;

        playerData.OnPlayerNameChanged += UpdatePlayerName;
        playerData.OnJobNameChanged += HandleJobNameChanged;
        playerData.OnHpChanged += HandleHpChanged;
        playerData.OnSpChanged += HandleSpChanged;
        playerData.OnBaseLevelChanged += HandleBaseLevelChanged;
        playerData.OnJobLevelChanged += HandleJobLevelChanged;
        playerData.OnBaseExpChanged += HandleBaseExpChanged;
        playerData.OnJobExpChanged += HandleJobExpChanged;
        playerData.OnWeightChanged += HandleWeightChanged;
        playerData.OnZenyChanged += HandleZenyChanged;
    }

    public void ToggleMinimizeWindow()
    {
        isMinimized = !isMinimized;
        swapPanelController.GetNextGroup();

        UpdatePlayerName(playerData.playerName);
    }

    public void UpdatePlayerName(string playerName)
    {
        // maximized
        playerNameTextMax.text = playerName;

        // minimized
        min_playerNameText.text = isMinimized ? playerName : "Basic Info";
    }

    private void HandleJobNameChanged(string jobName)
    {
        // maximized
        jobNameTextMax.text = jobName;

        // minimized
        min_jobNameText.text = $"/{jobName}";
    }

    private void HandleHpChanged(int chp, int mhp)
    {
        // maximized
        hpText.text = $"{chp,6} / {mhp,6}";
        var hpPerc = (float) chp / mhp;
        hpSlider.SetValueWithoutNotify(hpPerc);
        hpPercentageText.text = $"{Mathf.RoundToInt(hpPerc * 100)}%";

        // minimized
        min_HPText.text = string.Format("HP:{0,7}/{1,5} |", chp, mhp);
    }

    private void HandleSpChanged(int csp, int msp)
    {
        // maximized
        spText.text = $"{csp,6} / {msp,6}";
        var spPerc = (float) csp / msp;
        spSlider.SetValueWithoutNotify(spPerc);
        spPercentageText.text = $"{Mathf.RoundToInt(spPerc * 100)}%";

        // minimized
        min_SPText.text = $"SP:{csp,6}/{msp,6}";
    }

    private void HandleBaseLevelChanged(int cblv, int mblv)
    {
        // maximized
        baseLevelTextMax.text = $"Base Lv. {cblv}";

        // minimized
        min_baseLvText.text = $"Lv. {cblv} /";
    }

    private void HandleJobLevelChanged(int cjlv, int mjlv)
    {
        // maximized
        jobLevelTextMax.text = $"Class Lv. {cjlv}";

        // minimized
        min_jobLvText.text = $"Lv. {cjlv} /";
    }

    private void HandleBaseExpChanged(long cbexp, long mbexp)
    {
        // maximized
        baseExpSlider.SetValueWithoutNotify((float)cbexp / mbexp);

        // minimized
        min_baseExpText.text = $"/{Mathf.RoundToInt((float)cbexp / mbexp * 100)}%";
    }

    private void HandleJobExpChanged(long cjexp, long mjexp)
    {
        // maximized
        jobExpSlider.SetValueWithoutNotify((float)cjexp / mjexp);
    }

    private void HandleWeightChanged(int cw, int mw)
    {
        // maximized
        weightText.text = $"Weight: {cw} / {mw}";
    }

    private void HandleZenyChanged(long cz, long mz)
    {
        // maximized
        zenyText.text = FormatZeny(cz);
    }

    private string FormatZeny(long amount)
    {
        return $"Zeny: {amount:N0}";
    }
}
