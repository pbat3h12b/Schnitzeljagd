using UnityEngine;
using System.Collections;

public class GameControll_LogIn : MonoBehaviour {

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

	bool isIncorrect = false;

	// Initialisierung
	void Start ()
	{
		gameController = GameObject.Find("GameController");

        screenHeight = Screen.height;
        screenWidth = Screen.width;

        scaleWidth = screenWidth / 100;
        scaleHeight = screenHeight / 100;
	}

	// Oberfläche konstruieren
    void OnGUI()
    {
		inputUsername = GUI.TextField(new Rect((float)((screenWidth / 2) - ((60 * scaleWidth) / 2)), 
		                                       (float)((screenHeight / 3) - (5 * scaleHeight)), 
		                                       (float)(60 * scaleWidth), 
		                                       (float)(8 * scaleHeight)), inputUsername);
		inputPassword = GUI.PasswordField(new Rect((float)((screenWidth / 2) - ((60 * scaleWidth) / 2)), 
		                                           (float)((screenHeight / 3) + (5 * scaleHeight)), 
		                                           (float)(60 * scaleWidth), 
		                                           (float)(8 * scaleHeight)), inputPassword, '*');

        if (GUI.Button(new Rect((float)((screenWidth / 2) - ((60 * scaleWidth) / 2)), 
		                        (float)((screenHeight / 3) + (5 * scaleHeight) * 3), 
		                        (float)(60 * scaleWidth), 
		                        (float)(8 * scaleHeight)), buttonLoginValue))
        {
			if (gameController.GetComponent<beispiel_api>().logIn(inputUsername, inputPassword) == true)
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
			GUI.Label(new Rect((float)((screenWidth / 2) - ((60 * scaleWidth) / 2)), 
			                   (float)((screenHeight / 3) - (5 * scaleHeight) * 2), 
			                   (float)(60 * scaleWidth), 
			                   (float)(8 * scaleHeight)), "Falsche Eingabe");
		}

        if (GUI.Button(new Rect((float)((screenWidth / 2) - ((60 * scaleWidth) / 2)), 
		                        (float)((screenHeight / 3) + (5 * scaleHeight) * 5), 
		                        (float)(60 * scaleWidth), 
		                        (float)(8 * scaleHeight)), buttonRegisterValue))
        {
            Application.LoadLevel(2);
        }
    }
}
