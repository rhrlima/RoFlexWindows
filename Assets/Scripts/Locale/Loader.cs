using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public static Loader Instance { get; private set; }
    public TextAsset englishLocale;
    public TextAsset portugueseLocale;
    public string activeLocaleCode = "en";
    private static Dictionary<string, Localization> locales;

    private void Awake()
    {
        Instance = this;

        LoadLocaleFile(englishLocale);
        LoadLocaleFile(portugueseLocale);

        Debug.Log("Locales loaded.");
    }

    public void LoadLocaleFile(TextAsset file)
    {
        locales ??= new();

        var locale = JsonUtility.FromJson<Localization>(file.text);
        locale.ParseData();
        locales[locale.languageCode] = locale;

        Debug.Log(locale);
    }

    public string GetLocalizedString(string key)
    {
        if (!locales.ContainsKey(activeLocaleCode))
            return "NULL??";

        return locales[activeLocaleCode].parsedData[key];
    }

    public void SetLocale(string localeCode)
    {
        activeLocaleCode = localeCode;
    }
}
