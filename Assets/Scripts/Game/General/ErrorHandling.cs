using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorHandling : MonoBehaviour {

    public enum ErrorType {
        ValueNotFormattedCorrectly,
        InvalidCommand
    }

    static public ErrorHandling instance;

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
            default:
                errorMessage += "nothing";
                break;
        }
        CanvasLogicInGame.instance.SetOutput(errorMessage);
    }
}
