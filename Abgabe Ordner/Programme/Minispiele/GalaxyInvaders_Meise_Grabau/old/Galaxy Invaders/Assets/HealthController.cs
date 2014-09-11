using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {
	
	//Klasse zur Lebensverwaltung von Objekten
	//Erstellt von Fabian Meise am 23.8.2014
	//Zuletzt bearbeitet am 28.8.2014
	
	#region Attribute
	//Leben des Gameobjects
	public float currentHealth = 1;
	//Punkte für Töten
	public int points = 10;
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
	public void Destroythis()
	{
		if (this.gameObject.tag == "enemy")
		{
			GameObject score;
			score = GameObject.FindGameObjectWithTag("score");
			score.SendMessage("ApplyScore",
			                  points,
			                  SendMessageOptions.DontRequireReceiver);
		}
		
		GameObject.Destroy (this.gameObject);
	}
	#endregion
}