using UnityEngine;
using System.Collections;

public class Regulator : MonoBehaviour {

	public int id;
	public Camera camera;
	CommandManager cm;

	// Use this for initialization
	void Start () 
	{
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
				position = camera.ScreenToWorldPoint(Input.mousePosition);	//Die Position des Fingers auf dem Screen wird in die Spielweltposition konvertiert
				position.x = 1.938336f;
				position.z = -1;
				position.y = Mathf.Clamp (position.y, -3.755138f, 2.190039f);		//Die Regler kann sich nur in einem bestimmten Bereich bewegen
			}
			transform.position = position;


			if(Input.GetMouseButtonUp(0) || Input.touchCount == 0)
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

	void PositionChecker()
	{
		Vector3 position = transform.position;
		if(transform.position.y <= 2.190039f && transform.position.y >= 1.5f)
		{
			position.y = 2.190039f;
		}
		if(transform.position.y <= -0.05f && transform.position.y >= -1.5f)
		{
			position.y = -0.77686f;
		}
		if(transform.position.y <= -3 && transform.position.y >= -3.755138f)
		{
			position.y = -3.755138f;
		}

		transform.position = position;
		
		if(transform.position.y == 2.190039f)
		{
			cm.ChangeRegulator(id+1);
		}
		else if(transform.position.y == -0.77686f)
		{
			cm.ChangeRegulator(id + 2);
		}
		else if(transform.position.y == -3.755138f)
		{
			cm.ChangeRegulator(id + 3);
		}
	}

}
