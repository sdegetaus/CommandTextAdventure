using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class _TutorialScripts : MonoBehaviour {

    [SerializeField] private Prueba prueba;

    private void Start() {
        prueba._text = new Dictionary<string, string>();
        prueba._text.Add(prueba._textKey[0], prueba._textValue[0]);
        Debug.Log(prueba._text[prueba._textKey[0]]);
    }

}
