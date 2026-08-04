using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-90)]
public class ItemsDatabase : MonoBehaviour
{
    public static readonly List<Item> items = new();

    public void Awake()
    {
        items.Clear();

        items.Add(new Item()
        {
            sprite = SpriteDatabase.GetSprite("item_placeholder"),
            name = "Book",
            amount = 1
        });
        items.Add(new Item()
        {
            sprite = SpriteDatabase.GetSprite("skill_placeholder"),
            name = "Skill",
            amount = 99
        });
        Debug.Log(items.Count);
    }
}
