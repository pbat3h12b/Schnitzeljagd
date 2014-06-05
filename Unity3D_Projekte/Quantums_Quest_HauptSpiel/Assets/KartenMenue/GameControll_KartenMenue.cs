using UnityEngine;
using System.Collections;

public class GameControll_KartenMenue : MonoBehaviour {

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
        if (GUI.Button(new Rect((float)((screenWidth) - ((50 * scaleWidth))), (float)((screenHeight) - (5 * scaleHeight) * 2), (float)(50 * scaleWidth), (float)(5 * scaleHeight)), "QR-Code Scannen"))
        {
            Application.LoadLevel(6);
        }
    }
}
