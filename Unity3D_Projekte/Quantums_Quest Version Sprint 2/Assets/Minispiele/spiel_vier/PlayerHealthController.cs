using UnityEngine;
using System.Collections;

public class PlayerHealthController : MonoBehaviour {
	
	//Klasse zur Lebensverwaltung von Objekten
	//Erstellt von Fabian Meise am 23.8.2014
	//Zuletzt bearbeitet am 04.09.2014 von Florens Grabau
	
	#region Attribute
	//Leben des Gameobjects
	public float currentHealth = 6;
	//Die life Variablen bilden die Lebensanzeige
	public GameObject life1;
	public GameObject life2;
	public GameObject life3;
	//Die Texturen zum verändern der Lebensanzeige
	public Texture2D fullHealth;
	public Texture2D halfHealth;
	public Texture2D noHealth;
	#endregion
	void Start () {

	}

	void Update()
	{
		HealthBar ();
	}
	#region Methoden
	//Erstellt von Fabian Meise
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

	//Erstellt von Florens Grabau
	//Die Methode ist dafür da die Lebensanzeige bei jedem Treffer
	//zu aktualisieren
	void HealthBar()
	{
		if (currentHealth == 6) {
			life3.renderer.material.mainTexture = fullHealth;
			life2.renderer.material.mainTexture = fullHealth;
			life1.renderer.material.mainTexture = fullHealth;
		}
		else if (currentHealth == 5) {
			life3.renderer.material.mainTexture = halfHealth;
		}
		else if (currentHealth == 4) {
			life3.renderer.material.mainTexture = noHealth;
		}
		else if (currentHealth == 3) {
			life2.renderer.material.mainTexture = halfHealth;
		}
		else if (currentHealth == 2) {
			life2.renderer.material.mainTexture = noHealth;
		}
		else if (currentHealth == 1) {
			life1.renderer.material.mainTexture = halfHealth;
		}
		else if (currentHealth == 0) {
			life3.renderer.material.mainTexture = noHealth;
			GameOver();
		}

		
	}
	//Erstellt von Fabian Meise
	void GameOver()
	{
		//Szenenwechsel
        GameObject.Find("GameController").GetComponent<PlayerInformation>().newScore("HNF", GameObject.Find("ScoreDisplay").GetComponent<ShowScore>().score);
		Application.LoadLevel(4);
	}
	#endregion
}