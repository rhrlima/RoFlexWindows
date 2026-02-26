using System;
using TMPro;
using UnityEngine;

public class PlayerDataUI : MonoBehaviour
{
    [Header("Player Data Object")]
    public PlayerData playerData;

    [Header("UI Components")]
    public TMP_InputField playerNameInput;
    public TMP_InputField jobNameInput;
    public ROSlider hpSlider;
    public TMP_InputField hpInput;
    public ROSlider spSlider;
    public TMP_InputField spInput;
    public ROSlider baseLvSlider;
    public TMP_InputField baseLvInput;
    public ROSlider jobLvSlider;
    public TMP_InputField jobLvInput;
    public ROSlider baseExpSlider;
    public TMP_InputField baseExpInput;
    public ROSlider jobExpSlider;
    public TMP_InputField jobExpInput;
    public ROSlider weightSlider;
    public TMP_InputField weightInput;
    public ROSlider zenySlider;
    public TMP_InputField zenyInput;

    public void Start()
    {
        playerNameInput.text = playerData.playerName;
        jobNameInput.text = playerData.jobName;

        hpInput.text = playerData.maxHp.ToString();
        hpSlider.Value = (float)playerData.currentHp / playerData.maxHp;

        spInput.text = playerData.maxSp.ToString();
        spSlider.Value = (float)playerData.currentSp / playerData.maxSp;

        baseLvInput.text = playerData.baseLevel.ToString();
        jobLvInput.text = playerData.jobLevel.ToString();

        baseExpInput.text = playerData.maxBaseExp.ToString();
        baseExpSlider.Value = (float)playerData.currentBaseExp / playerData.maxBaseExp;

        jobExpInput.text = playerData.maxJobExp.ToString();
        jobExpSlider.Value = (float)playerData.currentJobExp / playerData.maxJobExp;

        weightInput.text = playerData.maxWeight.ToString();
        weightSlider.Value = (float)playerData.currentWeight / playerData.maxWeight;

        zenyInput.text = playerData.maxZeny.ToString();
        zenySlider.Value = (float)playerData.currentZeny / playerData.maxZeny;
    }

    public void UpdatePlayerName()
    {
        playerData.SetPlayerName(playerNameInput.text);
    }

    public void UpdateJobName()
    {
        playerData.SetJobName(jobNameInput.text);
    }

    public void UpdateHP()
    {
        int maxHp = int.Parse(hpInput.text);
        int currentHp = Mathf.RoundToInt(hpSlider.Value * maxHp);
        playerData.SetHP(currentHp, maxHp);
    }

    public void UpdateSP()
    {
        int maxSp = int.Parse(spInput.text);
        int currentSp = Mathf.RoundToInt(spSlider.Value * maxSp);
        playerData.SetSP(currentSp, maxSp);
    }

    public void UpdateBaseLevel()
    {
        int maxBaseLevel = int.Parse(baseLvInput.text);
        int currentBaseLevel = Mathf.RoundToInt(baseLvSlider.Value * maxBaseLevel);
        playerData.SetBaseLevel(currentBaseLevel, maxBaseLevel);
    }

    public void UpdateJobLevel()
    {
        int maxJobLevel = int.Parse(jobLvInput.text);
        int currentJobLevel = Mathf.RoundToInt(jobLvSlider.Value * maxJobLevel);
        playerData.SetJobLevel(currentJobLevel, maxJobLevel);
    }

    public void UpdateBaseExp()
    {
        int maxBaseExp = int.Parse(baseExpInput.text);
        int currentBaseExp = Mathf.RoundToInt(baseExpSlider.Value * maxBaseExp);
        playerData.SetBaseExp(currentBaseExp, maxBaseExp);
    }

    public void UpdateJobExp()
    {
        int maxJobExp = int.Parse(jobExpInput.text);
        int currentJobExp = Mathf.RoundToInt(jobExpSlider.Value * maxJobExp);
        playerData.SetJobExp(currentJobExp, maxJobExp);
    }

    public void UpdateWeight()
    {
        int maxWeight = int.Parse(weightInput.text);
        int currentWeight = Mathf.RoundToInt(weightSlider.Value * maxWeight);
        playerData.SetWeight(currentWeight, maxWeight);
    }

    public void UpdateZeny()
    {
        int maxZeny = int.Parse(zenyInput.text);
        int currentZeny = Mathf.RoundToInt(zenySlider.Value * maxZeny);
        playerData.SetZeny(currentZeny, maxZeny);
    }
}
