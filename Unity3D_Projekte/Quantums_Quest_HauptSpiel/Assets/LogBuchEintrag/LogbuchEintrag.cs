using UnityEngine;
using System.Collections;

public class LogbuchEintrag : MonoBehaviour {

	string lgeintrag = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUI.Box (new Rect (Screen.width / 13, Screen.height / 13, Screen.width * 0.85f, Screen.height / 2 + 100), "");
		lgeintrag = GUI.TextField (new Rect (Screen.width / 11, Screen.height / 9, Screen.width / 2, Screen.width / 3), lgeintrag);
		GUI.Label (new Rect (Screen.width / 2 + 200, Screen.height / 9, 200, 200), "Hier können Sie eine Nachricht hinterlassen");
		GUI.Button (new Rect (Screen.width / 2 + 250, Screen.height / 3, 100, 100), "Weiter");
	}
}
