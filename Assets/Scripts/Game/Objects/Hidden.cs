using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidden : MonoBehaviour {

    static public Hidden instance;

    //private OutputController outputController;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        //outputController = OutputController.instance;
    }

    static public void Help(string[] nounAndVar = null) {
        OutputController.instance.SetOutput("Random Characters Here");
    }

    static public void Hello(string[] nounAndVar = null) {
       OutputController.instance.SetOutput("Hello!");
    }
}
