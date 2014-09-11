using UnityEngine;
using System.Collections;

/// <summary>
/// Klasse erstellt von Niclas Hüppmeier
/// </summary>

public class GameControll_MainMenue : MonoBehaviour {
    [HideInInspector]
    //Ein Objekt des GameControllers
    private GameObject gameController;

    //Texturen, werden im Inspector gesetzt
    public Texture2D verlauf;
    public Texture2D background;
    public Texture2D KarteAnzeigen;
    public Texture2D SpieleAnzeigen;

    //GUI Style für zum setzen des Hintergrundes und der Schriftgröße
    GUIStyle styleKarteAnzeigen = new GUIStyle();
    //GUI Style für zum setzen des Hintergrundes und der Schriftgröße
    GUIStyle styleSpieleAnzeigen = new GUIStyle();

    //private int screenWidth;
    //private int screenHeight;
    //private double scaleWidth;
    //private double scaleHeight;
    // Use this for initialization
    void Start()
    {
        //sucht das Gameobjekt mit dem Namen "GameController"
        gameController = GameObject.Find("GameController");

        //Setzt den Hintergrund des buttons
        styleKarteAnzeigen.normal.background = KarteAnzeigen;
        //Setzt den Hintergrund des buttons
        styleSpieleAnzeigen.normal.background = SpieleAnzeigen;

        //screenHeight = Screen.width;
        //screenWidth = Screen.height;

        //scaleWidth = screenWidth / 100;
        //scaleHeight = screenHeight / 100;
        gameController.GetComponent<PlayerInformation>().getUserData();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        //Zeichnet den Hintergrund
        GUI.DrawTexture(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(0, 0, 100, 100)), background);
        //Zeichnet den Button zum KarteAnzeigen
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(15, 40, 30, 15)), "", styleKarteAnzeigen))
        {
            //Lädt das Kartenmenü
            Application.LoadLevel(4);
        }
        //Zeichnet den Button zum Spiele anzeigen
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(55, 40, 30, 15)), "", styleSpieleAnzeigen))
        {
            //Lädt das Spiele Menü
            Application.LoadLevel(3);
        }
        //Zeichnet den Hintergrundverlauf
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), verlauf);
    }
}
