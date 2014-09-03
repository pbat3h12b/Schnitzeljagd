using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInformation : MonoBehaviour {

    private static string userName = "";
    private static bool[] caches = {false,false,false,false,false,false};
    private static string[] logBook = new string[5];
    private static string[] cachnamen = { "Zukunftsmeile", "HNF", "Wohnheim", "Fluss", "Serverraum" };
    private static string[] gameNames = { "Lookpick", "Galaxy Invaders", "Wohnheim Spiel", "Angel Spiel", "Endkapmf Spiel" };
    private static bool[] games = { false, false, false, false, false, false };
    private static int[] highscores = new int[5];
    private static float timeSinceUpdate = 0;
    private static string lastFoundSecret = "";


	// Use this for initialization
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(1);
        }
	}

    public string getUsername()
    {
        return userName;
    }

    public void logIn(string name)
    {
        userName = name;
        getUserData();
    }

    void getUserData()
    {
        List<Logbookentry> temp = GameObject.Find("GameController").GetComponent<RESTCommunication>().getAlleLogBookEntrys();

        for (int i = 0; i < temp.Count; i++)
        {
            caches[i] = temp[i].Puzzlesolved;
            games[i] = true;
        }

        for (int i = 0; i < highscores.Length; i++)
        {
            highscores[i] = GameObject.Find("GameController").GetComponent<RESTCommunication>().getTopScoreByUser(cachnamen[i]).Points;
        }
    }

    public void updateGeoData(float longitude, float latitude)
    {
        if (timeSinceUpdate > 5 && userName != "")
        {
			GameObject.Find("GameController").GetComponent<RESTCommunication>().UpdatePosition(longitude,latitude);
            timeSinceUpdate = 0;
        }
    }

    public void newScore(string SpielID, int Score)
    {
        //if (highscores[SpielID - 1] < Score)
        //{
        //    highscores[SpielID - 1] = Score;
        //}

        GameObject.Find("GameController").GetComponent<RESTCommunication>().SubmitGameScore(Score, SpielID);
    }

    public bool checkCach(string secret)
    {
        if (GameObject.Find("GameController").GetComponent<RESTCommunication>().checkCacheSecret(secret).Success)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void markPuzzel()
    {
        GameObject.Find("GameController").GetComponent<RESTCommunication>().markPuzzelSolved();
    }


    public int[] getHighscores()
    {
        return highscores;
    }

    public void setCachSecret(string secret)
    {
        lastFoundSecret = secret;
    }

    public string getSecret()
    {
        return lastFoundSecret;
    }

    public Rect GetRelativeRect(Rect oldRect)
    {
        Rect newRect = new Rect();

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        newRect.width = (oldRect.width * screenWidth) / 100;
        newRect.height = (oldRect.height * screenHeight) / 100;

        newRect.x = (oldRect.x * screenWidth) / 100;
        newRect.y = (oldRect.y * screenHeight) / 100;

        return newRect;
    }
}

