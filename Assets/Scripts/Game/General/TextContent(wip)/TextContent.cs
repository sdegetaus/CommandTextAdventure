using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextContent : MonoBehaviour {

    public string key;

	void Start () {
        string text;
        text = TextContentManager.instance.GetTextContentValue(key);
	}

    // TODO
    public string GetTextContent(string key) {
        return TextContentManager.instance.GetTextContentValue(key);
    }

}
