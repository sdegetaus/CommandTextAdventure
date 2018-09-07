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
        instance.StartCoroutine(instance.WorkTime(5.0f));
    }

    static public void Get(string[] nounAndVar) {
        string _noun = nounAndVar[0];
        string _var = nounAndVar[1];

        switch(_noun)
        {
            case "Money":
            case "Coins":
                CanvasLogicInGame.instance.SetOutput("Total Coins: " + DataManager.instance.GetCoins().ToString());
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
    #endregion

}
