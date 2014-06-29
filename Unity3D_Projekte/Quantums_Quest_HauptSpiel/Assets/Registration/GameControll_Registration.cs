using UnityEngine;
using System.Collections;

public class GameControll_Registration : MonoBehaviour {

    [HideInInspector]
	private GameObject gameController;

    private int screenWidth;
    private int screenHeight;

    private double scaleWidth;
    private double scaleHeight;

	private string inputUsername = "";
	private string inputPassword = "";
	private string inputPasswordRepetition = "";
	private string buttonRegisterValue = "Registrieren";
	private string buttonCancelValue = "Abbrechen";

    private bool isIncorrect = false;

	// Initialisieren
	void Start () {
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
		                                  	   (float)((screenHeight / 3) - (5 * scaleHeight) * 3), 
		                                       (float)(60 * scaleWidth), 
		                                       (float)(8 * scaleHeight)), inputUsername);
		inputPassword = GUI.PasswordField(new Rect((float)((screenWidth / 2) - ((60 * scaleWidth) / 2)), 
		                                    	   (float)((screenHeight / 3) - (5 * scaleHeight)), 
		                                           (float)(60 * scaleWidth), 
		                                           (float)(8 * scaleHeight)), inputPassword,'*');
		inputPasswordRepetition = GUI.PasswordField(new Rect((float)((screenWidth / 2) - ((60 * scaleWidth) / 2)), 
		                                          			 (float)((screenHeight / 3) + (5 * scaleHeight)), 
		                                                     (float)(60 * scaleWidth), 
		                                                     (float)(8 * scaleHeight)), inputPasswordRepetition,'*');

        if (GUI.Button(new Rect((float)((screenWidth / 2) - ((60 * scaleWidth) / 2)), 
		                        (float)((screenHeight / 3) + (5 * scaleHeight) * 3), 
		                        (float)(60 * scaleWidth), 
		                        (float)(8 * scaleHeight)), buttonRegisterValue))
        {
			if (inputPassword == inputPasswordRepetition && inputPassword != "")
            {
				if (gameController.GetComponent<beispiel_api>().newRegistrierung(inputUsername, inputPassword, "beispielMail") == true)
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
            GUI.Label(new Rect((float)((screenWidth / 2) - ((60 * scaleWidth) / 2)), 
			                   (float)((screenHeight / 3) - (5 * scaleHeight) * 4), 
			                   (float)(60 * scaleWidth), 
			                   (float)(8 * scaleHeight)), "Falsche Eingabe");
        }
        if (GUI.Button(new Rect((float)((screenWidth / 2) - ((60 * scaleWidth) / 2)), 
		                        (float)((screenHeight / 3) + (5 * scaleHeight) * 5), 
		                        (float)(60 * scaleWidth), 
		                        (float)(8 * scaleHeight)), buttonCancelValue))
        {
            Application.LoadLevel(0);
        }
    }
}
