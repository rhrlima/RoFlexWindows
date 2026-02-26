using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Ragnarok/Player")]
public class PlayerData : ScriptableObject
{
    public string playerName;
    public string jobName;
    public int currentHp;
    public int maxHp;
    public int currentSp;
    public int maxSp;
    public int baseLevel;
    public int maxBaseLevel;
    public int jobLevel;
    public int maxJobLevel;
    public int currentBaseExp;
    public int maxBaseExp;
    public int currentJobExp;
    public int maxJobExp;
    public int currentWeight;
    public int maxWeight;
    public int currentZeny;
    public int maxZeny;

    public event Action<string> OnPlayerNameChanged;
    public event Action<string> OnJobNameChanged;
    public event Action<int, int> OnHpChanged;
    public event Action<int, int> OnSpChanged;
    public event Action<int, int> OnBaseLevelChanged;
    public event Action<int, int> OnJobLevelChanged;
    public event Action<long, long> OnBaseExpChanged;
    public event Action<long, long> OnJobExpChanged;
    public event Action<int, int> OnWeightChanged;
    public event Action<long, long> OnZenyChanged;

    public void SetPlayerName(string name)
    {
        playerName = name;
        OnPlayerNameChanged?.Invoke(name);
    }

    public void SetJobName(string name)
    {
        jobName = name;
        OnJobNameChanged?.Invoke(name);
    }

    public void SetHP(int current, int max)
    {
        currentHp = current;
        maxHp = max;
        OnHpChanged?.Invoke(current, max);
    }

    public void SetSP(int current, int max)
    {
        currentSp = current;
        maxSp = max;
        OnSpChanged?.Invoke(current, max);
    }

    public void SetBaseLevel(int current, int max)
    {
        baseLevel = current;
        maxBaseLevel = max;
        OnBaseLevelChanged?.Invoke(current, max);
    }

    public void SetJobLevel(int current, int max)
    {
        jobLevel = current;
        maxJobLevel = max;
        OnJobLevelChanged?.Invoke(current, max);
    }

    public void SetBaseExp(int current, int max)
    {
        currentBaseExp = current;
        maxBaseExp = max;
        OnBaseExpChanged?.Invoke(current, max);
    }

    public void SetJobExp(int current, int max)
    {
        currentJobExp = current;
        maxJobExp = max;
        OnJobExpChanged?.Invoke(current, max);
    }

    public void SetWeight(int current, int max)
    {
        currentWeight = current;
        maxWeight = max;
        OnWeightChanged?.Invoke(current, max);
    }

    public void SetZeny(int current, int max)
    {
        currentZeny = current;
        maxZeny = max;
        OnZenyChanged?.Invoke(current, max);
    }
}