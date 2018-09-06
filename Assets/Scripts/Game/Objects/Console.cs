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
        // To move into another file
        CanvasLogicInGame.instance.SetOutput("Command Text Adventure is a singled-person developed game by Santiago Degetau. \n" + "Version: " + Application.version);
    }

    static public void Clear(string[] nounAndVar = null) {
        CanvasLogicInGame.instance.ClearOutput();
    }

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
