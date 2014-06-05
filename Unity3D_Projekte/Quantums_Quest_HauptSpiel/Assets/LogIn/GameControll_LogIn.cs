using UnityEngine;
using System.Collections;

public class GameControll_LogIn : MonoBehaviour {

    [HideInInspector]
    private int screenWidth;
    private int screenHeight;
    private double scaleWidth;
    private double scaleHeight;
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
        GUI.TextField(new Rect((float)((screenWidth / 2) - ((50 * scaleWidth) / 2)), (float)((screenHeight / 2) - (5 * scaleHeight)), (float)(50 * scaleWidth), (float)(5 * scaleHeight)), "UserName");
        GUI.TextField(new Rect((float)((screenWidth / 2) - ((50 * scaleWidth) / 2)), (float)((screenHeight / 2) + (5 * scaleHeight)), (float)(50 * scaleWidth), (float)(5 * scaleHeight)), "PassWord");

        if (GUI.Button(new Rect((float)((screenWidth / 2) - ((50 * scaleWidth) / 2)), (float)((screenHeight / 2) + (5 * scaleHeight) * 3), (float)(50 * scaleWidth), (float)(5 * scaleHeight)), "LogIn"))
        {
            Screen.orientation = ScreenOrientation.Landscape;
            Application.LoadLevel(1);
        }

        if (GUI.Button(new Rect((float)((screenWidth / 2) - ((50 * scaleWidth) / 2)), (float)((screenHeight / 2) + (5 * scaleHeight) * 5), (float)(50 * scaleWidth), (float)(5 * scaleHeight)), "Registrieren"))
        {
            Application.LoadLevel(2);
        }
    }
}
