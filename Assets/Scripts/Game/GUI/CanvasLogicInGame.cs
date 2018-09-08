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
        //Add playerprefs, hasPlayedAlready or something
        SetOutput("Welcome to Console TextAdventure! This game is all about exploration, only a few basic commands are going to be provided for you. What you have to know is that most commands follow this structure: <object> <.action> <noun> <variable>. \n" +
            "For example, to change the console background color, you would have to write this: console change bg #ff0ff0 \n", true);
        SetOutput("Here is a list of commands to get you started: \n", false);
        SetOutput("console help \n", false);
        SetOutput("player help \n", false);
        SetOutput("\n", false);
        // [...]
    }

    public void SetInputActive(bool active) {
        console_InputField.interactable = active;
        if (active) {
            console_InputField.ActivateInputField();
        }
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
