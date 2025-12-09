using UnityEngine;
using UnityEngine.UI;

public class CreateChat : Window
{
    [SerializeField] private InputField titleInputField;
    [SerializeField] private InputField passwordInputField;
    [SerializeField] private Dropdown limitDropdown;
    [SerializeField] private Dropdown typeDropdown;

    public void ShowValues()
    {
        Debug.Log($"TITLE: {titleInputField.text}");
        Debug.Log($"PASS: {passwordInputField.text}");
        Debug.Log($"LIMIT: {limitDropdown.options[limitDropdown.value].text}");
        Debug.Log($"TYPE: {typeDropdown.options[typeDropdown.value].text}");
    }
}
