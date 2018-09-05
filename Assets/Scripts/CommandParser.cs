using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandElements { Object, Action, Noun, Vars }

//public enum Prueba { Object, Action, Noun, Vars  }

public struct CommandStructure {
    public string Object;
    public string Action;
    public string Noun;
    //public string[] Vars;
    
    public CommandStructure(string _object, string _action, string _noun) {
        Object = _object;
        Action = _action;
        Noun = _noun;
    }
}

public class CommandParser : MonoBehaviour {

    string temp = "console change bg";
    List<string> objects = new List<string> { "console", "game", "other" };

    static Hashtable commands;

    private void Start() {
        commands = new Hashtable();

        // To be parsed from JSON or something alike
        //commands.Add(0, new CommandStructure("console","change","bg"));
        //commands.Add(1, new CommandStructure("game", "save", "-s"));
        //commands.Add(2, new CommandStructure("game", "help", "-h"));

        //// Print HashTable
        //foreach (DictionaryEntry entry in commands) {
        //    Debug.Log("Key: " + entry.Key + " | Value: " + entry.Value);
        //}

        //if(commands.ContainsValue(new CommandStructure("game", "help", "-h"))) {
        //    Debug.Log("It does!");
        //}
    }

    private List<string> ParseCommand(string command) {
        string[] splitString = command.Split(' ');
        var list = new List<string>();
        list.AddRange(splitString);
        return list;
    }

    //private bool StartsWithObject(List<string> a)
    //{
        
    //}

}

#region HashTable Tutorial
// Hashtable Tutorial
//static Hashtable userInfoHash;

//// -- Hashtable Tutorial --

//userInfoHash = new Hashtable();
////userInfoList = new List<UserInfo>();

//// Adding
//for (int i = 0; i< 10; i++) {
//    userInfoHash.Add(i, "user" + i);
//    //userInfoList.Add(new UserInfo(i, "user" + i));
//}

//// Removing
//if(userInfoHash.ContainsKey(0)) {
//    userInfoHash.Remove(0);
//}

//// Setting 
//if (userInfoHash.ContainsKey(1)) {
//    userInfoHash[1] = "replacementeName";
//}

//// Looping
//foreach(DictionaryEntry entry in userInfoHash) {
//    Debug.Log("Key : " + entry.Key + " / Value: " + entry.Value);
//}
#endregion  