using UnityEngine;
using System.Collections;

public class Regulator : MonoBehaviour {

	public int id;		//Die ID des Regulator
	public Camera camera;	//Die Kamera die im Spiel verwendet wird
	CommandManager cm;	//Einbindung des CommandManagers

	// Use this for initialization
	void Start () 
	{
		//Hier wird der CommandManager eingebunden
		cm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<CommandManager> ();	
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	//In dieser Methode wird der Regler bewegt
	void Movement()
	{
	
		Vector3 position = transform.position;

		//Wenn ein Finger den Touchscreen des Smartphones benutzt wird es ausgeführt
		if(Time.timeScale == 1)
		{
			if (Input.touchCount == 1)
			{
			//Nur wenn das Spiel noch läuft kann der Regulator bewegt werden.

				position = camera.ScreenToWorldPoint(Input.touches[0].position);	//Die Position des Fingers auf dem Screen wird in die Spielweltposition konvertiert
				position.x = 1.938336f;
				position.z = -1;
				position.y = Mathf.Clamp (position.y, -3.755138f, 2.190039f);		//Die Regler kann sich nur in einem bestimmten Bereich bewegen
			}
			if(Input.GetMouseButton(0))
			{
				position = camera.ScreenToWorldPoint(Input.mousePosition);	//Die Position der Maus auf dem Screen wird in die Spielweltposition konvertiert
				position.x = 1.938336f;
				position.z = -1;
				position.y = Mathf.Clamp (position.y, -3.755138f, 2.190039f);		//Die Regler kann sich nur in einem bestimmten Bereich bewegen
			}
			transform.position = position;

			//Wenn die Maustaste losgelassen wird oder wenn der Finger vom Screen genommen wird
			if(Input.GetMouseButtonUp(0))
			{
				PositionChecker ();
			}
		}


	}

	//Wenn sich die Maus, in diesem Fall der Finger, auf dem Objekt befindet wird die Methode ausgeführt
	void OnMouseOver()
	{
		Movement ();
	}
	
	//Diese Methode überprüft wo sich der Regulator befindet und ob er sich im richtigen Feld befindet.
	//Zusätzlich wird er automatisch auf das Feld gelegt die am nächsten ist.
	void PositionChecker()
	{
		//Die aktuelle Position des Regulators wird aufgefangen.
		Vector3 position = transform.position;	
		
		//Wenn sich der Regulator im oberen Bereich befindet setzt er sich auf Feld 3.
		if(transform.position.y <= 2.190039f && transform.position.y >= 1.5f)
		{
			position.y = 2.190039f;	//Die Y-Position wird hierbei gesetzt.
		}
		//Wenn sich der Regulator im mittleren Bereich befindet setzt er sich auf Feld 2.
		if(transform.position.y <= -0.05f && transform.position.y >= -1.5f)
		{
			position.y = -0.77686f; //Die Y-Position wird hierbei gesetzt.
		}
		//Wenn sich der Regulator im unteren Bereich befindet setzt er sich auf Feld 1.
		if(transform.position.y <= -3 && transform.position.y >= -3.755138f)
		{
			position.y = -3.755138f; //Die Y-Position wird hierbei gesetzt.
		}

		//Die veränderten Werte werden auf die Position des Regulators übertragen
		transform.position = position;	
		
		//Hier wird überprüft im welchen Feld sich der Regulator befindet.
		//Feld 1
		if(transform.position.y == 2.190039f)
		{
			cm.ChangeRegulator(id+1);
		}
		//Feld 2
		else if(transform.position.y == -0.77686f)
		{
			cm.ChangeRegulator(id + 2);
		}
		//Feld 3
		else if(transform.position.y == -3.755138f)
		{
			cm.ChangeRegulator(id + 3);
		}
	}

}
