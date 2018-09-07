using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputController : MonoBehaviour {

    static public OutputController instance;

    private void Awake() {
        instance = this;
    }

    //public void Output(bool wait = false) {
    //    if (!wait) {
    //        ConsoleResponseHandling.instance.ThrowResponse(ConsoleResponseHandling.ResponseType.Done);
    //    } else {
    //        StartCoroutine(WaitToOutput(5.0f));
    //    }
    //}

    //private IEnumerator WaitToOutput(float seconds) { 
    //    string waitDots = "......";
    //    foreach (char c in waitDots){
    //        CanvasLogicInGame.instance.SetOutput(c.ToString() + " ", false);
    //        yield return new WaitForSeconds(5.0f / waitDots.Length);
    //    }             
    //    CanvasLogicInGame.instance.SetOutput("\n", false);
    //    ConsoleResponseHandling.instance.ThrowResponse(ConsoleResponseHandling.ResponseType.Done);
    //}

}
