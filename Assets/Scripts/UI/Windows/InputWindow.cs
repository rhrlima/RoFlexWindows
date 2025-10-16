using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputWindow : Window
{
    public delegate void InputCallback(string inputValue);
    private InputCallback currentCallback;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button confirmBtn;

    protected override void Awake()
    {
        base.Awake();

        if (inputField == null)
            Debug.LogError("No inputField assigned.");

        if (confirmBtn == null)
            Debug.LogError("No confirm button found.");

        confirmBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.AddListener(OnConfirm);
    }

    public void ShowWindow(InputCallback callback, string initialText = "", int inputLimit = 30, bool onlyNumbers = false)
    {
        ShowWindow();

        currentCallback = callback;

        inputField.text = initialText;
        inputField.characterLimit = inputLimit;
        inputField.onValidateInput = (string text, int charIndex, char addedChar) =>
        {
            return Validate(addedChar, onlyNumbers);
        };

        inputField.Select();
        inputField.ActivateInputField();

    }

    private void OnConfirm()
    {
        Debug.Log($"INTERNAL CALLBACK: {inputField.text}");

        currentCallback?.Invoke(inputField.text);

        HideWindow();
        currentCallback = null;
    }

    private char Validate(char addedChar, bool onlyNumbers)
    {
        if (onlyNumbers)
        {
            return Char.IsDigit(addedChar) ? addedChar : '\0';
        }

        return addedChar;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnConfirm();
        }
    }
}
