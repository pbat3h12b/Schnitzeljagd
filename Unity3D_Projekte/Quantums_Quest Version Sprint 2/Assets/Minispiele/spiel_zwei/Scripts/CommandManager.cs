using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Dieses Skript wurde von Markus Baars erstellt.

public class CommandManager : MonoBehaviour {

	public List<string> Commands = new List<string>(); //Liste der Anweisungen
	public int index;			// index in der Kommandoliste
	public int counter = 0;		//Counter wie oft eine Anweisung befolgt wurde
	public GUIStyle guiStyle;	//Formatierung des Textes
	public GameObject timeline;	//Zeitlinie für das Spiel
	public float time = 10;		//Die verbliebene Zeit

	private int currentIndex;
	private int ranRange;		//Zufallsbereich des Indexes der Kommandos
	private string text;		//Der auszugebende Text
	private Switch switch1;		//Die Klasse "Switch" wird hier eingebunden;


	void Start()
	{
		Time.timeScale = 1;
		ranRange = Commands.Count;	//Der Zufallsbereich wird gesetzt
		index = Random.Range (0, ranRange);	//Der Index wird ermittelt
		switch1 = GameObject.FindGameObjectWithTag("Switch").GetComponent<Switch>();


		while(index == switch1.id+1)
		{
			index = Random.Range (0, ranRange);
		}
	}

	void Update()
	{
		TimeSet ();
		//Solange ein Kommando ausgeben bis 10 Kommandos befolgt wurden
		if (time > 0) 
		{
			//Solange der Schalter An/Aus ist wird der jeweilige Kommando nicht angezeigt
			while (switch1.switchState && index == switch1.id || !switch1.switchState && index == switch1.id+1)
				index = Random.Range (0, ranRange);

			if (counter < 10) 
			{
				text = Commands [index];
			} 
			else 
			{
				text = "You won!";
				Time.timeScale = 0;
				if (Input.GetKeyDown (KeyCode.Space))
					Application.LoadLevel (Application.loadedLevel);
			}
		}
		else
		{
			Time.timeScale = 0;
			text = "You Lost!";
			if (Input.GetKeyDown (KeyCode.Space))
				Application.LoadLevel (Application.loadedLevel);
		}

	}



	void TimeSet ()
	{
		if (time <= 0) 
		{
			time = 0;
		} 
		else 
		{
			time -= Time.deltaTime;
		}

		timeline.transform.localScale = new Vector3 (time / 2, 0.5f, 1);
	}

	//Diese Methode wird in Script "Button.cs" ausgeführt, sie sorgt bei einem Fall einer 
	//erfolgreichen Kommandobefolgung zum Wechsel des Kommandos.

	public void ChangeButton(int ButtonId)
	{

		if (ButtonId == index) 
		{
			currentIndex = index;
			counter++;
			while (currentIndex == index) 
			{
				index = Random.Range (0, ranRange);
			}
		}
		else
		{
			time -= 0.5f;
		}
	}

	//Diese Methode wird in Script "Script.cs" ausgeführt, sie sorgt bei einem Fall einer
	//erfolgreichen Umschaltung des Schalters zum Wechsel des Kommandos. Desweiteren wird
	//der Zustand des Schalters selbst umgestellt

	public void ChangeSwitch(int switchID)
	{
		if(switchID == index || switchID+1 == index)
		{
			currentIndex = index;
			switch1.switchState = !switch1.switchState;
			counter++;

			while(currentIndex == index)
				index = Random.Range (0, ranRange);
		}
		else
		{
			time -= 0.5f;
		}
	}

	public void ChangeRegulator(int RegID)
	{
		Debug.Log (RegID);
		if (RegID == index) 
		{
			currentIndex = index;
			counter++;

			while (currentIndex == index)
			{
				index = Random.Range (0, ranRange);
			}
		}
		else
		{
			time -= 0.5f;
		}
	}

	//Hier wird der Text ausgegeben
	void OnGUI()
	{
		GUI.Label (new Rect((Screen.width-50)/2,Screen.height/15,50,100),text,guiStyle);
        if (GUI.Button(GameObject.Find("GameController").GetComponent<GUI_Scale>().GetRelativeRect(new Rect(0, 0, 20, 15)), "kill"))
        {
            Application.Quit();
        }

	}
}
