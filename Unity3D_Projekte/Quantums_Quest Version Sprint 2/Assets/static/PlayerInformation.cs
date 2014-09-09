using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class PlayerInformation : MonoBehaviour {

    private static string userName = "";
    private static bool[] cacheStatus = { false, false, false, false, false, false };
    private static string[] logBook = new string[5];
    private static string[] cachnamen = { "Zukunftsmeile", "HNF", "Wohnheim", "Fluss", "Serverraum" };
    private static string[] gameNames = { "Lookpick", "Galaxy Invaders", "Wohnheim Spiel", "Angel Spiel", "Endkapmf Spiel" };
    private static bool[] games = { false, false, false, false, false, false };
    private static int[] highscores = new int[5];
    private static float timeSinceUpdate = 0;
    private static string lastFoundSecret = "";

    private List<CacheScript> _caches = new List<CacheScript>();

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
        List<Logbookentry> temp = GameObject.Find("GameController").GetComponent<RESTCommunication>().getAllLogBookEntrys();

        for (int i = 0; i < temp.Count; i++)
        {
            cacheStatus[i] = temp[i].Puzzlesolved;
            if (temp[i].Puzzlesolved)
            {
                games[i] = true;
            }
        }

        for (int i = 0; i < highscores.Length; i++)
        {
            highscores[i] = GameObject.Find("GameController").GetComponent<RESTCommunication>().getTopScoreByUser(cachnamen[i]).Points;
        }
    }

    public void updateGeoData(float longitude, float latitude)
    {
        if (userName != "")
        {
			GameObject.Find("GameController").GetComponent<RESTCommunication>().UpdatePosition(longitude,latitude);
            timeSinceUpdate = 0;
        }
    }

    public void newScore(string SpielID, int Score)
    {


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

    public bool[] getGames()
    {
        return games;
    }

    void ReadCacheList()
    {
        XmlDocument document = new XmlDocument();
        document.Load("Assets/CacheList.xml");
        XmlNode cacheListXml = document.DocumentElement;

        foreach(XmlNode cacheXml in cacheListXml.ChildNodes)
        {
            string description = cacheXml.Attributes["description"].Value;
            float longitude = float.Parse(cacheXml.Attributes["longitude"].Value);
            float latitude = float.Parse(cacheXml.Attributes["latitude"].Value);

            _caches.Add(new CacheScript(description, longitude, latitude));
        }
    }

    public CacheScript GetNextCache()
    {
        ReadCacheList();

        bool foundNextCache = false;
        CacheScript nextCache = null;

        for (int i = 0; i < _caches.Count; i++)
        {
            if (cacheStatus[i])
                _caches[i].Founded = true;

            if (!foundNextCache && !cacheStatus[i])
            {
                nextCache = _caches[i];
                foundNextCache = true;
            }
        }

        return nextCache;
    }
}

