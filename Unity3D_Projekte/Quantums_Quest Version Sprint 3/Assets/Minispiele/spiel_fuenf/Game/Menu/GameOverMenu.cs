using UnityEngine;
using System.Collections;

public class GameOverMenu : MonoBehaviour
{
    //GUISkin zur Änderung des Aussehens der GUI
    public GUISkin skin;

    //Rect für die Maße des Hauptanzeigefensters
    private Rect mainWindow;

    //Rect für die Maße des Standardbuttons
    private Rect defaultButton;

    //1 Prozent der Hauptfensterhöhe
    private int mainWindowHeightPercent;

    //1 Prozent der Hauptfensterbreite
    private int mainWindowWidthPercent;
    // Use this for initialization
    void Start()
    {
        //Größe der Schrift des Hauptfensters festlegen
        skin.window.fontSize = 35;
     
        //Maße des Hauptfensters festlegen
        mainWindow = new Rect(0, 0, Screen.width / 3, Screen.height / 1.5f);

        //Prozente des Hauptfensters ausrechnen
        mainWindowHeightPercent = (int)mainWindow.height / 100;
        mainWindowWidthPercent = (int)mainWindow.width / 100;

        //Maße des Standardbuttons festlegen
        defaultButton = new Rect(0, 0, 275 * mainWindowWidthPercent, 45 * mainWindowHeightPercent);
        //defaultButton = new Rect(0, 0, 350, 45);
    }

    //Verwaltung der GUI
    void OnGUI()
    {
        //GUI.skin auf den personalisierten GUISkin festlegen 
        GUI.skin = skin;

        //Anzeige des Hauptfensters
        mainWindow = GUI.Window(0, new Rect((Screen.width / 2) - mainWindow.width / 2, (Screen.height / 2) - mainWindow.height / 2, mainWindow.width, mainWindow.height), MainWindow, "Game Over");

        

    }

    //Inhalt des Hauptfensters
    void MainWindow(int windowID)
    {
        //Button "Erneut Spielen" bei Interaktion des Benutzers wird das Spiel neu gestartet
        if (GUI.Button(new Rect((mainWindow.width / 2) - defaultButton.width / 2, mainWindowHeightPercent * 30, defaultButton.width, defaultButton.height), "Erneut spielen"))
        {
            Time.timeScale = 1;
            Application.LoadLevel("Sprint1");
        }

        //Button "Ende" bei Interaktion des Benutzers wird das Spiel geschlossen
        if (GUI.Button(new Rect((mainWindow.width / 2) - defaultButton.width / 2, mainWindowHeightPercent * 40, defaultButton.width, defaultButton.height), "Ende"))
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

    
}