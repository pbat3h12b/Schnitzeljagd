using UnityEngine;
using System.Collections;

public class QRCode : MonoBehaviour {

	string qrcode = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUI.Box (new Rect (Screen.width / 13, Screen.height / 13, Screen.width * 0.85f, Screen.height / 2 + 100), "");
		GUI.Label (new Rect (Screen.width / 11, Screen.height / 10, 200, 100), "QR-Code Scanner");
		qrcode = GUI.TextField (new Rect (Screen.width / 11, Screen.height / 5, Screen.width / 2, 25), qrcode);
		GUI.Label (new Rect (Screen.width / 11, Screen.height / 4, 200, 50), "Bitte Code eingeben");
        if (GUI.Button(new Rect(Screen.width / 2 - 130, Screen.height / 3, 100, 100), "Zurück"))
        {
            Application.LoadLevel(4);
        }
		if (GUI.Button (new Rect (Screen.width / 2, Screen.height / 3, 100, 100), "Weiter") )
        {
            Application.LoadLevel(6);
        }
	}
}
