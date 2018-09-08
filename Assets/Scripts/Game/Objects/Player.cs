using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    static public Player instance;
    private PlayerLocation currentPlayerLocation;
    private PlayerLocation previousPlayerLocation;

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

        currentPlayerLocation = PlayerLocation.Home;
    }

    #region Player Actions
    static public void Help(string[] nounAndVar = null) {
        instance.canvasLogicInGame.SetOutput("Player available commands: \n" +
                                             "\n" +
                                             "-> stats" + "\n" +
                                             "-> get <noun>" + "\n" +
                                             "-> set <noun>" + "\n" +
                                             "-> goto <location>" + "\n" +
                                             "-> work" + "\n" +
                                             "-> buy <noun> " + "\n \n" +
                                             "Type \"help\" after a command to get help" + "\n");
    }

    static public void Stats(string[] nounAndVar = null) {
        //ToDo
        instance.canvasLogicInGame.SetOutput("Here your stats will be displayed :) \n" +
                                             "IDEAS: \n" +
                                             "Commands Made, ");
    }

    static public void Goto(string[] nounAndVar = null) {
        string _noun = nounAndVar[0]; // Place
        string _var = nounAndVar[1]; // ... todo, perhas adverbs of time (how quickly the player can get there).
        bool isHelp = false;
        switch(_noun) {
            case "Home":
                instance.previousPlayerLocation = instance.currentPlayerLocation;
                instance.currentPlayerLocation = PlayerLocation.Home;
                break;
            case "Work":
                instance.previousPlayerLocation = instance.currentPlayerLocation;
                instance.currentPlayerLocation = PlayerLocation.Work;
                break;
            case "Store":
                instance.previousPlayerLocation = instance.currentPlayerLocation;
                instance.currentPlayerLocation = PlayerLocation.Store;
                break;
            case "Help":
                instance.canvasLogicInGame.SetOutput("You can go to other places in the world: player goto <location>");
                instance.PrintAvailableLocations();
                isHelp = true;
                break;
            default:
                instance.consoleResponseHandling.ThrowError(ConsoleResponseHandling.ErrorType.InvalidCommand);
                return;
        }

        // If player types help, do not run the las bit of code
        if(isHelp) {
            return;
        }

        if (instance.previousPlayerLocation == instance.currentPlayerLocation) {
            instance.consoleResponseHandling.ThrowRestriction(ConsoleResponseHandling.PlayerRestrictions.PlayerAlreadyAtLocation, _noun);
            return;
        } else {
            instance.StartCoroutine(instance.GotoTime(5.0f));
        }
    }

    static public void Location(string[] nounAndVar = null) {
        if (nounAndVar[0] == "Help") {
            instance.canvasLogicInGame.SetOutput("To navigate the world use: player goto <location> \n ");
            instance.PrintAvailableLocations();
            return;
        }
        instance.canvasLogicInGame.SetOutput("Player currently in: " + instance.currentPlayerLocation.ToString());
    }

    static public void Buy(string[] nounAndVar = null) {
        if (instance.currentPlayerLocation != PlayerLocation.Store) {
            instance.consoleResponseHandling.ThrowRestriction(ConsoleResponseHandling.PlayerRestrictions.PlayerNotAtCorrectLocation, PlayerLocation.Store.ToString());
            return;
        } else {
            instance.canvasLogicInGame.SetOutput("Currently the store is closed.");
        }
        // ToDo
    }

    static public void Work(string[] nounAndVar = null) {
        // If noun is Help, show info
        if (nounAndVar[0] == "Help") {
            instance.canvasLogicInGame.SetOutput("Player must be at Work in order to win money. Type: player goto <location>");
            return;
        }
        // If player is not at work, throw restriction
        if (instance.currentPlayerLocation != PlayerLocation.Work) {
            instance.consoleResponseHandling.ThrowRestriction(ConsoleResponseHandling.PlayerRestrictions.PlayerNotAtCorrectLocation, PlayerLocation.Work.ToString());
            return;
        } else {
            instance.StartCoroutine(instance.WorkTime(5.0f));
        }
    }

    static public void Get(string[] nounAndVar) {
        string _noun = nounAndVar[0];
        string _var = nounAndVar[1];

        switch(_noun) {
            case "Money":
            case "Gold":
            case "Coins":
                instance.canvasLogicInGame.SetOutput("Total Coins: " + instance.dataManager.GetCoins().ToString());
                break;
            case "Name":
                instance.canvasLogicInGame.SetOutput(PlayerPrefs.GetString(_Cn.data_PlayerName, "Undefined"));
                break;
            case "Location":
                Location();
                break;
            case "Help":
                instance.canvasLogicInGame.SetOutput("Get information about the player: player get <noun>");
                break;
            default:
                instance.consoleResponseHandling.ThrowError(ConsoleResponseHandling.ErrorType.InvalidCommand);
                break;
        }
    }

    static public void Set(string[] nounAndVar) {
        string _noun = nounAndVar[0];
        string _var = nounAndVar[1];

        switch (_noun) {
            case "Name":
                PlayerPrefs.SetString(_Cn.data_PlayerName, _var);
                instance.canvasLogicInGame.SetOutput("Player's Name Changed to: " + _var);
                break;
            case "Help":
                instance.canvasLogicInGame.SetOutput("Set information about the player: player set <noun>");
                break;
            default:
                instance.consoleResponseHandling.ThrowError(ConsoleResponseHandling.ErrorType.InvalidCommand);
                break;
        }
    }
    #endregion

    #region Player Helper Functions

    private IEnumerator WorkTime(float seconds) {
        string waitDots = "...";
        instance.canvasLogicInGame.SetInputActive(false);
        instance.canvasLogicInGame.SetOutput("working ", false);
        foreach (char c in waitDots) {
            instance.canvasLogicInGame.SetOutput(c.ToString() + " ", false);
            yield return new WaitForSeconds(seconds / waitDots.Length);
        }
        instance.canvasLogicInGame.SetOutput("\n", false);
        int valueGained = 5; // Set dynamically
        instance.dataManager.SetCoins(valueGained);
        instance.canvasLogicInGame.SetOutput("Coins gained: " + valueGained);
        instance.canvasLogicInGame.SetInputActive(true);
    }

    private IEnumerator GotoTime(float seconds) {
        string waitDots = "...";
        instance.canvasLogicInGame.SetInputActive(false);
        instance.canvasLogicInGame.SetOutput("travelling ", false);
        foreach (char c in waitDots)
        {
            instance.canvasLogicInGame.SetOutput(c.ToString() + " ", false);
            yield return new WaitForSeconds(seconds / waitDots.Length);
        }
        instance.canvasLogicInGame.SetOutput("\n", false);
        instance.canvasLogicInGame.SetOutput("Player has arrived to: " + instance.currentPlayerLocation.ToString());
        instance.canvasLogicInGame.SetInputActive(true);
    }

    private void PrintAvailableLocations() {
        instance.canvasLogicInGame.SetOutput("Available locations: \n", false);
        foreach (PlayerLocation location in Enum.GetValues(typeof(PlayerLocation))) {
            instance.canvasLogicInGame.SetOutput("- " + location.ToString() + "\n", false);
        }
    }
    #endregion

}
