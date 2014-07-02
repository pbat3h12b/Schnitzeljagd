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
		GUI.Box (new Rect (Screen.width / 13, Screen.height / 13, Screen.width * 0.85f, Screen.height * 0.85f), "");
		lgeintrag = GUI.TextField (new Rect (Screen.width / 11, Screen.height / 9, Screen.width / 2, Screen.width * 0.45f), lgeintrag);
		GUI.Label (new Rect (Screen.width / 1.5f, Screen.height / 9, 200, 200), "Hier können Sie eine Nachricht hinterlassen");
		GUI.Button (new Rect (Screen.width / 1.5f, Screen.height / 3, Screen.width * 0.15f, Screen.height * 0.1f), "Weiter");
	}
}
