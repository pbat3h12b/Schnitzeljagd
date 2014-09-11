using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

    //GUISkin zur Änderung des Aussehens der GUI
	public GUISkin skin;
    //Rect für die Maße des Hauptanzeigefensters
    private Rect mainWindow;
    //Rect für die Maße des Standardbuttons
    private Rect defaultButton;
    //bool die besagt ob das Hilfsmenü angezeigt wird oder nicht
    private bool helpMenu = false;
    //1 Prozent der Hauptfensterhöhe
    private float mainWindowHeightPercent;
    //1 Prozent der Hauptfensterbreite
    private float mainWindowWidthPercent;

	// Use this for initialization
	void Start () 
    {
        //Maße des Hauptfensters festlegen
        mainWindow = new Rect(0, 0, Screen.width / 3, Screen.height / 1.5f);

        //Prozente des Hauptfensters ausrechnen
        mainWindowHeightPercent = (int)mainWindow.height / 100;
        mainWindowWidthPercent = (int)mainWindow.width / 100;

        //Größe für die SChrift der Button festelgen
        skin.button.fontSize = 20;

        //Maße des Standardbuttons festlegen
        defaultButton = new Rect(0, 0, 350, 45);
	}
	
	

    //Verwaltung der GUI
    void OnGUI()
    {
        //GUI.skin auf den personalisierten GUISkin festlegen     
        GUI.skin = skin;

        //Anzeige des Hauptfensters
        mainWindow = GUI.Window(0, new Rect((Screen.width / 2)- mainWindow.width / 2, (Screen.height / 2) - mainWindow.height / 2, mainWindow.width, mainWindow.height) , MainWindow, "Menu");

        //Abfrage ob das Hilfemenü angezeigt werden soll
        if (helpMenu)
        {
            //Anzeige des Optionsmenü
            mainWindow = GUI.Window(0, new Rect((Screen.width / 2) - mainWindow.width / 2, (Screen.height / 2) - mainWindow.height / 2, mainWindow.width, mainWindow.height), HelpWindow, "Hilfe");
        }
        
    }

    //Inhalt des Hauptfensters
    void MainWindow(int windowID)
    {
        //Button "Spiel Starten" bei Interaktion des Benutzers wird das Spiel geladen
        if (GUI.Button(new Rect((mainWindow.width / 2) - defaultButton.width / 2,  mainWindowHeightPercent * 30, defaultButton.width, defaultButton.height),"Spiel starten"))
        {
            Application.LoadLevel("Sprint1");
        }
        
        //Button "Hilfe" bei Interaktion des Benutzers wird ein Hilfetext angezeigt
        if (GUI.Button(new Rect((mainWindow.width / 2) - defaultButton.width / 2, mainWindowHeightPercent * 40, defaultButton.width, defaultButton.height), "Hilfe"))
        {
            helpMenu = true;
        }

        //Button "Ende" bei Interaktion des Benutzers wird das Spiel geschlossen
        if (GUI.Button(new Rect((mainWindow.width / 2) - defaultButton.width / 2, mainWindowHeightPercent * 50, defaultButton.width, defaultButton.height), "Ende"))
        {
            //Funktion Hüppi
            GameObject.Find("GameController").GetComponent<PlayerInformation>().newScore("Fluss", 10000);
            if (GameObject.Find("GameController").GetComponent<PlayerInformation>().getGames()[4] == true &&
                GameObject.Find("GameController").GetComponent<PlayerInformation>().getPuzzels()[4] == false)
            {
                GameObject.Find("GameController").GetComponent<PlayerInformation>().markPuzzel();
            }
            Application.LoadLevel(3);
        }
    }

    //Inhalt des Hilfemenüs
    void HelpWindow(int WindowID)
    {
        //Anzeige und Inhat des Hilfetextes
        GUI.TextArea(new Rect((mainWindow.width / 2) - defaultButton.width / 2, 100, defaultButton.width, 175),
            "Ziel des Spiel ist es Teile aus dem Fluss zu fischen");

        //Button "Zurueck" bei Interaktion des Benutzers wird das Hauptfenster angezeigt
        if (GUI.Button(new Rect((mainWindow.width / 2) - defaultButton.width / 2, 300, defaultButton.width, defaultButton.height), "Zurueck"))
        {
            helpMenu = false;
        }
    }
}
    



