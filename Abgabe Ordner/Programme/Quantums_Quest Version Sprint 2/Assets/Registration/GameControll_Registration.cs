using UnityEngine;
using System.Collections;

/// <summary>
/// Klasse erstell von Niclas Hüppmeier
/// </summary>

public class GameControll_Registration : MonoBehaviour {

    [HideInInspector]
    //Ein Objekt des GameControllers
	private GameObject gameController;

    //Texturen, werden im Inspector gesetzt
    public Texture2D User;
    public Texture2D Passwort;
    public Texture2D Regis;
    public Texture2D Cancel;
    public Texture2D verlauf;
    public Texture2D background;

    //private int screenWidth;
    //private int screenHeight;

    //private double scaleWidth;
    //private double scaleHeight;

    //Variable zum speichern des Usernames
    private string inputUsername = "";
    //Variable zum speichern des Passwortes
    private string inputPassword = "";
    //Variable zum speichern des Passwortes
    private string inputPasswordRepetition = "";
    //Variable zum Ausgeben einer Fehlermeldung
    private string falseInput = "";
    //private string buttonRegisterValue = "Registrieren";
    //private string buttonCancelValue = "Abbrechen";

    //GUI Style für zum setzen des Hintergrundes und der Schriftgröße
    GUIStyle styleUser = new GUIStyle();
    //GUI Style für zum setzen des Hintergrundes und der Schriftgröße
    GUIStyle stylePasswort = new GUIStyle();
    //GUI Style für zum setzen des Hintergrundes und der Schriftgröße
    GUIStyle styleRegis = new GUIStyle();
    //GUI Style für zum setzen des Hintergrundes und der Schriftgröße
    GUIStyle styleCancel = new GUIStyle();

    //Variable ob eine Falsche Eingabe gemacht wurde
    private bool isIncorrect = false;

	// Initialisieren
	void Start () {
		gameController = GameObject.Find("GameController");

        //Setzt den Hintergrund des buttons
        styleUser.normal.background = User;
        //Setzt den Hintergrund des buttons
        styleCancel.normal.background = Cancel;
        //Setzt den Hintergrund des buttons
        stylePasswort.normal.background = Passwort;
        //Setzt den Hintergrund des buttons
        styleRegis.normal.background = Regis;
        //Setzt die Text Positionierung auf mittig 
        styleUser.alignment = TextAnchor.MiddleCenter;
        //Setzt die Text Positionierung auf mittig 
        stylePasswort.alignment = TextAnchor.MiddleCenter;
        //setzt die Schriftgröße
        stylePasswort.fontSize = 35;
        //setzt die Schriftgröße
        styleUser.fontSize = 35;
        //screenHeight = Screen.height;
        //screenWidth = Screen.width;

        //scaleWidth = screenWidth / 100;
        //scaleHeight = screenHeight / 100;
	}
	
	// Oberfläche konstruieren
    void OnGUI()
    {
        //Zeichnen des Hintergrundes
        GUI.DrawTexture(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(0, 0, 100, 100)), background);
        //Eingabefeld für den Usernamen
        inputUsername = GUI.TextField(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 25, 60, 10)), inputUsername, styleUser);
        //Eingabefeld für das Passwort
        inputPassword = GUI.PasswordField(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 40, 60, 10)), inputPassword, '*', stylePasswort);
        //Eingabefeld für das wiederholte Passwort
        inputPasswordRepetition = GUI.PasswordField(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 55, 60, 10)), inputPasswordRepetition, '*', stylePasswort);

        //Button für die Registration
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 70, 60, 10)), "", styleRegis))
        {
            //Überprüfung ob die eingabe stimmt
			if (inputPassword == inputPasswordRepetition && inputPassword != "")
            {
                //API antwort und User registration
                Response temp = gameController.GetComponent<RESTCommunication>().RegisterNewUser(inputUsername, inputPassword);
                //Api gibt true zurück
                if (temp.Success == true)
                {
                    //Nutzer wird zum Einlog Bildschirm geworfen
                    Application.LoadLevel(0);
                }
                else
                {
                    //Fehlermeldung wird gespeichert
                    falseInput = temp.Message;
                    //Ein Fehler ist geschehen
                    isIncorrect = true;
                }
            }
            else
            {
                //Ein Fehler ist geschehen
                isIncorrect = true;
            }
        }

        //Fehler bei der Registration
        if (isIncorrect)
        {
            //Fehlermeldung ausgeben
            GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 10, 60, 10)), falseInput);
        }
        //Butten zum Abbrechen
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 85, 60, 10)), "", styleCancel))
        {
            //Nutzer wird zum Einlog Bildschirm geworfen
            Application.LoadLevel(0);
        }
        //Zeichnet den Verlauf im Hintergrund
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), verlauf);
    }
}
