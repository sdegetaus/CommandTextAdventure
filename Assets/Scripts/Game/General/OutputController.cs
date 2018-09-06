using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputController : MonoBehaviour {

    static public OutputController instance;

    private void Awake() {
        instance = this;
    }

}
