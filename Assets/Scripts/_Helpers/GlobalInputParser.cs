using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// GlobalInputParser is a regular C# class that contains a set of methods which help with common string to "object" parsing. For example, hexadecimal value to a color.
/// </summary>
public class GlobalInputParser {

    static public Color StringToColor(string hexValue) {
        // Removing '#' char at beginning
        if (hexValue.StartsWith("#")) { hexValue = hexValue.Substring(1); }
        // If lenght doesn't contain 6 or 3 chars return with error
        switch(hexValue.Length) {
            case 3: // If three numbers where parsed, duplicate them
                hexValue = hexValue + hexValue;
                break;
            case 6: // Do nothing
                break;
            default:
                Debug.LogError("Hex Value Not Formatted Correctly!"); // ToDo
                return new Color(0, 0, 0);
        }
        // Separate HexValue string into 2's (ff ff ff)
        List<string> separatedHexValues = new List<string>();
        for (int i = 0; i < hexValue.Length; i+=2) {
            separatedHexValues.Add(hexValue.Substring(i, Mathf.Min(2, hexValue.Length - i)));
        }
        // Converting each pair of letters to Hexadecimal
        int[] stringValuesToHex = new int[3];
        for (int i = 0; i < 3; i++) {
            stringValuesToHex[i] = Int32.Parse(separatedHexValues[i].ToString(), System.Globalization.NumberStyles.HexNumber);
            Debug.Log(stringValuesToHex[i]);
        }
        // Returning values as individual floats to fit Color's attributes (255/255 = 1)
        return new Color(stringValuesToHex[0] / 255.0f, stringValuesToHex[1] / 255.0f, stringValuesToHex[2] / 255.0f);
    }

}
