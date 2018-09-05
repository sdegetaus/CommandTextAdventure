using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour {

    static public Console instance;

    //private string action;
    //private string noun;
    //private string var;

    private void Awake() {
        instance = this;
    }

    //public Console(string _action, string _noun = "", string _var = "") {
    //    action = _action;
    //    noun = _noun;
    //    var = _var;
    //}

    public void MyFunction()
    {
        Debug.Log("WOW!");
    }

    //public void Change(string _noun, string _var = "") {

    //}

}
