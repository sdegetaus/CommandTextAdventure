using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerLocation {
    Home,
    Work,
    Store,
    School
}

public enum Objects {
    Console,
    Player,
    Hidden
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

    // Statistics
    public const string stats_CurrentCoins = "data_CurrentCoins";
    public const string stats_TotalCoins = "stats_TotalCoins";
    public const string stats_TotalCommandsEntered = "stats_TotalCommandsEntered";

    #endregion

}
