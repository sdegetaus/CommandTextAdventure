using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour {

    static public Console instance;

    private Dictionary<string, double> localVariables = new Dictionary<string, double>();

    // Temporal Code: See localVariables in inspector
    [SerializeField] private List<string> localVariableKeys = new List<string>();
    [SerializeField] private List<double> localVariableValues = new List<double>();

    private ResponseHandling responseHandling;
    private OutputController outputController;
    private DataManager dataManager;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        responseHandling = ResponseHandling.instance;
        outputController = OutputController.instance;
        dataManager = DataManager.instance;
    }

    #region Console Actions
    /// <summary>
    /// Prints out the About content.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void About(string[] nounAndVar = null) {
        instance.outputController.SetOutput("Command TextAdventure is a simple text-adventure game developed by Santiago Degetau just for fun: v" + Application.version);
    }

    /// <summary>
    /// Prints out the Help content.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Help(string[] nounAndVar = null) {
        instance.outputController.SetOutput("Console available commands: \n" +
                                             "\n" +
                                             "-> about" + "\n" +
                                             "-> clear" + "\n" +
                                             "-> quit" + "\n" +
                                             "-> change <noun> <var>" + "\n" +
                                             "-> get <noun> <var>" + "\n" +
                                             "-> set <noun> <var>" + "\n" +
                                             "-> calc <expression>" + "\n" + "\n" +
                                             "Type \"help\" after a command to get help" + "\n");
    }

    /// <summary>
    /// Clears the console output text.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Clear(string[] nounAndVar = null) {
        instance.outputController.ClearOutput();
    }

    /// <summary>
    /// Quits the Application
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Exit(string[] nounAndVar = null) {
        Application.Quit();
    }

    //static public void Print(string[] nounAndVar) {
    //    CanvasLogicInGame.instance.SetOutput(string.Join("", nounAndVar));
    //}

    /// <summary>
    /// Changes background or text color of console.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Change(string[] nounAndVar) {
        string _noun = nounAndVar[0]; // Bg or Text, Invert || Help
        string _var = nounAndVar[1]; // Hexadecimal Value

        switch(_noun) {
            case "Bg":
                instance.outputController.SetUIColors(GlobalInputParser.StringToColor(_var), true);
                return;
            case "Text":
                instance.outputController.SetUIColors(GlobalInputParser.StringToColor(_var), false);
                return;
            case "Help":
                instance.outputController.SetOutput("You can change the console bg and text colors:\n console change bg / text <hex-value>  OR  console change invert  OR  console invert \n");
                return;
            case "Invert":
                Invert();
                return;
            case "Swap":
                Swap();
                return;
            default:
                instance.responseHandling.ThrowError(ErrorType.InvalidCommand);
                return;
        }
    }

    /// <summary>
    /// Inverts the colors of background and its text.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Invert(string[] nounAndVar = null) {
        instance.outputController.InvertUIColors();
    }

    /// <summary>
    /// Swaps the colors of background and text.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Swap (string[] nounAndVar = null) {
        instance.outputController.SwapUIColors();
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
            instance.outputController.SetOutput("You can set local temporal variables: console set <var> <value>");
            return;
        }

        // Limit variables to only one digit, for simplicity.
        if (_noun.Length > 1) {
            instance.responseHandling.ThrowError(ErrorType.OnlyOneDigitVariablesAllowed);
            return;
        }

        // Checking if noun is a letter, if not return with ThrowError
        if (!char.IsLetter(_noun[0])) {
            instance.responseHandling.ThrowError(ErrorType.VariablesCanOnlyBeLetters);
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
            instance.outputController.SetOutput("You can get values saved in the console: console get <var>  OR  console get all");
            return;
        }
        if (_noun == "All") {
            // If there are no variables, notify. If there are, loop over them.
            if (instance.localVariables.Count == 0) {
                instance.responseHandling.ThrowResponse(ResponseType.ThereAreNoVariablesSet);
                return;
            } else {
                instance.outputController.SetOutput("There are currently " + instance.localVariables.Count + " variables:");
                foreach (KeyValuePair<string, double> entry in instance.localVariables) {
                    instance.outputController.SetOutput(entry.Key + " -> " + entry.Value);
                }
            }
            return;
        }

        // If _noun (key) exists, show value. If not, throw error.
        if (instance.localVariables.ContainsKey(_noun)) {
            instance.outputController.SetOutput(_noun + " = " + instance.localVariables[_noun]);
        } else {
            instance.responseHandling.ThrowError(ErrorType.LocalVariableNotFound, _noun);
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
            instance.outputController.SetOutput("You can calculate values: console calc <math-expression>; local variables are allowed");
            return;
        }
        string result = instance.LookAndApplyVariableValue(_noun).ToString();
        instance.outputController.SetOutput(_noun + " = " + result);
    } 
    
    /// <summary>
    /// Calculates mathematical expression. Variables are allowed.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Reset(string[] nounAndVar = null) {
        PlayerPrefs.DeleteAll();
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
                    instance.responseHandling.ThrowError(ErrorType.LocalVariableNotFound, c.ToString());
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
