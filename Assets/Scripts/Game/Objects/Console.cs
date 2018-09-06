using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour {

    static public Console instance;

    private void Awake() {
        instance = this;
    }

    // Console Actions
    static public void About(string[] nounAndVar = null) {
        // To move into another file (the text)
        CanvasLogicInGame.instance.SetOutput("Command Text Adventure is a singled-person developed game by Santiago Degetau. \n" + "Version: " + Application.version);
    }

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

    // Console Clear: clears the command output.
    static public void Clear(string[] nounAndVar = null) {
        CanvasLogicInGame.instance.ClearOutput();
    }

    // Console Change _noun _color
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
            default:
                ConsoleResponseHandling.instance.ThrowError(ConsoleResponseHandling.ErrorType.InvalidCommand);
                return;
        }
    }

}
