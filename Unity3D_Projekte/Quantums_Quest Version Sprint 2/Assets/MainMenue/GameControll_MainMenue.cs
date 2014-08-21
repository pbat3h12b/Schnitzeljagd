﻿using UnityEngine;
using System.Collections;

public class GameControll_MainMenue : MonoBehaviour {
    [HideInInspector]
    private GameObject gameController;

    public Texture2D KarteAnzeigen;
    public Texture2D SpieleAnzeigen;

    GUIStyle styleKarteAnzeigen = new GUIStyle();
    GUIStyle styleSpieleAnzeigen = new GUIStyle();

    //private int screenWidth;
    //private int screenHeight;
    //private double scaleWidth;
    //private double scaleHeight;
    // Use this for initialization
    void Start()
    {
        gameController = GameObject.Find("GameController");

        styleKarteAnzeigen.normal.background = KarteAnzeigen;
        styleSpieleAnzeigen.normal.background = SpieleAnzeigen;

        //screenHeight = Screen.width;
        //screenWidth = Screen.height;

        //scaleWidth = screenWidth / 100;
        //scaleHeight = screenHeight / 100;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUI.Button(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(15, 40, 30, 15)), "",styleKarteAnzeigen))
        {
            Application.LoadLevel(4);
        }

        if (GUI.Button(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(55, 40, 30, 15)), "",styleSpieleAnzeigen))
        {
            Application.LoadLevel(3);
        }
    }
}