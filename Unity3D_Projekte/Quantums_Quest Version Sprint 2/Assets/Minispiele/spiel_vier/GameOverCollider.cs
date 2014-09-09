using UnityEngine;
using System.Collections;

public class GameOverCollider : MonoBehaviour {
	
	//Klasse zur Lebensverwaltung von Objekten
	//Erstellt von Fabian Meise am 23.8.2014
	//Zuletzt bearbeitet am 28.8.2014
	
	//Das Spiel ist vorbei sobald ein Gegner auf das mit dieser
	//Klasse verbundende Objekt trifft
	
	#region Methoden
	//Überprüfen ob nur Enemy sich im Collider befindet
	public string tagName="enemy";
	#endregion
	
	
	#region Methoden
	//Es werden alle 3 Triggermethoden benutzt um mögliche Fehler
	//zu vermeiden
	
	//wird aufgerufen wenn ein der Collider eines anderen
	//Gameobjects den Trigger berührt
	//other ist das andere Gameobject
	void OnTriggerEnter(Collider other)
	{	
		//Aufruf von Senddamage
		GameOver(other);
	}
	//wird aufgerufen wenn ein der Collider eines anderen
	//Gameobjects im Trigger befindet
	//other ist das andere Gameobject
	void OnTriggerStay(Collider other)
	{
		//Aufruf von Senddamage
		GameOver(other);
	}
	//wird aufgerufen wenn ein der Collider eines anderen
	//Gameobjects den Trigger berührt
	//other ist das andere Gameobject
	void OnTriggerExit(Collider other)
	{
		//Aufruf von Senddamage
		GameOver(other);
	}
	
	
	//Methode zum Senden von Schaden an Objekt
	void GameOver(Collider other)
	{
		if (other.gameObject.tag == tagName)
		{
			//Szenenwechsel	
			Application.LoadLevel(1);
		}
	}
	#endregion
}