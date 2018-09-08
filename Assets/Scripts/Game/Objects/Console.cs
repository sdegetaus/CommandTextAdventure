using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour {

    static public Console instance;

    private Dictionary<string, double> localVariables = new Dictionary<string, double>();

    // Temporal Code: See localVariables in inspector
    [SerializeField] private List<string> localVariableKeys = new List<string>();
    [SerializeField] private List<double> localVariableValues = new List<double>();

    private ConsoleResponseHandling consoleResponseHandling;
    private CanvasLogicInGame canvasLogicInGame;
    private DataManager dataManager;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        consoleResponseHandling = ConsoleResponseHandling.instance;
        canvasLogicInGame = CanvasLogicInGame.instance;
        dataManager = DataManager.instance;
    }

    #region Console Actions
    /// <summary>
    /// Prints out the About content.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void About(string[] nounAndVar = null) {
        // To move into another file (the text)
        instance.canvasLogicInGame.SetOutput("Command Text Adventure is a singled-person developed game by Santiago Degetau. \n" + "Version: " + Application.version);
    }

    /// <summary>
    /// Prints out the Help content.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Help(string[] nounAndVar = null) {
        instance.canvasLogicInGame.SetOutput("Console available commands: \n" +
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
        instance.canvasLogicInGame.ClearOutput();
    }

    /// <summary>
    /// Quits the Application
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Quit(string[] nounAndVar = null) {
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
                instance.canvasLogicInGame.SetUIColors(GlobalInputParser.StringToColor(_var), true);
                break;
            case "Text":
                instance.canvasLogicInGame.SetUIColors(GlobalInputParser.StringToColor(_var), false);
                break;
            case "Help":
                instance.canvasLogicInGame.SetOutput("You can change the console bg and text colors:\n console change bg / text <hex-value>  OR  console change invert  OR  console invert \n");
                break;
            case "Invert":
                Invert();
                break;
            default:
                instance.consoleResponseHandling.ThrowError(ConsoleResponseHandling.ErrorType.InvalidCommand);
                return;
        }
    }

    /// <summary>
    /// Inverts the colors of background and its text.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Invert(string[] nounAndVar = null) {
        instance.canvasLogicInGame.InvertUIColors();
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
            instance.canvasLogicInGame.SetOutput("You can set local temporal variables: console set <var> <value>");
            return;
        }

        // Limit variables to only one digit, for simplicity.
        if (_noun.Length > 1) {
            instance.consoleResponseHandling.ThrowError(ConsoleResponseHandling.ErrorType.OnlyOneDigitVariablesAllowed);
            return;
        }

        // Checking if noun is a letter, if not return with ThrowError
        if (!char.IsLetter(_noun[0])) {
            instance.consoleResponseHandling.ThrowError(ConsoleResponseHandling.ErrorType.VariablesCanOnlyBeLetters);
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
            instance.canvasLogicInGame.SetOutput("You can get values saved in the console: console get <var>  OR  console get all");
            return;
        }
        if (_noun == "All") {
            // If there are no variables, notify. If there are, loop over them.
            if (instance.localVariables.Count == 0) {
                instance.consoleResponseHandling.ThrowResponse(ConsoleResponseHandling.ResponseType.ThereAreNoVariablesSet);
                return;
            } else {
                instance.canvasLogicInGame.SetOutput("There are currently " + instance.localVariables.Count + " variables:");
                foreach (KeyValuePair<string, double> entry in instance.localVariables) {
                    instance.canvasLogicInGame.SetOutput(entry.Key + " -> " + entry.Value);
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
            instance.canvasLogicInGame.SetOutput("You can calculate values: console calc <math-expression>; local variables are allowed");
            return;
        }
        string result = instance.LookAndApplyVariableValue(_noun).ToString();
        instance.canvasLogicInGame.SetOutput(result);
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
                    instance.consoleResponseHandling.ThrowError(ConsoleResponseHandling.ErrorType.LocalVariableNotFound, c.ToString());
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
