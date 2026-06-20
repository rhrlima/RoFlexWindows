using RO_Flex_UI.Components;
using TMPro;
using UnityEngine;

namespace RO_Flex_UI.Samples
{
    public class PlayerSimulator : MonoBehaviour
    {
        [Header("Player Data Object")]
        public PlayerData playerData;

        [Header("UI Components")]
        public TMP_InputField playerNameInput;
        public TMP_InputField jobNameInput;
        public RoSlider hpSlider;
        public TMP_InputField hpInput;
        public RoSlider spSlider;
        public TMP_InputField spInput;
        public RoSlider baseLvSlider;
        public TMP_InputField baseLvInput;
        public RoSlider jobLvSlider;
        public TMP_InputField jobLvInput;
        public RoSlider baseExpSlider;
        public TMP_InputField baseExpInput;
        public RoSlider jobExpSlider;
        public TMP_InputField jobExpInput;
        public RoSlider weightSlider;
        public TMP_InputField weightInput;
        public RoSlider zenySlider;
        public TMP_InputField zenyInput;

        public void Start()
        {
            playerNameInput.text = playerData.playerName;
            jobNameInput.text = playerData.jobName;

            hpInput.text = playerData.maxHp.ToString();
            hpSlider.value = (float)playerData.currentHp / playerData.maxHp;

            spInput.text = playerData.maxSp.ToString();
            spSlider.value = (float)playerData.currentSp / playerData.maxSp;

            baseLvInput.text = playerData.maxBaseLevel.ToString();
            baseLvSlider.value = (float)playerData.baseLevel / playerData.maxBaseLevel;

            jobLvInput.text = playerData.maxJobLevel.ToString();
            jobLvSlider.value = (float)playerData.jobLevel / playerData.maxJobLevel;

            baseExpInput.text = playerData.maxBaseExp.ToString();
            baseExpSlider.value = (float)playerData.currentBaseExp / playerData.maxBaseExp;

            jobExpInput.text = playerData.maxJobExp.ToString();
            jobExpSlider.value = (float)playerData.currentJobExp / playerData.maxJobExp;

            weightInput.text = playerData.maxWeight.ToString();
            weightSlider.value = (float)playerData.currentWeight / playerData.maxWeight;

            zenyInput.text = playerData.maxZeny.ToString();
            zenySlider.value = (float)playerData.currentZeny / playerData.maxZeny;
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
            int currentHp = Mathf.RoundToInt(hpSlider.value * maxHp);
            playerData.SetHP(currentHp, maxHp);
        }

        public void UpdateSP()
        {
            int maxSp = int.Parse(spInput.text);
            int currentSp = Mathf.RoundToInt(spSlider.value * maxSp);
            playerData.SetSP(currentSp, maxSp);
        }

        public void UpdateBaseLevel()
        {
            int maxBaseLevel = int.Parse(baseLvInput.text);
            int currentBaseLevel = Mathf.RoundToInt(baseLvSlider.value * maxBaseLevel);
            playerData.SetBaseLevel(currentBaseLevel, maxBaseLevel);
        }

        public void UpdateJobLevel()
        {
            int maxJobLevel = int.Parse(jobLvInput.text);
            int currentJobLevel = Mathf.RoundToInt(jobLvSlider.value * maxJobLevel);
            playerData.SetJobLevel(currentJobLevel, maxJobLevel);
        }

        public void UpdateBaseExp()
        {
            int maxBaseExp = int.Parse(baseExpInput.text);
            int currentBaseExp = Mathf.RoundToInt(baseExpSlider.value * maxBaseExp);
            playerData.SetBaseExp(currentBaseExp, maxBaseExp);
        }

        public void UpdateJobExp()
        {
            int maxJobExp = int.Parse(jobExpInput.text);
            int currentJobExp = Mathf.RoundToInt(jobExpSlider.value * maxJobExp);
            playerData.SetJobExp(currentJobExp, maxJobExp);
        }

        public void UpdateWeight()
        {
            int maxWeight = int.Parse(weightInput.text);
            int currentWeight = Mathf.RoundToInt(weightSlider.value * maxWeight);
            playerData.SetWeight(currentWeight, maxWeight);
        }

        public void UpdateZeny()
        {
            int maxZeny = int.Parse(zenyInput.text);
            int currentZeny = Mathf.RoundToInt(zenySlider.value * maxZeny);
            playerData.SetZeny(currentZeny, maxZeny);
        }
    }
}