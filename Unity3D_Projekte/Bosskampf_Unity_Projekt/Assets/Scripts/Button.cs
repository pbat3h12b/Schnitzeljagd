using UnityEngine;
using System.Collections;

//Dieses Script wurde von Kai Bursmeier erstellt.

public class Button : MonoBehaviour {

	public int id;	//Die id des jeweiligen Buttons
	private CommandManager cm;	//Das Script "CommandoManager.cs" wird hier eingebunden...
	// Use this for initialization
	void Start () {
		cm = GameObject.FindGameObjectWithTag("GameController").GetComponent<CommandManager> (); //..und hier zugewiesen
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Überprüfung ob die Maus über dem Button ist.
	void OnMouseOver()
	{
		//Bei einem Mausklick oder einer Toucheingabe wird die Methode "ChangeIndex" ausgeführt
		if (Input.GetMouseButtonDown (0)) 
		{
			cm.ChangeButton(id);
		}
	}
}
