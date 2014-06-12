using UnityEngine;
using System.Collections;

public class PlayerInformation : MonoBehaviour {

    private static string userName;
    private static float longitude;
    private static float latitude;
    private static bool[] caches = {false,false,false,false,false,false};
    private static string[] logBook = new string[5];
    private static string[] gameNames = { "Lookpick", "Galaxy Invaders", "Wohnheim Spiel", "Angel Spiel", "Endkapmf Spiel" };
    private static bool[] games = new bool[5];
    private static int[] highscores = new int[5];
    private static float timeSinceUpdate;


	// Use this for initialization
	void Start () {
        Input.location.Start();
	}
	
	// Update is called once per frame
	void Update () {

        longitude = Input.location.lastData.longitude;
        latitude = Input.location.lastData.latitude;
        updateGeoData();

	}

    void logIn(string name)
    {
        userName = name;
    }

    void getUserData()
    {
        caches = GameObject.Find("GameController").GetComponent<beispiel_api>().getCaches(userName);
        games = caches;
        highscores = GameObject.Find("GameController").GetComponent<beispiel_api>().getHighscors(userName);
    }

    void updateGeoData()
    {
        if (timeSinceUpdate > 60)
        {
            GameObject.Find("GameController").GetComponent<beispiel_api>().updateGeoData(longitude, latitude);
        }
    }

    void updateCach(int ID)
    {
        caches[ID - 1] = true;
        GameObject.Find("GameController").GetComponent<beispiel_api>().updateCach(ID);
    }

    void newScore(int SpielID, int Score)
    {
        if (highscores[SpielID - 1] < Score)
        {
            highscores[SpielID - 1] = Score;
        }

        GameObject.Find("GameController").GetComponent<beispiel_api>().newScore(SpielID, Score);
    }

    void newlogBook( int CachID, string Text )
    {
        logBook[CachID - 1] = Text;
        GameObject.Find("GameController").GetComponent<beispiel_api>().newLogBook(Text, CachID);
    }

}

