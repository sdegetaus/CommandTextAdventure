﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleResponseHandling : MonoBehaviour {

    public enum ErrorType {
        ValueNotFormattedCorrectly,
        InvalidCommand,
        LocalVariableNotFound,
        OnlyOneDigitVariablesAllowed,
        VariablesCanOnlyBeLetters
    }

    public enum ResponseType {
        Done,
        ThereAreNoVariablesSet
    }

    public enum PlayerRestrictions {
        PlayerNotAtCorrectLocation,
        PlayerAlreadyAtLocation
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
                errorMessage += "local variable " + blamedValue + " not found";
                break;
            case ErrorType.OnlyOneDigitVariablesAllowed:
                errorMessage += "only one digit variables allowed";
                break;
            case ErrorType.VariablesCanOnlyBeLetters:
                errorMessage += "variables can only be letters.";
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
            case ResponseType.ThereAreNoVariablesSet:
                responseMessage += "Currently there are no variables set";
                break;
        }
        CanvasLogicInGame.instance.SetOutput(responseMessage);
    }

    public void ThrowRestriction(PlayerRestrictions playerRestrictions, string blamedValue = null) {
        string restrictionMessage = "NOT ALLOWED: ";
        switch (playerRestrictions) {
            case PlayerRestrictions.PlayerNotAtCorrectLocation:
                restrictionMessage += "The player is not at " + blamedValue;
                break;
            case PlayerRestrictions.PlayerAlreadyAtLocation:
                restrictionMessage += "The player is already at " + blamedValue;
                break;
        }
        CanvasLogicInGame.instance.SetOutput(restrictionMessage);
    }
}
