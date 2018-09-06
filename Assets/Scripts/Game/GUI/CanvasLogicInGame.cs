using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasLogicInGame : MonoBehaviour {

    static public CanvasLogicInGame instance;

    [SerializeField] private Image console_BG;
    [SerializeField] private TMP_Text console_InputText;
    [SerializeField] private TMP_Text console_InputTextPlaceholder;
    [SerializeField] private TMP_Text console_OutputText;

    private void Awake() {
        instance = this;
    }

    public void SetOutput(string value) {
        console_OutputText.text += "> " + value + "\n";
    }

    //public void ClearInput() {
    //    console_OutputText.text = "";
    //}

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
        }
    }

    public void InvertUIColors() {
        console_BG.color = new Color(Mathf.Abs(console_BG.color.r - 1), Mathf.Abs(console_BG.color.g - 1), Mathf.Abs(console_BG.color.b - 1));
        Color textInverted = new Color(Mathf.Abs(console_InputText.color.r - 1), Mathf.Abs(console_InputText.color.g - 1), Mathf.Abs(console_InputText.color.b - 1));
        console_InputText.color = textInverted;
        console_InputTextPlaceholder.color = textInverted;
        console_OutputText.color = textInverted;
    }

}
