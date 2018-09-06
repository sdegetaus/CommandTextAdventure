using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleResponseHandling : MonoBehaviour {

    public enum ErrorType {
        ValueNotFormattedCorrectly,
        InvalidCommand,
        LocalVariableNotFound,
        OnlyOneDigitVariablesAllowed
    }

    public enum ResponseType {
        Done
    }

    static public ConsoleResponseHandling instance;

    private void Awake() {
        instance = this;
    }

    public void ThrowError(ErrorType errorType, string blamedValue = null) {
        string errorMessage = "ERROR: ";
        switch (errorType) {
            case ErrorType.ValueNotFormattedCorrectly:
                errorMessage += "value not formatted correctly";
                break;
            case ErrorType.InvalidCommand:
                errorMessage += "invalid command";
                break;
            case ErrorType.LocalVariableNotFound:
                errorMessage += "local variable not found";
                break;
            case ErrorType.OnlyOneDigitVariablesAllowed:
                errorMessage += "only one digit variables allowed";
                break;
            default:
                errorMessage += "nothing";
                break;
        }
        CanvasLogicInGame.instance.SetOutput(errorMessage);
    }

    public void ThrowResponse(ResponseType responseType, string blamedValue = null) {
        string responseMessage = "";
        switch (responseType) {
            case ResponseType.Done:
                responseMessage += "Done";
                break;
        }
        CanvasLogicInGame.instance.SetOutput(responseMessage);
    }
}
