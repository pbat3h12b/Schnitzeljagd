using UnityEngine;
using System.Collections;

public class ShowScore : MonoBehaviour {
	//Klasse zur Lebensverwaltung von Objekten
	//Erstellt von Fabian Meise
	//!!!Klasse unter Bearbeitung"
	
	#region Attribute
	//erreichte Punkte
	int score=0;
	#endregion
	
	
	#region Methoden
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	//Addierung der Punkte
	public void ApplyScore(int points)
	{
		score += points;
	}

	void OnGUI()
	{
		
		//FONT = GUI.skin.font = MyFont;
		//Verändern der Schriftgröße um die Score lesbar zu machen
		GUIStyle fontStyle = new GUIStyle();
		fontStyle.fontSize = 25;
		//Textfeld dass die bereits erreichte Punktzahl des Spielers anzeigt 
		GUI.Label(new Rect(Screen.width/1.2f,Screen.height/10, 100,100),"" + score, fontStyle);
	}
	#endregion
}
