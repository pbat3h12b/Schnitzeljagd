using UnityEngine;
using System.Collections;

public class GameControll_LogIn : MonoBehaviour
{

    //Texturen, werden im Inspector gesetzt
    public Texture2D name;
    public Texture2D passwort;
    public Texture2D login;
    public Texture2D registrieren;
    public Texture2D verlauf;
    public Texture2D background;

    [HideInInspector]
    //Ein Objekt des GameControllers
    private GameObject gameController;

    //Variable zur speicherung des Usernames
    private string inputUsername = "";
    //Variable zur speicherung des passworts
    private string inputPassword = "";
    //private string buttonLoginValue = "Login";
    //private string buttonRegisterValue = "Registrieren";
    //Fehlermeldungs Text
    private string falseInput = "";

    //GUI Style für zum setzen des Hintergrundes und der Schriftgröße
    private GUIStyle styleLogIn = new GUIStyle();
    //GUI Style für zum setzen des Hintergrundes und der Schriftgröße
    private GUIStyle styleRegistration = new GUIStyle();
    //GUI Style für zum setzen des Hintergrundes und der Schriftgröße
    private GUIStyle stylePassword = new GUIStyle();
    //GUI Style für zum setzen des Hintergrundes und der Schriftgröße
    private GUIStyle styleLogInName = new GUIStyle();

    //Speicherung ob der Login erfolgreich war oder nicht
    bool isIncorrect = false;

    // Initialisierung
    void Start()
    {

        //Setzt den Hintergrund des buttons
        stylePassword.normal.background = passwort;
        //setzt die Schriftgröße
        stylePassword.fontSize = 35;
        //setzt die Schrift positionierung
        stylePassword.alignment = TextAnchor.MiddleCenter;
        //Setzt den Hintergrund des buttons
        styleLogInName.normal.background = name;
        //setzt die Schriftgröße
        styleLogInName.fontSize = 25;
        //setzt die Schrift positionierung
        styleLogInName.alignment = TextAnchor.MiddleCenter;
        //Setzt den Hintergrund des buttons
        styleLogIn.normal.background = login;
        //Setzt den Hintergrund des buttons
        styleRegistration.normal.background = registrieren;


        //sucht das Gameobjekt mit dem Namen "GameController"
        gameController = GameObject.Find("GameController");

    }

    // Oberfläche konstruieren
    void OnGUI()
    {

        //Hintergrundmap zeichnen
        GUI.DrawTexture(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(0, 0, 100, 100)), background);

        //Feld zum eingeben des Benutzernamens, die eingabe wird direkt in die Variable gepackt
        inputUsername = GUI.TextField(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 25, 60, 10)), inputUsername, styleLogInName);
        //Feld zum eingeben des Passwortes, die eingabe wird direkt in die Variable gepackt
        inputPassword = GUI.PasswordField(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 40, 60, 10)), inputPassword, '*', stylePassword);

        // Abfrage ob der Button zum Login gedrückt wird
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 55, 60, 10)), "", styleLogIn))
        {
            //Variable zum auswerten der Antwort vom Server
            Response temp = gameController.GetComponent<RESTCommunication>().LoginUser(inputUsername, inputPassword);
            // Server gibt true zurück heißt alles ist gut gegangen
            if (temp.Success == true)
            {
                //Speichert den Usernamen im PlayerInformation Script
                gameController.GetComponent<PlayerInformation>().logIn(inputUsername);
                //Ändert die ausrichtung von Hochkant auf vertikal
                Screen.orientation = ScreenOrientation.Landscape;
                //Läd die nächste Szene
                Application.LoadLevel(1);
            }
            else
            {
                //setzt die Variable isIncorrect auf true
                isIncorrect = true;
                //Übergibt die Fehlermeldung vom Server in eine Variable
                falseInput = temp.Message;
            }
        }

        //Abfroge ob ein Fehler bei der Anmeldung unterlaufen ist
        if (isIncorrect)
        {
            //Gibt Fehler in einem Label aus
            GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 15, 60, 10)), falseInput);
        }

        //Abfrage ob der Button zur Registration gedrückt wurde
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 70, 60, 10)), "", styleRegistration))
        {
            //Lädt die Szene zur Registration
            Application.LoadLevel(2);
        }

        //Zeichnet einen Verlauf über die komplette Szene
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), verlauf);
    }
}
