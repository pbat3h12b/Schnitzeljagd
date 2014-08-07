using UnityEngine;
using System.Collections;

public class GameControll_LogIn : MonoBehaviour {

    public Texture2D name;
    public Texture2D passwort;
    public Texture2D login;
    public Texture2D registrieren;

    [HideInInspector]
	private GameObject gameController;

    private int screenWidth;
    private int screenHeight;

    private double scaleWidth;
    private double scaleHeight;

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
        styleLogInName.fontSize = 35;
        styleLogIn.normal.background = login;
        styleRegistration.normal.background = registrieren;


		gameController = GameObject.Find("GameController");

        screenHeight = Screen.height;
        screenWidth = Screen.width;

        scaleWidth = screenWidth / 100;
        scaleHeight = screenHeight / 100;
	}

	// Oberfläche konstruieren
    void OnGUI()
    {
        
        inputUsername = GUI.TextField(new Rect((float)((screenWidth / 2) - ((40 * scaleWidth) / 2)),
                                               (float)((screenHeight / 3) - (5 * scaleHeight)),
                                               (float)(20 * scaleWidth) * 2,
                                               (float)(7 * scaleHeight)),inputUsername, styleLogInName);
		inputPassword = GUI.PasswordField(new Rect((float)((screenWidth / 2) - ((40 * scaleWidth) / 2)), 
		                                           (float)((screenHeight / 3) + (5 * scaleHeight)), 
		                                           (float)(20 * scaleWidth)*2, 
		                                           (float)(7 * scaleHeight)), inputPassword, '*', stylePassword);

        if (GUI.Button(new Rect((float)((screenWidth / 2) - ((40 * scaleWidth) / 2)), 
		                        (float)((screenHeight / 3) + (5 * scaleHeight) * 3), 
		                        (float)(20 * scaleWidth)*2, 
		                        (float)(7 * scaleHeight)), "",styleLogIn))
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
			GUI.Label(new Rect((float)((screenWidth / 2) - ((40 * scaleWidth) / 2)), 
			                   (float)((screenHeight / 3) - (5 * scaleHeight) * 2), 
			                   (float)(60 * scaleWidth), 
			                   (float)(8 * scaleHeight)), "Falsche Eingabe");
		}

        if (GUI.Button(new Rect((float)((screenWidth / 2) - ((40 * scaleWidth) / 2)), 
		                        (float)((screenHeight / 3) + (5 * scaleHeight) * 5),
                                (float)(20 * scaleWidth) * 2,
                                (float)(7 * scaleHeight)), "", styleRegistration))
        {
            Application.LoadLevel(2);
        }
    }
}
