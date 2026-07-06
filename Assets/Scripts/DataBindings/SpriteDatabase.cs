using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class SpriteDatabase : MonoBehaviour
{
    private static readonly Dictionary<string, Sprite> spritesDict = new();
    public List<Sprite> sprites;

    private void Awake()
    {
        foreach (var sprite in sprites)
        {
            Debug.Log($"{sprite.name} - {sprite}");
            spritesDict[sprite.name] = sprite;
        }
    }

    public static Sprite GetSprite(string name)
    {
        return spritesDict[name];
    }
}
