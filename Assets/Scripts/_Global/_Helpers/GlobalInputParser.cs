using UnityEngine;
using System;
using System.Data;
using System.Collections.Generic;

/// <summary>
/// GlobalInputParser is a regular C# class that contains a set of methods which help with common string to "object" parsing. For example, hexadecimal value to a color.
/// </summary>
public class GlobalInputParser {

    /// <summary>
    /// Takes a string with a Hexadecimal value in it, and converts it into a usable Unity Color Struct.
    /// </summary>
    /// <param name="hexValue">Hexadecimal string, with or without a leading #</param>
    /// <returns>Unity Color</returns>
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
                ConsoleResponseHandling.instance.ThrowError(ConsoleResponseHandling.ErrorType.ValueNotFormattedCorrectly);
                return new Color(0, 0, 0);
        }
        // Separate HexValue string into 2's (ff ff ff)
        List<string> separatedHexValues = new List<string>();
        for (int i = 0; i < hexValue.Length; i+=2) {
            separatedHexValues.Add(hexValue.Substring(i, Mathf.Min(2, hexValue.Length - i)));
        }
        try {
            // Converting each pair of letters to Hexadecimal
            int[] stringValuesToHex = new int[3];
            for (int i = 0; i < 3; i++) {
                stringValuesToHex[i] = Int32.Parse(separatedHexValues[i].ToString(), System.Globalization.NumberStyles.HexNumber);
            }
            // Returning values as individual floats to fit Color's attributes (255/255 = 1)
            return new Color(stringValuesToHex[0] / 255.0f, stringValuesToHex[1] / 255.0f, stringValuesToHex[2] / 255.0f);
        } catch {
            ConsoleResponseHandling.instance.ThrowError(ConsoleResponseHandling.ErrorType.ValueNotFormattedCorrectly);
            return new Color(0, 0, 0);
        }
    }

    /// <summary>
    /// Checks if the provided string starts with and Uppercase letter, and returns it capitalized if not
    /// </summary>
    /// <param name="s">String to check</param>
    /// <returns>Capitalized version of string</returns>
    static public string SetFirstCharToUpper(string s) {
        // Check is string passed is null
        if (s == null) {
            return s;
        }
        // Check if it is uppercased
        if (char.IsUpper(s[0])) {
            return s;
        } else {
            char[] letters = s.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);
            return new string(letters);
        }
    }

    /// <summary>
    /// Evaluates a string mathematical expression and return the answer. Code by: Petar Repac at https://stackoverflow.com/a/1417488/8869187.
    /// </summary>
    /// <param name="expression">String passed as an expression.</param>
    /// <returns>Mathematical Expression Result</returns>
    static public double EvaluateMathExpression(string expression) {
        var loDataTable = new DataTable();
        var loDataColumn = new DataColumn("Eval", typeof(double), expression);
        loDataTable.Columns.Add(loDataColumn);
        loDataTable.Rows.Add(0);
        return (double)(loDataTable.Rows[0]["Eval"]);
    }

    // WIP - Doesn't work atm
    static public bool IsArithmeticOperation(string[] s) {
        int count = 0;
        for (int i = 0; i < s.Length; i++) {
            foreach (char c in s[i]) {
                if ((c == '+' || c == '-' || c == '*' || c == '/' || c == '%') && !char.IsLetter(c) && c != '=') {
                    count++;
                    Debug.Log(count);
                }
            }
        }
        Debug.Log(count + "?=" + s.Length);
        if (count == s.Length) {
            return true;
        } else {
            return false;
        }
    }
}
