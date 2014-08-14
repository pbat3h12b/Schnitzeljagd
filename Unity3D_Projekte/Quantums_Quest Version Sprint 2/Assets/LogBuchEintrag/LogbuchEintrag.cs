using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogbuchEintrag : MonoBehaviour {

    [HideInInspector]
    private GameObject gameController;

    public Texture2D Weiter;
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
        GUI.Box(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(10, 10, 80, 80)), "");
        lgeintrag = GUI.TextField(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(15, 15, 50, 70)), lgeintrag);
        GUI.Label(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(70, 15, 10, 10)), "Hier können Sie eine Nachricht hinterlassen");
        GUI.Button(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(67, 30, 20, 10)), "", styleWeiter);
	}
}
