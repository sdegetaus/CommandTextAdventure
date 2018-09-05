using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Experiment : MonoBehaviour {

    [SerializeField] private InputField inputField;

    private void InputParser(string _input) { 
        if (_input == "" || _input == " ") {
            Debug.Log("Nothing");
        } else {
            // 1. Separating each word to an Array
            string[] inputArray = _input.Split(' ');
            for (int i = 0; i < inputArray.Length; i++) {
                Regex.Replace(inputArray[i], @"\s+", "");
            }
            // 2. Deleting Nulls or Empty Elements at Array via a List
            List<string> tempList = new List<string>();
            for (int i = 0; i < inputArray.Length; i++) {
                if (!string.IsNullOrEmpty(inputArray[i])) {
                    tempList.Add(inputArray[i]);
                }
            }
            string[] inputWithoutSpaces = tempList.ToArray();
            InputLengthChecker(inputWithoutSpaces);
            tempList.Clear();
        }
        inputField.text = "";
    }

    private void InputLengthChecker(string[] _inputSplit) {
        int _inputSplitLength = _inputSplit.Length;
        switch (_inputSplitLength) {
            case 1:
                // Action
                Debug.Log("Action");
                CallCommand(_action: _inputSplit[0]);
                break;
            case 2:
                // Object, Action
                Debug.Log("Object, Action");
                CallCommand(_action: _inputSplit[1], _object: _inputSplit[0]);
                break;
            case 3:
                // Object, Action, Noun
                Debug.Log("Object, Action, Noun");
                CallCommand(_action: _inputSplit[1], _object: _inputSplit[0], _noun: _inputSplit[2]);
                break;
            case 4:
                // Object, Action, Noun, Var
                Debug.Log("Object, Action, Noun, Var");
                CallCommand(_action: _inputSplit[1], _object: _inputSplit[0], _noun: _inputSplit[2], _var: _inputSplit[3]);
                break;
            default:
                Debug.LogError("Default Case: Invalid Command");
                break;
        }
    }

    private void CallCommand(string _action, string _object = null, string _noun = null, string _var = null) {
        Debug.Log(_object + " | " + _action + " | " + _noun + " | " + _var);

        // Defensive Checker
        if (_action == null) {
            Debug.LogError("Missing Action: Invalid Command");
            return;
        }

        if (_object == null) {
            Debug.Log("_object = null"); // Go To Action
        }

        // This does the trick :)
        //Console.instance.Invoke("MyFunction", 0.0f);
    }

    //private void LookUpCommand(string _action, string _object = "", string _noun = "", string _var = "") { }

    // Temporal Attachement to Button
    public void Submit() {
        InputParser(inputField.text);
    }
}
