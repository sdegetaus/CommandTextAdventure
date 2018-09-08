using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasLogicInGame : MonoBehaviour {

    static public CanvasLogicInGame instance;

    [SerializeField] private Image console_BG;
    [SerializeField] private TMP_InputField console_InputField;
    [SerializeField] private TMP_Text console_InputText;
    [SerializeField] private TMP_Text console_InputTextPlaceholder;
    [SerializeField] private TMP_Text console_OutputText;
    [SerializeField] private TMP_Text console_BracketText;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        SetOutput("Welcome Message!", true);
    }

    public string GetInputText() {
        return console_InputField.text;
    }

    public void ActivateInputField() {
        console_InputField.ActivateInputField();
    }

    public void SetCaretToEnd() {
        console_InputField.caretPosition = GetInputText().Length;
    }

    public void SetInput(string value) {
        console_InputField.text = value;
    }

    public void SetOutput(string value, bool addLineCharacters = true) {
        if (addLineCharacters) {
            console_OutputText.text += "> " + value + "\n";
        } else {
            console_OutputText.text += value;
        }
    }

    public void ClearInput() {
        console_InputField.text = "";
    }

    public void ClearOutput() {
        console_OutputText.text = "";
    }

    public void SetUIColors(Color color, bool background) {
        if (background) {
            console_BG.color = color;
        } else {
            console_InputText.color = color;
            console_InputTextPlaceholder.color = color;
            console_OutputText.color = color;
            console_BracketText.color = color;
        }
    }

    public void InvertUIColors() {
        console_BG.color = new Color(Mathf.Abs(console_BG.color.r - 1), Mathf.Abs(console_BG.color.g - 1), Mathf.Abs(console_BG.color.b - 1));
        Color textInverted = new Color(Mathf.Abs(console_InputText.color.r - 1), Mathf.Abs(console_InputText.color.g - 1), Mathf.Abs(console_InputText.color.b - 1));
        console_InputText.color = textInverted;
        console_InputTextPlaceholder.color = textInverted;
        console_OutputText.color = textInverted;
        console_BracketText.color = textInverted;
    }

}
