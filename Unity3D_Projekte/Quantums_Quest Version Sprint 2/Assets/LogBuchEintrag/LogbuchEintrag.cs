using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogbuchEintrag : MonoBehaviour {

    [HideInInspector]
    private GameObject gameController;

    public Texture2D Weiter;
    public Texture2D verlauf;
    public Texture2D background;
    GUIStyle styleWeiter = new GUIStyle();

	string lgeintrag = "";

	// Use this for initialization
	void Start () {
        gameController = GameObject.Find("GameController");
        styleWeiter.normal.background = Weiter;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
        GUI.DrawTexture(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(0, 0, 100, 100)), background);
        GUI.Box(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(10, 10, 80, 80)), "");
        lgeintrag = GUI.TextField(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(15, 15, 50, 70)), lgeintrag);
        GUI.Label(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(70, 15, 10, 10)), "Hier können Sie eine Nachricht hinterlassen");
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(67, 30, 20, 10)), "", styleWeiter) && lgeintrag != "")
        {
            Response temp = gameController.GetComponent<RESTCommunication>().MakeLogBookEntry(lgeintrag, gameController.GetComponent<PlayerInformation>().getSecret());

            if (temp.Success == true)
            {

            }
            else
            {

            }
        }
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), verlauf);
	}
}
