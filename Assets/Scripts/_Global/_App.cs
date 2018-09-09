using UnityEngine;

public class _App : MonoBehaviour {
    static public _App instance;

    void Awake() {
        instance = this;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        //Application.runInBackground = _Cn.debugOn ? true : false;
        //Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void OnApplicationPause() {
        Debug.Log("OnApplicationPause");
    }

    private void OnApplicationQuit() {
        PlayerPrefs.Save();
    }
}
