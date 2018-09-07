using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TextContentEditor : EditorWindow {

    public TextContentData textContentData;

    [MenuItem("Window/Text Content Editor")]
    static private void Init() {
        EditorWindow.GetWindow(typeof(TextContentEditor)).Show();
    }

    private void OnGUI() {
        if (textContentData != null) {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("textContentData");
            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save Data")) {
                SaveData();
            }
        }

        if (GUILayout.Button("Load Data")) {
            LoadData();
        }

        if (GUILayout.Button("Create New Data")) {
            CreateNewData();
        }
    }

    private void LoadData() {
        string filePath = EditorUtility.OpenFilePanel("Select Data File", Application.streamingAssetsPath, "json");
        if (!string.IsNullOrEmpty(filePath)) {
            string dataAsJson = File.ReadAllText(filePath);
            textContentData = JsonUtility.FromJson<TextContentData>(dataAsJson);
        }
    }

    private void SaveData() {
        string filePath = EditorUtility.SaveFilePanel("Save Data File", Application.streamingAssetsPath, "", "json");
        if (!string.IsNullOrEmpty(filePath)) {
            string dataAsJson = JsonUtility.ToJson(textContentData);
            File.WriteAllText(filePath, dataAsJson);
        }
    }

    private void CreateNewData() {
        textContentData = new TextContentData();
    }

}
