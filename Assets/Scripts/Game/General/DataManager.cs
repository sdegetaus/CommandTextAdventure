using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DataManager takes care of all the game-related data; such as in-game currency, general player-prefs, etc.
/// </summary>
public class DataManager : MonoBehaviour {

    static public DataManager instance;

    // Location States
    public PlayerLocation currentPlayerLocation;
    public PlayerLocation previousPlayerLocation;

    #region PlayerPrefs

    private int data_PlayerLocation { get { return PlayerPrefs.GetInt(_Cn.data_PlayerLocation); } set { PlayerPrefs.SetInt(_Cn.data_PlayerLocation, value);  } }

    // Statistics
    private int stats_CurrentCoins { get { return PlayerPrefs.GetInt(_Cn.stats_CurrentCoins); } set { PlayerPrefs.SetInt(_Cn.stats_CurrentCoins, value); } }
    private int stats_TotalCoins { get { return PlayerPrefs.GetInt(_Cn.stats_TotalCoins); } set { PlayerPrefs.SetInt(_Cn.stats_TotalCoins, value); } }
    private int stats_TotalCommandsEntered { get { return PlayerPrefs.GetInt(_Cn.stats_TotalCommandsEntered); } set { PlayerPrefs.SetInt(_Cn.stats_TotalCommandsEntered, value); } }

    #endregion

    private void Awake() {
        instance = this;
    }

    private void Start() {
        currentPlayerLocation = (PlayerLocation)data_PlayerLocation; // Save Prefs
    }

    #region Currency Methods

    public void SetCurrentCoins(int amount) {
        stats_CurrentCoins += amount;
    }

    public int GetCurrentCoins() {
        return PlayerPrefs.GetInt(_Cn.stats_CurrentCoins);
    }

    // Total Coins Gained for Statistics
    public void SetTotalCoins(int amount) {
        stats_TotalCoins += amount;
    }
    public int GetTotalCoins() {
        return PlayerPrefs.GetInt(_Cn.stats_TotalCoins);
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
    }

    #endregion

    #region Store Methods

    #endregion

    #region General Statistics

    public int GetTotalCommandsEntered() { return stats_TotalCommandsEntered; }
    public void SetTotalCommandsEntered() { stats_TotalCommandsEntered++; }

    #endregion
}
