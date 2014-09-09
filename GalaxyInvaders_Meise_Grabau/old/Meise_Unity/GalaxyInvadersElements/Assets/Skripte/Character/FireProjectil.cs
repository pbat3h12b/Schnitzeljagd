using UnityEngine;
using System.Collections;

public class FireProjectil : MonoBehaviour {

	//Klasse zur Lebensverwaltung von Objekten
	//Erstellt von Fabian Meise am 23.8.2014
	//Zuletzt bearbeitet am 28.8.2014

	#region Attribute
	//Beinhaltet das Gameobject des Projektils
	public GameObject projectil;
	//nach reloadTime-Frames wird Projektil abgefeuert
	public double reloadFrames = 25;
	//Frames nach Abschuss des Projektils
	public double remainFrames;
	//Optimierung unterschiedliche Schussart von Enemy/Spieler
	public bool isEnemy=false;
	//Trefferchance für Gegner
	public int FireChance=11;
	#endregion
	
	#region Methoden
	// Update is called once per frame
	void Update () {
		//Darf ein Projektil abgeschossen werden
		if (reloadFrames == remainFrames)
		{
			//Zurücksetzen des Framecounters
			remainFrames=0.0;
			//Initialisierung des Projektils
		    if(isEnemy)
			{
				EnemyFire();
			}
			else
			{
			Fire();
			}
		}
		else
		{
			//+ ein Frame
			remainFrames+=1;
		}
	}

	//Kann Enemy einen Schuss abfeuern?
	public void EnemyFire()
	{
		//Randomzahl
		int random;
		random = Random.Range (1, FireChance);

		//Prüft ob Enemy feuern darf
		if (random == 1)
		{
			Fire();		
		}

	}
	
	//Initialisierung des Projektils
	public void Fire()
	{
		//Startposition bestimmen
		//anhand der Position des abfeuernden Gameobjects
		Vector3 startposition = new Vector3 (transform.position.x,
		                                     transform.position.y + 1f );
		//Instanziierung des Projectils
		GameObject.Instantiate (projectil, startposition, transform.rotation);
	}
	#endregion
}