using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// InputController controls the keys pressed during runtime and reads the text value from the console GUI.
/// </summary>
public class InputController : MonoBehaviour {

    static public InputController instance;

    [SerializeField] private InputField inputField;
    [SerializeField] private List<string> memoList;

    private readonly int memoLimit = 5;
    private int memoPointer;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        inputField.ActivateInputField();
        memoPointer = memoList.Count;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            InputParser(inputField.text);
            ClearInput();
            inputField.ActivateInputField(); // Re-select input field
            memoPointer = memoList.Count; // Restart Memo Navigation
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            NavigateMemo(KeyCode.UpArrow);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            NavigateMemo(KeyCode.DownArrow);
        }
    }

    #region Input Readers
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

            // Removing Data if Memo has reached its limit
            if (memoList.Count >= memoLimit) { memoList.RemoveAt(0); }
            // Saving Data to Memoization
            memoList.Add(string.Join(" ", inputWithoutSpaces));
        }
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

    private void ClearInput() {
        inputField.text = "";
    }

    #endregion

    #region Input Memoization

    private void NavigateMemo(KeyCode keyCode) {
        // If the list count is 0, we assume it is empty
        if(memoList.Count == 0) { return; }
        ClearInput();
        // Up: pointer - 1 | Down: pointer + 1
        if (keyCode == KeyCode.UpArrow) { memoPointer--; } else { memoPointer++; }
        // If pointer is less than zero, start at end
        if (memoPointer < 0) { memoPointer = memoList.Count - 1; }
        // If pointer is more than length, start at the beginning
        if (memoPointer >= memoList.Count) { memoPointer = 0; }
        // Set input to the older value
        inputField.text = memoList[memoPointer];
    }

    #endregion

    //private void LookUpCommand(string _action, string _object = "", string _noun = "", string _var = "") { }
}
