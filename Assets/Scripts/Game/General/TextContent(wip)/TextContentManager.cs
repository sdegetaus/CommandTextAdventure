using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextContentManager : MonoBehaviour {

    static public TextContentManager instance;

    private Dictionary<string, string> textContent;
    private bool isReady = false;

    private void Awake() {
        instance = this;
    }

    public void LoadText(string fileName) {
        textContent = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(filePath)) {
            string dataAsJson = File.ReadAllText(filePath);
            TextContentData loadedData = JsonUtility.FromJson<TextContentData>(dataAsJson);
            for (int i = 0; i < loadedData.items.Length; i++) {
                textContent.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
            Debug.Log("Data Loaded. Dictionary contains: " + textContent.Count + " entries");

        } else {
            Debug.LogError("Cannot Find JSON File!");
        }
        isReady = true;
    }

    public string GetTextContentValue(string key) {
        string result = "Text Not Found";
        if (textContent.ContainsKey(key)) {
            result = textContent[key];
        }
        return result;
    }

    public bool GetIsReady() {
        return isReady;
    }
}
