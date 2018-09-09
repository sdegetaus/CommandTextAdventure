using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerLocation {
    Home,
    Work,
    Store
}

public enum Objects {
    Console,
    Player
}

#region ResponseHandling

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

#endregion

public class _Cn  {

    #region Player Preferences
    public const string data_PlayerName = "data_PlayerName";
    public const string data_PlayerLocation = "data_PlayerLocation";
    public const string data_TotalCoins = "data_TotalCoins";
    #endregion

}
