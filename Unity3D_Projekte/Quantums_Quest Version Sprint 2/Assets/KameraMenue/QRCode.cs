using UnityEngine;
using System.Collections;

/// <summary>
/// Klasse erstellt von Niclas Hüppmeier
/// </summary>
public class QRCode : MonoBehaviour
{

    [HideInInspector]
    //Ein Objekt des GameControllers
    private GameObject gameController;

    //Texturen, werden im Inspector gesetzt
    public Texture2D Absenden;
    public Texture2D Zurueck;
    public Texture2D Weiter;
    public Texture2D verlauf;
    public Texture2D background;

    //GUI Style für zum setzen des Hintergrundes und der Schriftgröße
    GUIStyle styleWeiter = new GUIStyle();
    //GUI Style für zum setzen des Hintergrundes und der Schriftgröße
    GUIStyle styleBack = new GUIStyle();
    //GUI Style für zum setzen des Hintergrundes und der Schriftgröße
    GUIStyle styleAbsenden = new GUIStyle();

    //Speicherung des codes
    string qrcode = "";

    // Use this for initialization
    void Start()
    {
        //sucht das Gameobjekt mit dem Namen "GameController"
        gameController = GameObject.Find("GameController");

        //Setzt den Hintergrund des buttons
        styleAbsenden.normal.background = Absenden;
        //Setzt den Hintergrund des buttons
        styleWeiter.normal.background = Weiter;
        //Setzt den Hintergrund des buttons
        styleBack.normal.background = Zurueck;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        //Hintergrundmap zeichnen
        GUI.DrawTexture(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(0, 0, 100, 100)), background);
        //Feld zum eingeben des qrcodes, die eingabe wird direkt in die Variable gepackt
        qrcode = GUI.TextField(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(30, 35, 20, 10)), qrcode);
        //Label
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(35, 30, 20, 10)), "Bitte Code eingeben");
        // Abfrage ob der Absenden Button gedrückt wurde
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(53, 35, 20, 10)), "", styleAbsenden))
        {
            //Überprüft ob der qr-Code correct ist
            if (gameController.GetComponent<PlayerInformation>().checkCach(qrcode))
            {
                //Debug.Log("yes got it");
                //speichert den cach in der PlayerInformation
                gameController.GetComponent<PlayerInformation>().setCachSecret(qrcode);
                //Debug.Log(gameController.GetComponent<PlayerInformation>().getSecret());
                //Lädt die nächste Szene
                Application.LoadLevel(6);
            }
        }
        //Zeichnet einen Verlauf über die komplette Szene
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), verlauf);

        // Wird ausgeführt, wenn Zurück-Button oben links oder der Zurück-Button von Endgeräten mit Android gedrückt wurde
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(1, 1, 20, 10)), "", styleBack) || Input.GetKeyDown(KeyCode.Escape))
        {
            //Springt eine Szene zurück
            Application.LoadLevel(1);
        }
    }
}
