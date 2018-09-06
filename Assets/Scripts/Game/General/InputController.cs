using System;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// InputController controls the keys pressed during runtime and reads the text value from the console GUI; It also keeps the input memoization.
/// </summary>
public class InputController : MonoBehaviour {

    static public InputController instance;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private List<string> memoList;

    private readonly int memoLimit = 5;
    private int memoPointer;

    //private char[] commandHooks = { ' ', '+', '-', '*', '/', '%', '='};
    
    private void Awake() {
        instance = this;
    }

    private void Start() {
        inputField.text = "";
        inputField.ActivateInputField();
        memoPointer = memoList.Count;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            InputParser(inputField.text);
            ClearInput();
            inputField.ActivateInputField();
            memoPointer = memoList.Count; // Restart Memo Navigation
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            NavigateMemo(KeyCode.UpArrow);
            inputField.ActivateInputField();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            NavigateMemo(KeyCode.DownArrow);
            inputField.ActivateInputField();
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

            List<string> arithmeticList = new List<string>();
            for (int i = 0; i < inputWithoutSpaces.Length; i++) {
                foreach(char c in inputWithoutSpaces[i]) {
                    if (c == '+' || c == '-' || c == '*' || c == '/' || c == '%' || c == '=') {
                        Debug.Log("Arithmetic Evaluation!");
                        arithmeticList.Add(c.ToString());
                    }
                    Debug.Log(c);
                }
            }


            //Debug.Log(string.Join("@", arithmeticList.ToArray()));

            //object result = new DataTable().Compute("(3+3)*2+1", null);
            //int res = Convert.ToInt32(result);
            //Debug.Log(res.ToString());

            DataTable dt = new DataTable();
            var v = dt.Compute("3 * (2+4)", "");
            Debug.Log(v.ToString());

            // Test
            //for (int i = 0; i < inputWithoutSpaces.Length; i++) {
            //    if(inputWithoutSpaces[i] == "+" || inputWithoutSpaces[i] == "-" || inputWithoutSpaces[i] == "*" || inputWithoutSpaces[i] == "/" || inputWithoutSpaces[i] == "%") {
            //        Debug.Log("Arithmetic Operation!");
            //    }
            //    if (inputWithoutSpaces[i] == "var") {
            //        Debug.Log("Variable Assigning!");
            //    }
            //}

            InputLengthChecker(inputWithoutSpaces);
            tempList.Clear();

            // Removing Data if Memo has reached its limit
            if (memoList.Count >= memoLimit) { memoList.RemoveAt(0); }
            // Saving Data to Memoization
            memoList.Add(string.Join(" ", inputWithoutSpaces)); // Add it as an Object, not string?
        }
    }

    private void InputLengthChecker(string[] _inputSplit) {
        int _inputSplitLength = _inputSplit.Length;
        CanvasLogicInGame.instance.SetOutput(string.Join(" ", _inputSplit));
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
                ConsoleResponseHandling.instance.ThrowError(ConsoleResponseHandling.ErrorType.InvalidCommand);
                break;
        }
    }

    private void CallCommand(string _action, string _object = null, string _noun = null, string _var = null) {

        // Setting first char of every string to upper (except _var)
        _action = GlobalInputParser.SetFirstCharToUpper(_action);
        _object = GlobalInputParser.SetFirstCharToUpper(_object);
        _noun = GlobalInputParser.SetFirstCharToUpper(_noun);

        string[] NounAndVar = { _noun, _var };

        try {
            if (_object == null) {
                InvokeStringMethod(_action, "Console", NounAndVar);
            } else {
                InvokeStringMethod(_action, _object, NounAndVar);
            }
        } catch {
            ConsoleResponseHandling.instance.ThrowError(ConsoleResponseHandling.ErrorType.InvalidCommand);
            return;
        }
        ConsoleResponseHandling.instance.ThrowResponse(ConsoleResponseHandling.ResponseType.Done);
        //Debug.Log(_object + " | " + _action + " | " + _noun + " | " + _var);
    }

    #endregion

    /// <summary>
    /// Invokes a Class Method with parameters. This where the magic happens :) 
    /// </summary>
    /// <param name="methodName">Method to Call</param>
    /// <param name="className">Method's Class Name</param>
    /// <param name="stringParams">Method's parameters</param>
    /// <returns>Null</returns>
    public string InvokeStringMethod(string methodName, string className = null, string[] stringParams = null) {
        Type calledType = Type.GetType(className);
        String s = (String)calledType.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null,
                    new System.Object[] { stringParams });
        return s;
    }

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
        // Set input to the selected value
        inputField.text = memoList[memoPointer];
    }

    #endregion

    private void ClearInput() {
        inputField.text = ""; // Maybe this method is superfluos...
    }

}
