using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DataManager takes care of all the game-related data; such as in-game currency, general player-prefs, etc.
/// </summary>
public class DataManager : MonoBehaviour {

    static public DataManager instance;

    private int totalCoins;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        totalCoins = PlayerPrefs.GetInt(_Cn.data_TotalCoins, 0);
    }

    public void SetCoins(int amount) {
        totalCoins += amount;
        PlayerPrefs.SetInt(_Cn.data_TotalCoins, totalCoins);
    }

    public int GetCoins() {
        return PlayerPrefs.GetInt(_Cn.data_TotalCoins);
    }



}
