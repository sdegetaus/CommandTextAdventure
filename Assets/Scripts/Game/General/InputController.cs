using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// InputController controls the keys pressed during runtime and reads the text value from the console GUI; It also keeps the input memoization.
/// </summary>
public class InputController : MonoBehaviour {

    static public InputController instance;

    [SerializeField] private List<string> memoList;
    private readonly int memoLimit = 5;
    private int memoPointer;

    private OutputController outputController;
    private ResponseHandling responseHandling;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        outputController = OutputController.instance;
        responseHandling = ResponseHandling.instance;

        outputController.ClearInput();
        outputController.ActivateInputField();
        memoPointer = memoList.Count;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            InputParser(outputController.GetInputText());
            outputController.ClearInput();
            outputController.ActivateInputField();
            memoPointer = memoList.Count; // Restart Memo Navigation
            outputController.CheckForOutputLimit(); // Check output size
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            NavigateMemo(KeyCode.UpArrow);
            outputController.ActivateInputField();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            NavigateMemo(KeyCode.DownArrow);
            outputController.ActivateInputField();
        }
        if (Input.GetMouseButtonDown(0)) {
            outputController.ActivateInputField();
            outputController.SetCaretToEnd(false);
        }
    }

    #region Input Readers

    /// <summary>
    /// Deletes all the spaces that the player input, saves to memo. Basically, cleans the input for accurate reading and calling.
    /// </summary>
    /// <param name="_input"></param>
    private void InputParser(string _input) {
        if (_input == "" || _input == " ") {
            return;
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

    /// <summary>
    /// Establishes the kind of command that is being parsed, i.e. one word = action; two: object and action.
    /// </summary>
    /// <param name="_inputSplit"></param>
    private void InputLengthChecker(string[] _inputSplit) {
        int _inputSplitLength = _inputSplit.Length;
        outputController.SetOutput(string.Join(" ", _inputSplit));
        switch (_inputSplitLength)
        {
            case 1:
                // Action
                CallCommand(_action: _inputSplit[0]);
                break;
            case 2:
                // Object, Action
                CallCommand(_action: _inputSplit[1], _object: _inputSplit[0]);
                break;
            case 3:
                // Object, Action, Noun
                CallCommand(_action: _inputSplit[1], _object: _inputSplit[0], _noun: _inputSplit[2]);
                break;
            case 4:
                // Object, Action, Noun, Var
                CallCommand(_action: _inputSplit[1], _object: _inputSplit[0], _noun: _inputSplit[2], _var: _inputSplit[3]);
                break;
            default:
                // More: throw error
                responseHandling.ThrowError(ErrorType.InvalidCommand);
                break;
        }
    }

    /// <summary>
    /// Calls the command throughout the project. 
    /// </summary>
    /// <param name="_action"></param>
    /// <param name="_object"></param>
    /// <param name="_noun"></param>
    /// <param name="_var"></param>
    private void CallCommand(string _action, string _object = null, string _noun = null, string _var = null) {

        // ToDo
        //if (_action[0] == '@') {
        //    Debug.Log("HELOOO");
        //}

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
            responseHandling.ThrowError(ErrorType.InvalidCommand);
            return;
        }

        // ToDo
        //OutputController.instance.Output();
        //ConsoleResponseHandling.instance.ThrowResponse(ConsoleResponseHandling.ResponseType.Done);  
    }

    // ToDo
    //private void ObjectAnchor() {

    //}

    #endregion

    /// <summary>
    /// Invokes a Class Method with parameters. This where the magic happens :) 
    /// </summary>
    /// <param name="methodName">Method to Call</param>
    /// <param name="className">Method's Class Name</param>
    /// <param name="stringParams">Method's Parameters</param>
    /// <returns>Null</returns>
    public string InvokeStringMethod(string methodName, string className = null, string[] stringParams = null) {
        Type calledType = Type.GetType(className);
        String s = (String)calledType.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
                   null, null, new System.Object[] { stringParams });
        return s;
    }

    #region Input Memoization

    /// <summary>
    /// Iterates through the previous entered commands, max value of memoization can be set at class' variables.
    /// </summary>
    /// <param name="keyCode">Up or Down Arrow</param>
    private void NavigateMemo(KeyCode keyCode) {
        // If the list count is 0, we assume it is empty
        if(memoList.Count == 0) { return; }
        outputController.ClearInput();
        // Up: pointer - 1 | Down: pointer + 1
        if (keyCode == KeyCode.UpArrow) { memoPointer--; } else { memoPointer++; }
        // If pointer is less than zero, start at end
        if (memoPointer < 0) { memoPointer = memoList.Count - 1; }
        // If pointer is more than length, start at the beginning
        if (memoPointer >= memoList.Count) { memoPointer = 0; }
        // Set input to the selected value
        outputController.SetInput(memoList[memoPointer]);
        outputController.SetCaretToEnd(true);
    }

    #endregion

}
