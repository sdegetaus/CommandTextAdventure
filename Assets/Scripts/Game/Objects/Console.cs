using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        string _noun = nounAndVar[0]; // Bg or Text, Invert || Help
        string _var = nounAndVar[1]; // Hexadecimal Value

        switch(_noun) {
            case "Bg":
                CanvasLogicInGame.instance.SetUIColors(GlobalInputParser.StringToColor(_var), true);
                break;
            case "Text":
                CanvasLogicInGame.instance.SetUIColors(GlobalInputParser.StringToColor(_var), false);
                break;
            case "Help":
                CanvasLogicInGame.instance.SetOutput("To change the colors of the console background or text, you must follow this structure: console change bg/text <hexadecimal-color> \n " +
                                                     "Furthermore, if you wish to invert the colors of both parameters: console invert");
                break;
            default:
                ConsoleResponseHandling.instance.ThrowError(ConsoleResponseHandling.ErrorType.InvalidCommand);
                return;
        }
    }

    /// <summary>
    /// Inverts the colors of background and its text.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Invert(string[] nounAndVar = null) {
        CanvasLogicInGame.instance.InvertUIColors();
    }

    /// <summary>
    /// Sets a variable.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Set(string[] nounAndVar) {
        string _noun = nounAndVar[0]; // variable name (key)
        string _var = nounAndVar[1]; // variable value (value)

        // Help Suffix Text
        if (_noun == "Help") {
            Debug.Log("Help!");
            return;
        }

        // Limit variables to only one digit, for simplicity.
        if (_noun.Length > 1) {
            ConsoleResponseHandling.instance.ThrowError(ConsoleResponseHandling.ErrorType.OnlyOneDigitVariablesAllowed);
            return;
        }

        // Checking if noun is a letter, if not return with ThrowError
        if (!char.IsLetter(_noun[0])) {
            ConsoleResponseHandling.instance.ThrowError(ConsoleResponseHandling.ErrorType.VariablesCanOnlyBeLetters);
            return;
        }

        double _varDouble = instance.LookAndApplyVariableValue(_var);

        // If dictionary doesn't contains the key: add it. Else: replace it.
        if (!instance.localVariables.ContainsKey(_noun)) {
            instance.localVariables.Add(_noun, _varDouble);
        } else {
            instance.localVariables[_noun] = _varDouble;
        }

        // See Dictionary in Inspector (doesn't account for variable replacements, it is just for simple debugging)
        instance.localVariableKeys.Add(_noun);
        instance.localVariableValues.Add(_varDouble);
    }

    /// <summary>
    /// Gets an established variable's value, or all
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Get(string[] nounAndVar) {
        string _noun = nounAndVar[0]; // variable name (key)
        // Help Suffix Text
        if (_noun == "Help") {
            Debug.Log("Help!");
            return;
        }
        if (_noun == "All") {
            // If there are no variables, notify. If there are, loop over them.
            if (instance.localVariables.Count == 0) {
                ConsoleResponseHandling.instance.ThrowResponse(ConsoleResponseHandling.ResponseType.ThereAreNoVariablesSet);
                return;
            } else {
                CanvasLogicInGame.instance.SetOutput("There are currently " + instance.localVariables.Count + " variables:");
                foreach (KeyValuePair<string, double> entry in instance.localVariables) {
                    CanvasLogicInGame.instance.SetOutput(entry.Key + " -> " + entry.Value);
                }
            }
            return;
        }

        // If _noun (key) exists, show value. If not, throw error.
        if (instance.localVariables.ContainsKey(_noun)) {
            CanvasLogicInGame.instance.SetOutput(_noun + " = " + instance.localVariables[_noun]);
        } else {
            ConsoleResponseHandling.instance.ThrowError(ConsoleResponseHandling.ErrorType.LocalVariableNotFound, _noun);
            return;
        }
    }

    /// <summary>
    /// Calculates mathematical expression. Variables are allowed.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Calc(string[] nounAndVar) {
        string _noun = nounAndVar[0]; // Mathematical Expression
        // Help Suffix Text
        if (_noun == "Help") {
            Debug.Log("Help!");
            return;
        }
        string result = instance.LookAndApplyVariableValue(_noun).ToString();
        CanvasLogicInGame.instance.SetOutput(result);
    }

    #endregion

    #region Console Helper Functions
    /// <summary>
    /// Iterates through every char of _noun (expression). If a letter is found, it searches if it is a local variable, throws error if not. Finally, it does the calculation.
    /// </summary>
    /// <param name="s"></param>
    /// <returns>Math Expression Result</returns>
    private double LookAndApplyVariableValue(string s) {
        List<string> expressionParsed = new List<string>();
        s = s.ToUpper(); // Handle this in another layer of the parsing
        foreach (char c in s) {
            if (char.IsLetter(c)) {
                if (instance.localVariables.ContainsKey(c.ToString())) {
                    expressionParsed.Add(instance.localVariables[c.ToString()].ToString());
                } else {
                    ConsoleResponseHandling.instance.ThrowError(ConsoleResponseHandling.ErrorType.LocalVariableNotFound, c.ToString());
                    expressionParsed.Clear();
                    return 0;
                }
            } else {
                expressionParsed.Add(c.ToString());
            }
        }
        string joinedExpression = string.Join("", expressionParsed.ToArray());
        expressionParsed.Clear();
        return GlobalInputParser.EvaluateMathExpression(joinedExpression);
    }

    #endregion

}
