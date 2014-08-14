using UnityEngine;
using System.Collections;

public class GameControll_LogIn : MonoBehaviour {

    public Texture2D name;
    public Texture2D passwort;
    public Texture2D login;
    public Texture2D registrieren;

    [HideInInspector]
	private GameObject gameController;

    //private int screenWidth;
    //private int screenHeight;

    //private double scaleWidth;
    //private double scaleHeight;

    private string inputUsername = "";
    private string inputPassword = "";
	private string buttonLoginValue = "Login";
	private string buttonRegisterValue = "Registrieren";

    private GUIStyle styleLogIn = new GUIStyle();
    private GUIStyle styleRegistration = new GUIStyle();
    private GUIStyle stylePassword = new GUIStyle();
    private GUIStyle styleLogInName = new GUIStyle();
    
	bool isIncorrect = false;

	// Initialisierung
	void Start ()
	{
        stylePassword.normal.background = passwort;
        stylePassword.fontSize = 35;
        styleLogInName.normal.background = name;
        styleLogInName.fontSize = 25;
        styleLogIn.normal.background = login;
        styleRegistration.normal.background = registrieren;


		gameController = GameObject.Find("GameController");


        //screenHeight = Screen.height;
        //screenWidth = Screen.width;

        //scaleWidth = screenWidth / 100;
        //scaleHeight = screenHeight / 100;
	}

	// Oberfläche konstruieren
    void OnGUI()
    {

        inputUsername = GUI.TextField(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(20,25,60,10)), inputUsername, styleLogInName);
        inputPassword = GUI.PasswordField(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(20, 40, 60, 10)), inputPassword, '*', stylePassword);

        if (GUI.Button(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(20, 55, 60, 10)), "", styleLogIn))
        {
			if (gameController.GetComponent<RESTCommunication>().LoginUser(inputUsername,inputPassword) == true )
            {
				gameController.GetComponent<PlayerInformation>().logIn(inputUsername);
                Screen.orientation = ScreenOrientation.Landscape;
                Application.LoadLevel(1);
			}
			else
			{
				isIncorrect = true;
			}
        }

		if (isIncorrect)
		{
            GUI.Label(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(20, 15, 60, 10)), "Falsche Eingabe");
		}

        if (GUI.Button(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(20, 70, 60, 10)), "", styleRegistration))
        {
            Application.LoadLevel(2);
        }
    }
}
