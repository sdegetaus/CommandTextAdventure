using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OutputController : MonoBehaviour {

    static public OutputController instance;

    [SerializeField] private TMP_InputField inputRT;
    [SerializeField] private TMP_Text outputRT;

    [SerializeField] private RectTransform consoleMargins;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        outputRT.text = "";
        inputRT.text = "";
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Return)) {
            outputRT.text += "\n" + inputRT.text;
            inputRT.text = "";
            inputRT.ActivateInputField();
        }
    }

}
