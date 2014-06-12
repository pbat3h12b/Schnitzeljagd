using UnityEngine;
using System.Collections;

public class GameControll_Registration : MonoBehaviour {

    [HideInInspector]
    private int screenWidth;
    private int screenHeight;
    private double scaleWidth;
    private double scaleHeight;
    private string userName = "";
    private string password = "";
    private string passwordAgain = "";
    private bool isIncorrect = false;
	// Use this for initialization
	void Start () {
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        scaleWidth = screenWidth / 100;
        scaleHeight = screenHeight / 100;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        userName = GUI.TextField(new Rect((float)((screenWidth / 2) - ((50 * scaleWidth) / 2)), (float)((screenHeight / 2) - (5 * scaleHeight)*3), (float)(50 * scaleWidth), (float)(5 * scaleHeight)), userName);
        password = GUI.PasswordField(new Rect((float)((screenWidth / 2) - ((50 * scaleWidth) / 2)), (float)((screenHeight / 2) - (5 * scaleHeight)), (float)(50 * scaleWidth), (float)(5 * scaleHeight)), password,'*');
        passwordAgain = GUI.PasswordField(new Rect((float)((screenWidth / 2) - ((50 * scaleWidth) / 2)), (float)((screenHeight / 2) + (5 * scaleHeight)), (float)(50 * scaleWidth), (float)(5 * scaleHeight)), passwordAgain,'*');

        if (GUI.Button(new Rect((float)((screenWidth / 2) - ((50 * scaleWidth) / 2)), (float)((screenHeight / 2) + (5 * scaleHeight) * 3), (float)(50 * scaleWidth), (float)(5 * scaleHeight)), "Registrieren"))
        {
            if (password == passwordAgain && password != "")
            {
                if (GameObject.Find("GameController").GetComponent<beispiel_api>().newRegistrierung(userName, password, "beispielMail") == true)
                {
                    Application.LoadLevel(0);
                }
            }
            else
            {
                isIncorrect = true;
            }
        }
        if (isIncorrect == true)
        {
            GUI.Label(new Rect((float)((screenWidth / 2) - ((50 * scaleWidth) / 2)), (float)((screenHeight / 2) - (5 * scaleHeight) * 4), (float)(50 * scaleWidth), (float)(5 * scaleHeight)), "Falsche Eingabe");
        }
        if (GUI.Button(new Rect((float)((screenWidth / 2) - ((50 * scaleWidth) / 2)), (float)((screenHeight / 2) + (5 * scaleHeight) * 5), (float)(50 * scaleWidth), (float)(5 * scaleHeight)), "Abbrechen"))
        {
            Application.LoadLevel(0);
        }
    }
}
