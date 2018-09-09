using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidden : MonoBehaviour {

    static public Hidden instance;

    private void Awake() {
        instance = this;
    }

    static public void Hello(string[] nounAndVar = null) {
        OutputController.instance.SetOutput("Hello!");
    }
}
