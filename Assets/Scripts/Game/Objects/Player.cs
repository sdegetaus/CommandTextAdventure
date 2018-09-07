using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public enum PlayerLocation {
        Start
    }

    static public Player instance;

    private void Awake() {
        instance = this;
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
        string _var = nounAndVar[1]; // ...
        //PlayerLocation playerLocation;
        //playerLocation = _noun;
        CanvasLogicInGame.instance.SetOutput(_noun);
    }

    static public void Work(string[] nounAndVar = null) {
        int valueGained = 5;
        DataManager.instance.SetCoins(5);
        CanvasLogicInGame.instance.SetOutput("You have made " + valueGained + ".");
        CanvasLogicInGame.instance.SetOutput("Total is: " + DataManager.instance.GetCoins());
    }

    #endregion

}
