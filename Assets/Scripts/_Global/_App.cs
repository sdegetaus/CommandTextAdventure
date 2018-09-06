using UnityEngine;

public class _App : MonoBehaviour
{
    public static _App instance;

    //public delegate void GenericHandler();
    //public event GenericHandler OnMainMenuLoaded;

    void Awake() {
        instance = this;

        // TO DO
        //QualitySettings.vSyncCount = 0;  // VSync must be disabled
        //Application.targetFrameRate = 60;
        //Application.runInBackground = _Cn.debugOn ? true : false;
        //Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void LoadPreferences() {
        // ToDo
    }

    void OnApplicationPause(bool pauseStatus) {
        Debug.Log("OnApplicationPause");
    }

    void OnApplicationQuit() {
        Debug.Log("OnApplicationQuit");
        PlayerPrefs.Save();
    }
}
