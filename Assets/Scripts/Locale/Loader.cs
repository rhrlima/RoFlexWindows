using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class Loader : MonoBehaviour
{
    public void SetLocale(string localeCode)
    {
        StartCoroutine(ChangeLocaleCoroutine(localeCode));
    }

    private IEnumerator ChangeLocaleCoroutine(string localeCode)
    {
        yield return LocalizationSettings.InitializationOperation;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);

        Debug.Log($"Locale changed to: {LocalizationSettings.SelectedLocale}");
    }
}
