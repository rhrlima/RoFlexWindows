using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scritable Objects/Item Data")]
[Serializable]
public class ItemData : ScriptableObject
{
    public int id;
    public new string name;
    public int amount;
}