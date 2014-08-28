using UnityEngine;
using System.Collections;

public class FireProjectil : MonoBehaviour {

	//Klasse zum Abfeuern von Projektilen (Spielerseitig)
	//Erstellt von Fabian Meise am 23.8.2014

	#region Attribute
	//Beinhaltet das Gameobject des Projektils
	public GameObject projectil;
	//nach reloadTime-Frames wird Projektil abgefeuert
	public double reloadFrames = 25;
	//Frames nach Abschuss des Projektils
	public double remainFrames;
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
			Fire();
		}
		else
		{
			//+ ein Frame
			remainFrames+=1;
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