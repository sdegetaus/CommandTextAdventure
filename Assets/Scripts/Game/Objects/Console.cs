using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour {

    static public Console instance;

    private void Awake() {
        instance = this;
    }

    // Console Actions
    static public void Clear(string[] nounAndVar = null) {
        CanvasLogicInGame.instance.ClearOutput();
    }

    static public void Change(string[] nounAndVar) {

        string _noun = nounAndVar[0];
        string _var = nounAndVar[1];

        switch(_noun) {
            case "bg":
                CanvasLogicInGame.instance.ChangeUIColor(GlobalInputParser.StringToColor(_var), true);
                break;
            case "text":
                CanvasLogicInGame.instance.ChangeUIColor(GlobalInputParser.StringToColor(_var), false);
                break;
            default:
                ErrorHandling.instance.ThrowError(ErrorHandling.ErrorType.InvalidCommand);
                return;
        }
    }

    static public void About(string[] nounAndVar = null) {

    }
}
