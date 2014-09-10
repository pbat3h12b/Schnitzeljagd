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
	#endregion
}
