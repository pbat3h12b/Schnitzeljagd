using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {

	public Texture2D switchOn;		//Die Textur wenn der Schalter an ist
	public Texture2D switchOff;		//Die Textur wenn der Schalter aus ist
	public bool switchState;		//Abfrage welchen Status der Schalter hat
	public int id;

	private CommandManager cm;		//Einbindung des CommandManagers

	// Use this for initialization
	void Start () {
		renderer.material.SetTexture ("_MainTex", switchOff);	//Die Standardtextur wird auf aus gestellt
		switchState = false;			//Die Abfrage wird Standardmässig auf aus gestellt
		cm = GameObject.FindGameObjectWithTag("GameController").GetComponent<CommandManager>();
		//Es wird das GameObjekt mit dem Tag "GameController" gesucht und die Daten die es
		//dank des Scripts CommandManager sammelt werden hier eingebunden
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Diese Methode sorgt dafür, dass der Schalter umgeschalten wird
	void ToggleSwitch()
	{
		//Nur wenn das Spiel noch läuft kann der Schalter betätigt werden
		if(Time.timeScale == 1)
		{
			cm.ChangeSwitch(id);	//Der CommandManager registriert die Änderung des Schalters und setzt einen neuen Kommando wenn die Anweusung erfolgreich war

			//Je nachdem wie der Status des Schalters ist, wird die Textur geändert 
			if(switchState)
			{
				renderer.material.SetTexture ("_MainTex", switchOn);
			}
			else
			{
				renderer.material.SetTexture ("_MainTex", switchOff);
			}
		}
	}

	//Diese Automatische Funktion wird stetig aufgerufen wenn die Maus über dem Obejkt mit dem Script ist
	void OnMouseOver()
	{
		//Wenn auf dem Objekt die linke Maustaste/ mit dem Finger getippt wird,
		//wird ToggleSwitch() ausgeführt
		if(Input.GetMouseButtonDown(0))
			ToggleSwitch();
	}
}
