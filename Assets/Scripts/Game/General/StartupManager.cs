using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupManager : MonoBehaviour {

	private IEnumerator Start() {
        while (!TextContentManager.instance.GetIsReady()) {
            yield return null;
        }
        //Some sort of scenemanagement here
    }
}
