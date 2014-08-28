using UnityEngine;
using System.Collections;

public class GameOverCollider : MonoBehaviour {

	//Klasse für GameOver
	//Erstellt von Fabian Meise am 23.8.2014

	//Das Spiel ist vorbei sobald ein Gegner auf das mit dieser
	//Klasse verbundende Objekt trifft
	

	#region Methoden
	//Es werden alle 3 Triggermethoden benutzt um mögliche Fehler
	//zu vermeiden

	//wird aufgerufen wenn ein der Collider eines anderen
	//Gameobjects den Trigger berührt
	//other ist das andere Gameobject
	void OnTriggerEnter(Collider other)
	{	
		//Aufruf von Senddamage
		GameOver();
	}
	//wird aufgerufen wenn ein der Collider eines anderen
	//Gameobjects im Trigger befindet
	//other ist das andere Gameobject
	void OnTriggerStay(Collider other)
	{
		//Aufruf von Senddamage
		GameOver();
	}
	//wird aufgerufen wenn ein der Collider eines anderen
	//Gameobjects den Trigger berührt
	//other ist das andere Gameobject
	void OnTriggerExit(Collider other)
	{
		//Aufruf von Senddamage
		GameOver();
	}
	
	
	//Methode zum Senden von Schaden an Objekt
	void GameOver()
	{
		//Szenenwechsel	

	}
	#endregion
}