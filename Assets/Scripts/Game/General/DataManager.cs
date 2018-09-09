using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DataManager takes care of all the game-related data; such as in-game currency, general player-prefs, etc.
/// </summary>
public class DataManager : MonoBehaviour {

    static public DataManager instance;

    public PlayerLocation currentPlayerLocation;
    public PlayerLocation previousPlayerLocation;

    private int stats_totalCoins { get { return PlayerPrefs.GetInt(_Cn.data_TotalCoins); } set { PlayerPrefs.SetInt(_Cn.data_TotalCoins, value); } }
    private int data_PlayerLocation { get { return PlayerPrefs.GetInt(_Cn.data_PlayerLocation); } set { PlayerPrefs.SetInt(_Cn.data_TotalCoins, value);  } }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        currentPlayerLocation = (PlayerLocation)data_PlayerLocation; // Save Prefs
    }

    #region Currency Methods

    public void SetCoins(int amount) {
        stats_totalCoins += amount;
    }

    public int GetCoins() {
        return PlayerPrefs.GetInt(_Cn.data_TotalCoins);
    }

    #endregion

    #region Location Methods

    public string GetPlayerCurrentLocation() {
        return currentPlayerLocation.ToString();
    }

    public void SetPlayerCurrentLocation(PlayerLocation playerLocation) {
        previousPlayerLocation = currentPlayerLocation;
        currentPlayerLocation = playerLocation;
        data_PlayerLocation = (int)(PlayerLocation)currentPlayerLocation;
        PlayerPrefs.SetInt(_Cn.data_PlayerLocation, (int)(PlayerLocation)currentPlayerLocation);
    }

    #endregion

}
