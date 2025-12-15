using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleTest : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    private List<string> localeCodes;

    public void Awake()
    {
        StartCoroutine(WaitLocalizationSetup());
    }

    public void OnLocaleChanged()
    {
        var index = dropdown.value;
        var localeCode = localeCodes[index];

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
        Debug.Log($"Locale changed to: {LocalizationSettings.SelectedLocale}");
    }

    private IEnumerator WaitLocalizationSetup()
    {
        yield return LocalizationSettings.InitializationOperation;

        Debug.Log("Localization initialized.");

        localeCodes = new();

        dropdown.options.Clear();
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(locale.LocaleName));
            localeCodes.Add(locale.Identifier.Code);
        }

        Debug.Log("Setup finished.");

        OnLocaleChanged();
    }
}
