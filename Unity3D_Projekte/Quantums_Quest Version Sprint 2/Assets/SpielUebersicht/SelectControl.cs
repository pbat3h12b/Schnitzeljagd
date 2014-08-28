using UnityEngine;
using System.Collections;

public class SelectControl : MonoBehaviour {

    private GameObject gameController;
	public Texture2D texture1;
	public Texture2D texture2;
	public Texture2D texture3;
	public Texture2D texture4;
	public Texture2D texture5;
    public Texture2D playButton;
    public Texture2D verlauf;
    public Texture2D background;
    private GUIStyle stylePlay = new GUIStyle();
    private static string[] gameNames = { "Lookpick", "Galaxy Invaders", "Wohnheim Spiel", "Angel Spiel", "Endkapmf Spiel" };
    private static bool[] games = { false, false, false, false, false, false };
    private static int[] highscores = new int[5];


	Vector2 scrollPosition = Vector2.zero;

	// Use this for initialization
	void Start () {
        gameController = GameObject.Find("GameController");
        stylePlay.normal.background = playButton;
        
	}
	
	// Update is called once per frame
	void Update () {
 
	}

	void OnGUI()
	{
        GUI.DrawTexture(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(0, 0, 100, 100)), background);
        scrollPosition = GUI.BeginScrollView(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(5, 5, 90, 90)), scrollPosition, gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(5, 5, 85, 250)));
        
        // erstes
        GUI.Box(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(10, 10, 70, 35)), "" );
        GUI.DrawTexture(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(12, 15, 20, 25)), texture1);
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(55, 30, 20, 10)), "", stylePlay)) 
		{
            Application.LoadLevel(7);
		}
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(35, 15, 20, 10)), "" + gameNames[0] + "          Score :" + gameController.GetComponent<PlayerInformation>().getHighscores()[0]);
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(35, 25, 10, 10)), "Spielbar :");
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(45, 25, 20, 10)), "Ja");

        //zweites
        GUI.Box(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(10, 60, 70, 35)), "");
        GUI.DrawTexture(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(12, 65, 20, 25)), texture1);
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(55, 80, 20, 10)), "", stylePlay))
        {
            Application.LoadLevel(7);
        }
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(35, 65, 20, 10)), "" + gameNames[1] + "          Score :" + gameController.GetComponent<PlayerInformation>().getHighscores()[1]);
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(35, 75, 10, 10)), "Spielbar :");
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(45, 75, 20, 10)), "Ja");

        //drites
        GUI.Box(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(10, 110, 70, 35)), "");
        GUI.DrawTexture(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(12, 115, 20, 25)), texture1);
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(55, 130, 20, 10)), "", stylePlay))
        {
            Application.LoadLevel(7);
        }
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(35, 115, 20, 10)), "" + gameNames[2] + "          Score :" + gameController.GetComponent<PlayerInformation>().getHighscores()[2]);
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(35, 125, 10, 10)), "Spielbar :");
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(45, 125, 20, 10)), "Ja");

        //virtes
        GUI.Box(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(10, 160, 70, 35)), "");
        GUI.DrawTexture(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(12, 165, 20, 25)), texture1);
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(55, 180, 20, 10)), "", stylePlay))
        {
            Application.LoadLevel(7);
        }
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(35, 165, 20, 10)), "" + gameNames[3] + "          Score :" + gameController.GetComponent<PlayerInformation>().getHighscores()[3]);
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(35, 175, 10, 10)), "Spielbar :");
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(45, 175, 20, 10)), "Ja");

        //fünftes
        GUI.Box(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(10, 210, 70, 35)), "");
        GUI.DrawTexture(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(12, 215, 20, 25)), texture1);
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(55, 230, 20, 10)), "", stylePlay))
        {
            Application.LoadLevel(7);
        }
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(35, 215, 20, 10)), "" + gameNames[4] + "          Score :" + gameController.GetComponent<PlayerInformation>().getHighscores()[4]);
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(35, 225, 10, 10)), "Spielbar :");
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(45, 225, 20, 10)), "Ja");

        // OLD
        //GUI.Box (new Rect (50, 200, 420, 120), "");
        //GUI.DrawTexture (new Rect (60, 210, 80, 100), texture2);
        //GUI.Button (new Rect (360, 260, 100, 50), "Spielen");
        //GUI.Label (new Rect (160, 210, 100, 50), "Spielname");
        //GUI.Label (new Rect (160, 260, 100, 50), "Spielbar");

        //GUI.Box (new Rect (50, 350, 420, 120), "");
        //GUI.DrawTexture (new Rect (60, 360, 80, 100), texture3);
        //GUI.Button (new Rect (360,410, 100, 50), "Spielen");
        //GUI.Label (new Rect (160, 360, 100, 50), "Spielname");
        //GUI.Label (new Rect (160, 410, 100, 50), "Spielbar");

        //GUI.Box (new Rect (50, 500, 420, 120), "");
        //GUI.DrawTexture (new Rect (60, 510, 80, 100), texture4);
        //GUI.Button (new Rect (360,560, 100, 50), "Spielen");
        //GUI.Label (new Rect (160, 510, 100, 50), "Spielname");
        //GUI.Label (new Rect (160, 560, 100, 50), "Spielbar");

        //GUI.Box (new Rect (50, 650, 420, 120), "");
        //GUI.DrawTexture (new Rect (60, 660, 80, 100), texture5);
        //GUI.Button (new Rect (360,710, 100, 50), "Spielen");
        //GUI.Label (new Rect (160, 660, 100, 50), "Spielname");
        //GUI.Label (new Rect (160, 710, 100, 50), "Spielbar");

		GUI.EndScrollView();
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), verlauf);
	}
}
