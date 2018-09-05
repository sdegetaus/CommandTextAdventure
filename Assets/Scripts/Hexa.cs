using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexa : MonoBehaviour {

    [SerializeField] private string hexValue = "#abcdef";
    [SerializeField] private Image bg;

    private Color HexToColor(string hex) {

        if(hex.StartsWith("#")) {
            hex = hex.Substring(1);
        } else {
            Debug.LogError("Hex Value Not Formatted Correctly!");
            return new Color(0,0,0);
        }

        if (hex.Length != 6) {
            Debug.LogError("Hex Value Not Formatted Correctly!");
            return new Color(0, 0, 0);
        }

        Debug.Log(hex);

        List<string> values = new List<string>();
        for (int i = 0; i < hex.Length; i += 2) {
            values.Add(hex.Substring(i, Mathf.Min(2, hex.Length - i)));
        }

        Debug.Log(string.Join(" ", values.ToArray()));

        int[] valuesInDecimal = new int[3];
        for (int i = 0; i < values.Count; i++) {
            valuesInDecimal[i] = int.Parse(values[i], System.Globalization.NumberStyles.HexNumber);
            Debug.Log(valuesInDecimal[i]);
        }

        return new Color(valuesInDecimal[0] / 255, valuesInDecimal[1] / 255, valuesInDecimal[2] / 255);
    }

    private void Start()
    {
        HexToColor(hexValue);
    }
    void Update () {
        bg.color = HexToColor(hexValue);
	}

}
