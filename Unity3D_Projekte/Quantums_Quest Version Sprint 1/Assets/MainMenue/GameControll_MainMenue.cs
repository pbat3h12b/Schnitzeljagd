using UnityEngine;
using System.Collections;

public class GameControll_MainMenue : MonoBehaviour {
    [HideInInspector]
    private int screenWidth;
    private int screenHeight;
    private double scaleWidth;
    private double scaleHeight;
    // Use this for initialization
    void Start()
    {
        screenHeight = Screen.width;
        screenWidth = Screen.height;

        scaleWidth = screenWidth / 100;
        scaleHeight = screenHeight / 100;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect((float)((screenWidth / 2) - ((50 * scaleWidth) / 2)), (float)((screenHeight / 2) + (5 * scaleHeight)), (float)(50 * scaleWidth), (float)(5 * scaleHeight)),"Karte Anzeigen"))
        {
            Application.LoadLevel(4);
        }

        if (GUI.Button(new Rect((float)((screenWidth / 2) - ((50 * scaleWidth) / 2)), (float)((screenHeight / 2) - (5 * scaleHeight)), (float)(50 * scaleWidth), (float)(5 * scaleHeight)), "Spiele Anzeigen"))
        {
            Application.LoadLevel(3);
        }
    }
}
