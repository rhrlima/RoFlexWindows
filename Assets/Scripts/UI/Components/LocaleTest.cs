using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocaleTest : MonoBehaviour, ILocalizable
{
    public TMP_Dropdown dropdown;
    public TMP_Text message;
    public TMP_Text confirm;
    public TMP_Text cancel;

    private void Start()
    {
        OnLocaleChanged();
    }

    public void OnLocaleChanged()
    {
        if (Loader.Instance == null) return;

        var localeIndex = dropdown.value;

        if (localeIndex == 0)
        {
            Loader.Instance.SetLocale("en");
        }
        else if (localeIndex == 1)
        {
            Loader.Instance.SetLocale("pt-BR");
        }
        else
        {
            Loader.Instance.SetLocale("es");
        }

        // message.text = Loader.Instance.GetLocalizedString("test.message");
        // confirm.text = Loader.Instance.GetLocalizedString("test.confirm");
        // cancel.text = Loader.Instance.GetLocalizedString("test.cancel");
    }
}
