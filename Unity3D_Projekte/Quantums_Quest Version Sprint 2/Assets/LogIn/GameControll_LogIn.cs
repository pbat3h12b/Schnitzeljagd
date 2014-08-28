using UnityEngine;
using System.Collections;

public class GameControll_LogIn : MonoBehaviour {

    public Texture2D name;
    public Texture2D passwort;
    public Texture2D login;
    public Texture2D registrieren;
    public Texture2D verlauf;
    public Texture2D background;

    [HideInInspector]
	private GameObject gameController;

    private string inputUsername = "";
    private string inputPassword = "";
	private string buttonLoginValue = "Login";
	private string buttonRegisterValue = "Registrieren";
    private string falseInput = "";

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
        stylePassword.alignment = TextAnchor.MiddleCenter;
        styleLogInName.normal.background = name;
        styleLogInName.fontSize = 25;
        styleLogInName.alignment = TextAnchor.MiddleCenter;
        styleLogIn.normal.background = login;
        styleRegistration.normal.background = registrieren;

		gameController = GameObject.Find("GameController");

	}

	// Oberfläche konstruieren
    void OnGUI()
    {
        GUI.DrawTexture(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(0, 0, 100, 100)), background);

        inputUsername = GUI.TextField(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 25, 60, 10)), inputUsername, styleLogInName);
        inputPassword = GUI.PasswordField(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 40, 60, 10)), inputPassword, '*', stylePassword);

        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 55, 60, 10)), "", styleLogIn))
        {
            Response temp = gameController.GetComponent<RESTCommunication>().LoginUser(inputUsername,inputPassword);
			if (temp.Success == true )
            {
				gameController.GetComponent<PlayerInformation>().logIn(inputUsername);
                Screen.orientation = ScreenOrientation.Landscape;
                Application.LoadLevel(1);
			}
			else
			{
				isIncorrect = true;
                falseInput = temp.Message;
			}
        }

		if (isIncorrect)
		{
            GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 15, 60, 10)), falseInput);
		}

        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(20, 70, 60, 10)), "", styleRegistration))
        {
            Application.LoadLevel(2);
        }

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), verlauf);
    }
}
