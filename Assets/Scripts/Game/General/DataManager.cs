using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DataManager takes care of all the game-related data; such as in-game currency, general player-prefs, etc.
/// </summary>
public class DataManager : MonoBehaviour {

    static public DataManager instance;

    private int stats_totalCoins { get { return PlayerPrefs.GetInt(_Cn.data_TotalCoins); } set { PlayerPrefs.SetInt(_Cn.data_TotalCoins, value); } }
    private int data_PlayerLocation { get { return PlayerPrefs.GetInt(_Cn.data_PlayerLocation); } set { PlayerPrefs.SetInt(_Cn.data_TotalCoins, value);  } }

    private void Awake() {
        instance = this;
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

    public PlayerLocation GetPlayerCurrentLocation() {
        Debug.Log((PlayerLocation)data_PlayerLocation);
        return (PlayerLocation)data_PlayerLocation;
    }

    public void SetPlayerCurrentLocation(PlayerLocation playerLocation) {
        data_PlayerLocation = (int)(PlayerLocation)playerLocation;
        Debug.Log((PlayerLocation)data_PlayerLocation);
    }

    #endregion

}
