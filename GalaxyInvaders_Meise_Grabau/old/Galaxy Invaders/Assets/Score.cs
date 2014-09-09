using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	// Use this for initialization

	// Übergeben wird der Wert Score, momentan nur Hilfsvariable
	int score = 0;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI()
	{

		//FONT = GUI.skin.font = MyFont;
		//Verändern der Schriftgröße um die Score lesbar zu machen
		GUIStyle fontStyle = new GUIStyle();
		fontStyle.fontSize = 25;
		//Textfeld dass die bereits erreichte Punktzahl des Spielers anzeigt 
		GUI.Label(new Rect(Screen.width/1.2f,Screen.height/10, 100,100), "="+score, fontStyle);
	}
}
