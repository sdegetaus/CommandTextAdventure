using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public enum PlayerLocation {
        Home, //temp
        Work,
        Store
    }

    static public Player instance;
    private PlayerLocation currentPlayerLocation;
    private PlayerLocation previousPlayerLocation;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        currentPlayerLocation = PlayerLocation.Home;
    }

    #region Player Actions

    static public void Stats(string[] nounAndVar = null) {
        //ToDo
        CanvasLogicInGame.instance.SetOutput("Here your stats will be displayed :) \n" +
                                             "IDEAS: \n" +
                                             "Commands Made, ");
    }

    static public void Goto(string[] nounAndVar = null) {
        string _noun = nounAndVar[0]; // Place
        string _var = nounAndVar[1]; // ... todo, perhas adverbs of time (how quickly the player can get there).

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
            default:
                ConsoleResponseHandling.instance.ThrowError(ConsoleResponseHandling.ErrorType.InvalidCommand);
                return;
        }

        if (instance.previousPlayerLocation == instance.currentPlayerLocation) {
            ConsoleResponseHandling.instance.ThrowRestriction(ConsoleResponseHandling.PlayerRestrictions.PlayerAlreadyAtLocation, _noun);
            return;
        } else {
            instance.StartCoroutine(instance.GotoTime(5.0f));
        }
    }

    static public void Location(string[] nounAndVar = null) {
        CanvasLogicInGame.instance.SetOutput("Player currently in: " + instance.currentPlayerLocation.ToString());
    }

    static public void Buy(string[] nounAndVar = null) {
        if (instance.currentPlayerLocation != PlayerLocation.Store) {
            ConsoleResponseHandling.instance.ThrowRestriction(ConsoleResponseHandling.PlayerRestrictions.PlayerNotAtCorrectLocation, PlayerLocation.Store.ToString());
            return;
        } else {
            CanvasLogicInGame.instance.SetOutput("Currently the store is closed.");
        }
        // ToDo
    }

    static public void Work(string[] nounAndVar = null) {
        if (instance.currentPlayerLocation != PlayerLocation.Work) {
            ConsoleResponseHandling.instance.ThrowRestriction(ConsoleResponseHandling.PlayerRestrictions.PlayerNotAtCorrectLocation, PlayerLocation.Work.ToString());
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
            case "Coins":
                CanvasLogicInGame.instance.SetOutput("Total Coins: " + DataManager.instance.GetCoins().ToString());
                break;
            case "Name":
                CanvasLogicInGame.instance.SetOutput(PlayerPrefs.GetString(_Cn.data_PlayerName, "Undefined"));
                break;
            case "Locations":
                CanvasLogicInGame.instance.SetOutput("Available locations: \n", false);
                foreach (PlayerLocation location in Enum.GetValues(typeof(PlayerLocation))) {
                    CanvasLogicInGame.instance.SetOutput("- " + location.ToString() + "\n", false);
                }
                break;
        }
    }

    static public void Set(string[] nounAndVar) {
        string _noun = nounAndVar[0];
        string _var = nounAndVar[1];

        switch (_noun) {
            case "Name":
                PlayerPrefs.SetString(_Cn.data_PlayerName, _var);
                CanvasLogicInGame.instance.SetOutput("Player's Name Changed to: " + _var);
                break;
        }
    }
    #endregion

    #region Player Helper Functions

    private IEnumerator WorkTime(float seconds) {
        string waitDots = "...";
        CanvasLogicInGame.instance.SetOutput("working ", false);
        foreach (char c in waitDots) {
            CanvasLogicInGame.instance.SetOutput(c.ToString() + " ", false);
            yield return new WaitForSeconds(seconds / waitDots.Length);
        }
        CanvasLogicInGame.instance.SetOutput("\n", false);
        int valueGained = 5; // Set dynamically
        DataManager.instance.SetCoins(valueGained);
        CanvasLogicInGame.instance.SetOutput("Coins gained: " + valueGained);
    }

    private IEnumerator GotoTime(float seconds) {
        string waitDots = "...";
        CanvasLogicInGame.instance.SetOutput("travelling ", false);
        foreach (char c in waitDots)
        {
            CanvasLogicInGame.instance.SetOutput(c.ToString() + " ", false);
            yield return new WaitForSeconds(seconds / waitDots.Length);
        }
        CanvasLogicInGame.instance.SetOutput("\n", false);
        CanvasLogicInGame.instance.SetOutput("Player has arrived to: " + instance.currentPlayerLocation.ToString());
    }
    #endregion

}
