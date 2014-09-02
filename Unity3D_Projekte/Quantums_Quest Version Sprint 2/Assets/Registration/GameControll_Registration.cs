using UnityEngine;
using System.Collections;

public class GameControll_Registration : MonoBehaviour {

    [HideInInspector]
	private GameObject gameController;

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

    private string inputUsername = "";
    private string inputPassword = "";
    private string inputPasswordRepetition = "";
    private string falseInput = "";
    //private string buttonRegisterValue = "Registrieren";
    //private string buttonCancelValue = "Abbrechen";

    GUIStyle styleUser = new GUIStyle();
    GUIStyle stylePasswort = new GUIStyle();
    GUIStyle styleRegis = new GUIStyle();
    GUIStyle styleCancel = new GUIStyle();

    private bool isIncorrect = false;

	// Initialisieren
	void Start () {
		gameController = GameObject.Find("GameController");

        styleUser.normal.background = User;
        styleCancel.normal.background = Cancel;
        stylePasswort.normal.background = Passwort;
        styleRegis.normal.background = Regis;
        styleUser.alignment = TextAnchor.MiddleCenter;
        stylePasswort.alignment = TextAnchor.MiddleCenter;
        stylePasswort.fontSize = 35;
        styleUser.fontSize = 35;
        //screenHeight = Screen.height;
        //screenWidth = Screen.width;

        //scaleWidth = screenWidth / 100;
        //scaleHeight = screenHeight / 100;
	}
	
	// Oberfläche konstruieren
    void OnGUI()
    {
        GUI.DrawTexture(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(0, 0, 100, 100)), background);
        inputUsername = GUI.TextField(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 25, 60, 10)), inputUsername, styleUser);
        inputPassword = GUI.PasswordField(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 40, 60, 10)), inputPassword, '*', stylePasswort);
        inputPasswordRepetition = GUI.PasswordField(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 55, 60, 10)), inputPasswordRepetition, '*', stylePasswort);


        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 70, 60, 10)), "", styleRegis))
        {
			if (inputPassword == inputPasswordRepetition && inputPassword != "")
            {
                Response temp = gameController.GetComponent<RESTCommunication>().RegisterNewUser(inputUsername, inputPassword);
                if (temp.Success == true)
                {
                    Application.LoadLevel(0);
                }
                else
                {
                    falseInput = temp.Message;
                }
            }
            else
            {
                isIncorrect = true;
            }
        }
        if (isIncorrect)
        {
            GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 10, 60, 10)), falseInput);
        }
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 85, 60, 10)), "", styleCancel))
        {
            Application.LoadLevel(0);
        }
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), verlauf);
    }
}
