using UnityEngine;
using System.Collections;

public class GameControll_Registration : MonoBehaviour {

    [HideInInspector]
	private GameObject gameController;

    public Texture2D User;
    public Texture2D Passwort;
    public Texture2D Regis;
    public Texture2D Cancel;

    //private int screenWidth;
    //private int screenHeight;

    //private double scaleWidth;
    //private double scaleHeight;

    private string inputUsername = "";
    private string inputPassword = "";
    private string inputPasswordRepetition = "";
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
        //screenHeight = Screen.height;
        //screenWidth = Screen.width;

        //scaleWidth = screenWidth / 100;
        //scaleHeight = screenHeight / 100;
	}
	
	// Oberfläche konstruieren
    void OnGUI()
    {
        inputUsername = GUI.TextField(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(20, 25, 60, 10)), "", styleUser);
        inputPassword = GUI.PasswordField(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(20, 40, 60, 10)), "", '*',stylePasswort);
        inputPasswordRepetition = GUI.PasswordField(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(20, 55, 60, 10)), "", '*',stylePasswort);

        if (GUI.Button(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(20, 70, 60, 10)), "",styleRegis))
        {
			if (inputPassword == inputPasswordRepetition && inputPassword != "")
            {
				if (gameController.GetComponent<RESTCommunication>().RegisterNewUser(inputUsername,inputPassword).Success == true )
                {
                    Application.LoadLevel(0);
                }
            }
            else
            {
                isIncorrect = true;
            }
        }
        if (isIncorrect)
        {
            GUI.Label(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(20, 10, 60, 10)), "Falsche Eingabe");
        }
        if (GUI.Button(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(20, 85, 60, 10)), "",styleCancel))
        {
            Application.LoadLevel(0);
        }
    }
}
