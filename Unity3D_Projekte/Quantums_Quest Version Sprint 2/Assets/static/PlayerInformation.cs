using UnityEngine;
using System.Collections;

public class PlayerInformation : MonoBehaviour {

    private static string userName = "";
    private static float longitude;
    private static float latitude;
    private static bool[] caches = {false,false,false,false,false,false};
    private static string[] logBook = new string[5];
    private static string[] gameNames = { "Lookpick", "Galaxy Invaders", "Wohnheim Spiel", "Angel Spiel", "Endkapmf Spiel" };
    private static bool[] games = { false, false, false, false, false, false };
    private static int[] highscores = new int[5];
    private static float timeSinceUpdate = 0;


	// Use this for initialization
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	// Update is called once per frame
	void Update () {
        Input.location.Start();
        longitude = Input.location.lastData.longitude;
        latitude = Input.location.lastData.latitude;
        timeSinceUpdate += Time.deltaTime;
        updateGeoData();
        Input.location.Stop();
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
		//caches = GameObject.Find("GameController").GetComponent<RESTCommunication>().getCaches(userName);
        games = caches;
		//highscores = GameObject.Find("GameController").GetComponent<RESTCommunication>().getHighscors(userName);
    }

    void updateGeoData()
    {
        if (timeSinceUpdate > 5 && userName != "")
        {
			GameObject.Find("GameController").GetComponent<RESTCommunication>().UpdatePosition(longitude,latitude);
            timeSinceUpdate = 0;
        }
    }

    void updateCach(int ID)
    {
        caches[ID - 1] = true;
		//GameObject.Find("GameController").GetComponent<RESTCommunication>().updateCach(ID);
    }

    void newScore(int SpielID, int Score)
    {
        if (highscores[SpielID - 1] < Score)
        {
            highscores[SpielID - 1] = Score;
        }

		//GameObject.Find("GameController").GetComponent<RESTCommunication>().newScore(SpielID, Score);
    }

    void newlogBook( int CachID, string Text )
    {
        logBook[CachID - 1] = Text;
		//GameObject.Find("GameController").GetComponent<RESTCommunication>().newLogBook(Text, CachID);
    }
}

