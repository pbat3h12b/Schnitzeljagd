using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {

	//Klasse zur Lebensverwaltung von Objekten
	//Erstellt von Fabian Meise am 23.8.2014

	#region Attribute
	//Leben des Gameobjects
	public float currentHealth = 1;
	#endregion

	#region Methoden
	//Hier wird der Schaden/Gesundheit zugefügt
	//float damage ist der übertragende Schaden
	void ApplyDamage(float damage)
	{
		//Schadensverrechnung
		if(currentHealth > 0)
		{
			if(currentHealth-damage<=0)
			{
				//Wenn keine Leben mehr vorhanden sind
				Destroythis();
			}
			else
			{
				//Schaden wird verrechnet
				currentHealth -= damage;								
			}
		}
	}

	//Gameobject wird zerstört
	void Destroythis()
	{
		GameObject.Destroy (this.gameObject);
	}
	#endregion
}