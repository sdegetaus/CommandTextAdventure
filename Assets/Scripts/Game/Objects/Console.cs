using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Console : MonoBehaviour {

    static public Console instance;

    private Dictionary<string, double> localVariables = new Dictionary<string, double>();

    // See dictionary in inspector
    [SerializeField] private List<string> localVariableKeys = new List<string>();
    [SerializeField] private List<double> localVariableValues = new List<double>();

    private void Awake() {
        instance = this;
    }

    #region Console Actions
    /// <summary>
    /// Prints out the About content.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void About(string[] nounAndVar = null) {
        // To move into another file (the text)
        CanvasLogicInGame.instance.SetOutput("Command Text Adventure is a singled-person developed game by Santiago Degetau. \n" + "Version: " + Application.version);
    }

    /// <summary>
    /// Prints out the Help content.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Help(string[] nounAndVar = null) {
        CanvasLogicInGame.instance.SetOutput(
            Application.productName + " is a game about exploration. \n" +
            "Just like your regular Windows Command Prompt... TODO. \n" +
            "Here are a list of commands you can use to get you started: \n" +
            "about - get information about this game. \n" +
            "help - get help. \n" +
            "clear - clear the command output. \n" +
            "change - change the color of bg or text"
            );
    }

    /// <summary>
    /// Clears the console output text.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Clear(string[] nounAndVar = null) {
        CanvasLogicInGame.instance.ClearOutput();
    }

    /// <summary>
    /// Changes background or text color of console.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Change(string[] nounAndVar) {
        string _noun = nounAndVar[0];
        string _var = nounAndVar[1];
        switch(_noun) {
            case "Bg":
                CanvasLogicInGame.instance.ChangeUIColor(GlobalInputParser.StringToColor(_var), true);
                break;
            case "Text":
                CanvasLogicInGame.instance.ChangeUIColor(GlobalInputParser.StringToColor(_var), false);
                break;
            case "Help":
                CanvasLogicInGame.instance.SetOutput("To change the colors of the console background or text, you must follow this structure: console change bg/text <hexadecimal-color>");
                break;
            default:
                ConsoleResponseHandling.instance.ThrowError(ConsoleResponseHandling.ErrorType.InvalidCommand);
                return;
        }
    }

    /// <summary>
    /// Sets a variable.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Set(string[] nounAndVar) {
        string _noun = nounAndVar[0]; // variable name (key)
        string _var = nounAndVar[1]; // variable value (value)
        double _varDouble = GlobalInputParser.EvaluateMathExpression(_var);
        
        // Limit variables to only one digit, for simplicity.
        if(_noun.Length > 1) {
            ConsoleResponseHandling.instance.ThrowError(ConsoleResponseHandling.ErrorType.OnlyOneDigitVariablesAllowed);
            return;
        }

        // If dictionary doesn't contains the key: add it. Else: replace it.
        if (!instance.localVariables.ContainsKey(_noun)) {
            instance.localVariables.Add(_noun, _varDouble);
        } else {
            instance.localVariables[_noun] = _varDouble;
        }

        // See Dictionary in Inspector (doesn't account for variable replacements, it is jsut for simple debugging)
        instance.localVariableKeys.Add(_noun);
        instance.localVariableValues.Add(_varDouble);
    }

    /// <summary>
    /// Gets an established variable's value
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Get(string[] nounAndVar) {
        string _noun = nounAndVar[0]; // variable name (key)

        // If _noun (key) exists, show value. If not, throw error.
        if (instance.localVariables.ContainsKey(_noun)) {
            CanvasLogicInGame.instance.SetOutput(_noun + " = " + instance.localVariables[_noun]);
        } else {
            ConsoleResponseHandling.instance.ThrowError(ConsoleResponseHandling.ErrorType.LocalVariableNotFound);
        }
    }

    /// <summary>
    /// Calculates mathematical expression. Variables are allowed.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Calc(string[] nounAndVar) {
        string _noun = nounAndVar[0]; // Mathematical Expression
        // Iterates through every char of _noun (expression). If a letter is found, it searches if it is a local variable, throws error if not. Finally, it does the calculation.
        List<string> expressionParsed = new List<string>();
        foreach (char c in _noun) {
            if (char.IsLetter(c)) {
                if (instance.localVariables.ContainsKey(c.ToString())) {
                    expressionParsed.Add(instance.localVariables[c.ToString()].ToString());
                } else {
                    ConsoleResponseHandling.instance.ThrowError(ConsoleResponseHandling.ErrorType.LocalVariableNotFound);
                    return;
                }
            } else {
                expressionParsed.Add(c.ToString());
            }
        }
        string joinedExpression = string.Join("", expressionParsed.ToArray());
        string result = GlobalInputParser.EvaluateMathExpression(joinedExpression).ToString();
        CanvasLogicInGame.instance.SetOutput(result);
    }
    #endregion

}
