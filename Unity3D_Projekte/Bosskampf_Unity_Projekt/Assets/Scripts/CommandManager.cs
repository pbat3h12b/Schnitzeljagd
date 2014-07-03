using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommandManager : MonoBehaviour {

	public List<string> Commands = new List<string>(); //Liste der Anweisungen
	public int index;		// index in der Kommandoliste
	public int counter = 0;		//Counter wie oft eine Anweisung befolgt wurde
	public GUIStyle guiStyle;	//Formatierung des Textes

	private int ranRange;		//Zufallsbereich des Indexes der Kommandos
	private string text;		//Der auszugebende Text
	void Start()
	{
		ranRange = Commands.Count;	//Der Zufallsbereich wird gesetzt
		index = Random.Range (0, ranRange);	//Der Index wird ermittelt
	}

	void Update()
	{
		//Solange ein Kommando ausgeben bis 10 Kommandos befolgt wurden
		if (counter < 10) 
		{
			text = Commands[index];
		}
		else
		{
			text = "You won!";
			if(Input.GetKeyDown(KeyCode.Space))
				Application.LoadLevel(Application.loadedLevel);
		}
	}

	//Diese Methode wird in Script "Button.cs" ausgeführt, sie sorgt bei einem Fall einer 
	//erfolgreichen Kommandobefolgung zum Wechsel des Kommandos.
	public void ChangeIndex(int ButtonId)
	{
		if (ButtonId == index) 
		{
			int currentIndex = index;
			counter++;
			while (currentIndex == index) 
			{
				index = Random.Range (0, ranRange);

			}
		}
	}

	//Hier wird der Text ausgegeben
	void OnGUI()
	{
		GUI.Label (new Rect((Screen.width-50)/2,Screen.height/15,50,100),text,guiStyle);
	}
}