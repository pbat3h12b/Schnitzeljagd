using UnityEngine;
using System.Collections;

public class PlayerHealthController : MonoBehaviour {

	//Klasse zur Lebensverwaltung des Spielers
	//Erstellt von Fabian Meise am 23.8.2014

	#region Attribute
	//Leben des Gameobjects
	public float currentHealth = 6;
	#endregion

	#region Methoden
	//Hier wird der Schaden/Gesundheit zugefügt
	//float damage ist der übertragende Schaden
	void ApplyDamage(float damage)
	{
		if(currentHealth > 0)
		{
			if(currentHealth-damage<=0)
			{
				//Wenn keine Leben mehr vorhanden sind
				GameOver();
			}
			else
			{
				//Schaden wird verrechnet
				currentHealth -= damage;								
			}
		}
	}
	
	
	void GameOver()
	{
		//Szenenwechsel
	}
	#endregion
}