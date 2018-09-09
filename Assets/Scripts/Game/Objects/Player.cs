using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    static public Player instance;

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

    #region Player Actions

    /// <summary>
    /// Player Stats - command displayer method
    /// </summary>
    /// <param name="nounAndVar"></param>   
    static public void Help(string[] nounAndVar = null) {
        instance.outputController.SetOutput("Player available commands: \n" +
                                             "\n" +
                                             "-> stats" + "\n" +
                                             "-> get <noun>" + "\n" +
                                             "-> set <noun>" + "\n" +
                                             "-> goto <location>" + "\n" +
                                             "-> work" + "\n" +
                                             "-> buy <noun> " + "\n \n" +
                                             "Type \"help\" after a command to get help" + "\n");
    }

    /// <summary>
    /// Player Stats - command displayer method
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Stats(string[] nounAndVar = null) {
        //ToDo
        instance.outputController.SetOutput("Here your stats will be displayed :) \n" +
                                             "IDEAS: \n" +
                                             "Total Valid Commands Entered: " + instance.dataManager.GetTotalCommandsEntered() + "\n" +
                                             "Current Coins: " + instance.dataManager.GetCurrentCoins() + "\n" +
                                             "Total Coins: " + instance.dataManager.GetTotalCoins());
    }

    /// <summary>
    /// Player Goto: sets the Player to another virtual location
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Goto(string[] nounAndVar = null) {
        string _noun = nounAndVar[0]; // Place
        string _var = nounAndVar[1]; // ... todo, perhas adverbs of time (how quickly the player can get there).
        switch(_noun) {
            case "Home":
                instance.dataManager.SetPlayerCurrentLocation(PlayerLocation.Home);
                break;
            case "Work":
                instance.dataManager.SetPlayerCurrentLocation(PlayerLocation.Work);
                break;
            case "Store":
                instance.dataManager.SetPlayerCurrentLocation(PlayerLocation.Store);
                break;
            case "Help":
                instance.outputController.SetOutput("You can go to other places in the world: player goto <location>");
                instance.PrintAvailableLocations();
                return;
            default:
                instance.responseHandling.ThrowError(ErrorType.InvalidCommand);
                return;
        }

        if (instance.dataManager.previousPlayerLocation == instance.dataManager.currentPlayerLocation) {
            instance.responseHandling.ThrowRestriction(PlayerRestrictions.PlayerAlreadyAtLocation, _noun);
            return;
        } else {
            instance.StartCoroutine(instance.GotoTime(0.0f));
        }
    }

    /// <summary>
    /// Player's current location.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Location(string[] nounAndVar = null) {
        if (nounAndVar[0] == "Help") {
            instance.outputController.SetOutput("To navigate the world use: player goto <location> \n ");
            instance.PrintAvailableLocations();
            return;
        }
        instance.outputController.SetOutput("Player currently at: " + instance.dataManager.GetPlayerCurrentLocation().ToString());
    }

    /// <summary>
    /// Buy method for Player.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Buy(string[] nounAndVar = null) {
        if (instance.dataManager.currentPlayerLocation != PlayerLocation.Store) {
            instance.responseHandling.ThrowRestriction(PlayerRestrictions.PlayerNotAtCorrectLocation, PlayerLocation.Store.ToString());
            return;
        } else {
            instance.outputController.SetOutput("Currently the store is closed.");
        }
        // ToDo
    }

    /// <summary>
    /// Win money this way: data like money is managed at DataManager for data persistence.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Work(string[] nounAndVar = null) {
        // If noun is Help, show info
        if (nounAndVar[0] == "Help") {
            instance.outputController.SetOutput("Player must be at Work in order to win money. Type: player goto <location>");
            return;
        }
        // If player is not at work, throw restriction. Else, work with timer.
        if (instance.dataManager.currentPlayerLocation != PlayerLocation.Work) {
            instance.responseHandling.ThrowRestriction(PlayerRestrictions.PlayerNotAtCorrectLocation, PlayerLocation.Work.ToString());
            return;
        } else {
            instance.StartCoroutine(instance.WorkTime(0.0f));
        }
    }

    /// <summary>
    /// Method for getting Player's information, such as: Money, Name, Location
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Get(string[] nounAndVar) {
        string _noun = nounAndVar[0];
        string _var = nounAndVar[1];

        switch(_noun) {
            case "Money":
            case "Gold":
            case "Coins":
                instance.outputController.SetOutput("Total Coins: " + instance.dataManager.GetCurrentCoins().ToString());
                break;
            case "Name":
                instance.outputController.SetOutput(PlayerPrefs.GetString(_Cn.data_PlayerName, "Undefined"));
                break;
            case "Location":
                Location();
                break;
            case "Help":
                instance.outputController.SetOutput("Get information about the player: player get <noun>");
                break;
            default:
                instance.responseHandling.ThrowError(ErrorType.InvalidCommand);
                break;
        }
    }

    /// <summary>
    /// Set Player's Information --> perhaps a little superfluous.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Set(string[] nounAndVar) {
        string _noun = nounAndVar[0];
        string _var = nounAndVar[1];

        switch (_noun) {
            case "Name":
                PlayerPrefs.SetString(_Cn.data_PlayerName, _var);
                instance.outputController.SetOutput("Player's Name Changed to: " + _var);
                break;
            case "Help":
                instance.outputController.SetOutput("Set information about the player: player set <noun>");
                break;
            default:
                instance.responseHandling.ThrowError(ErrorType.InvalidCommand);
                break;
        }
    }

    /// <summary>
    /// Prints the availabl money.
    /// </summary>
    /// <param name="nounAndVar"></param>
    static public void Money(string[] nounAndVar = null) {
        instance.outputController.SetOutput("Total Current Coins: " + instance.dataManager.GetCurrentCoins().ToString());
    }

    #endregion

    #region Player Helper Functions

    /// <summary>
    /// "Punishment" for working time.
    /// </summary>
    /// <param name="seconds">Seconds to wait.</param>
    /// <returns>WaitForSeconds</returns>
    private IEnumerator WorkTime(float seconds) {
        string waitDots = "...";
        instance.outputController.SetInputActive(false);
        instance.outputController.SetOutput("working ", false);
        foreach (char c in waitDots) {
            instance.outputController.SetOutput(c.ToString() + " ", false);
            yield return new WaitForSeconds(seconds / waitDots.Length);
        }
        instance.outputController.SetOutput("\n", false);
        int valueGained = 5; // To be Set dynamically
        instance.dataManager.SetCurrentCoins(valueGained); // <-- Available to spend
        instance.dataManager.SetTotalCoins(valueGained); // <-- Total Gained throughout the game
        instance.outputController.SetOutput("Coins gained: " + valueGained);
        instance.outputController.SetInputActive(true);
    }

    /// <summary>
    /// "Punishment" for getting from one place to another
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private IEnumerator GotoTime(float seconds) {
        string waitDots = "...";
        instance.outputController.SetInputActive(false);
        instance.outputController.SetOutput("travelling ", false);
        foreach (char c in waitDots) {
            instance.outputController.SetOutput(c.ToString() + " ", false);
            yield return new WaitForSeconds(seconds / waitDots.Length);
        }
        instance.outputController.SetOutput("\n", false);
        instance.outputController.SetOutput("Player has arrived to: " + instance.dataManager.currentPlayerLocation.ToString());
        instance.outputController.SetInputActive(true);
    }

    /// <summary>
    /// Common Available Locations Printer
    /// </summary>
    private void PrintAvailableLocations() {
        instance.outputController.SetOutput("Available locations: \n", false);
        foreach (PlayerLocation location in Enum.GetValues(typeof(PlayerLocation))) {
            instance.outputController.SetOutput("- " + location.ToString() + "\n", false);
        }
    }
    
    /// <summary>
    /// Common Available Locations Printer
    /// </summary>
    private void PrintAvailablePurchaseables() {
        instance.outputController.SetOutput("Available items for buying: \n", false);
        //foreach (PlayerLocation location in Enum.GetValues(typeof(PlayerLocation))) {
        //    instance.outputController.SetOutput("- " + location.ToString() + "\n", false);
        //}
    }

    #endregion
}
