using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Klasse erstell von Niclas Hüppmeier
/// </summary>

public class LogbuchEintrag : MonoBehaviour {

    [HideInInspector]
    //Ein Objekt des GameControllers
    private GameObject gameController;

    //Texturen, werden im Inspector gesetzt
    public Texture2D Weiter;
    public Texture2D verlauf;
    public Texture2D background;

    //GUI Style für zum setzen des Hintergrundes
    GUIStyle styleWeiter = new GUIStyle();

    //Eingabe des Nutzers
	string lgeintrag = "";

	// Use this for initialization
	void Start () {
        //sucht das Gameobjekt mit dem Namen "GameController"
        gameController = GameObject.Find("GameController");
        //Setzt den Hintergrund des buttons
        styleWeiter.normal.background = Weiter;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
        //Hintergrundmap zeichnen
        GUI.DrawTexture(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(0, 0, 100, 100)), background);
        // Zeichnet eine Box um alle weiteren GUI Elemente
        GUI.Box(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(10, 10, 80, 80)), "");
        // Zeichnet das eingabefeld und fängt die eingabe ab
        lgeintrag = GUI.TextField(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(15, 15, 50, 70)), lgeintrag);
        // Gibt eine Nachricht für den Nutzer aus
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(70, 15, 15, 10)), "Hier können Sie eine Nachricht hinterlassen");
        // Zeichnet den Senden Button und prüft ob eine eingabe statt fand
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(67, 30, 20, 10)), "", styleWeiter) && lgeintrag != "")
        {
            //Antwort der API
            Response temp = gameController.GetComponent<RESTCommunication>().MakeLogBookEntry(lgeintrag, 
                            gameController.GetComponent<PlayerInformation>().getSecret());

            //API gibt True zurück
            if (temp.Success == true)
            {
                Debug.Log("yes");
                gameController.GetComponent<PlayerInformation>().markPuzzel();
                Application.LoadLevel(5);
            }
            else
            {
                Debug.Log("no");
                Application.LoadLevel(5);
            }
        }
        //zeichnet den Hintergrund verlauf
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), verlauf);
	}
}
