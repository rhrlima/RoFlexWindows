using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scritable Objects/Skill Data")]
[Serializable]
public class SkillData : ScriptableObject
{
    public int id;
    public new string name;
    public int currLevel;
    public int maxLevel;
    public int cost;
    public bool passive;
    public bool fixedLevel;
}