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

    public void ChangeUIColor(Color color, bool background) {
        if (background) {
            console_BG.color = color;
        } else {
            console_InputText.color = color;
            console_InputTextPlaceholder.color = color;
            console_OutputText.color = color;
        }
    }

}
