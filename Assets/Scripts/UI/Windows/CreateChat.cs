using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateChat : Window
{
    [SerializeField] private TMP_InputField titleInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_Dropdown limitDropdown;
    [SerializeField] private TMP_Dropdown typeDropdown;
    [SerializeField] private ToggleSwitch toggleSwitch;

    public void ShowValues()
    {
        Debug.Log($"TITLE: {titleInputField.text}");
        Debug.Log($"PASS: {passwordInputField.text}");
        Debug.Log($"LIMIT: {limitDropdown.options[limitDropdown.value].text}");
        Debug.Log($"TYPE: {typeDropdown.options[typeDropdown.value].text}");
        Debug.Log($"TOGGLE: {toggleSwitch.currentValue}");
    }
}
