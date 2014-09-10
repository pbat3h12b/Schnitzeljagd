using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

/// <summary>
/// Klasse erstell von Niclas Hüppmeier
/// </summary>

public class PlayerInformation : MonoBehaviour {
    //Benutzer name
    private static string userName = "";
    //Der status der caches des angemeldeten Spielers
    private static bool[] cacheStatus = { false, false, false, false, false, false };
    //Die Namen aller Caches zugleich auch die ID's der Spiele
    private static string[] cachnamen = { "Zukunftsmeile", "HNF", "Wohnheim", "Fluss", "Serverraum" };
    //Die Namen aller Spiele
    private static string[] gameNames = { "Lookpick", "Galaxy Invaders", "Wohnheim Spiel", "Angel Spiel", "Endkapmf Spiel" };
    //Ob ein Spiel freigeschaltet ist oder nicht
    private static bool[] games = { false, false, false, false, false, false };
    //Speichert die besten scores des Angemeldeten Spielers
    private static int[] highscores = new int[5];
    //Zeit seit des letzten Updates
    private static float timeSinceUpdate = 0;
    //Das letzte secret was vom Spieler gefunden wurde
    private static string lastFoundSecret = "";

    private List<CacheScript> _caches = new List<CacheScript>();

	// Use this for initialization
	void Start () {
        //lässt den Bildschirm nicht in den Sperrmodus gehen
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.LoadLevel(1);
        //}
	}

    //gibt den Usernamen zurück
    public string getUsername()
    {
        return userName;
    }

    //Loggt den Spieler ein
    public void logIn(string name)
    {
        //Speichert den Usernamen
        userName = name;
        //Holt alle relevanten Userdaten vom Sever
        getUserData();
    }

    //fragt Userdaten vom Server ab
    void getUserData()
    {
        //Holt alle Informationen vom Sever ab
        List<Logbookentry> temp = GameObject.Find("GameController").GetComponent<RESTCommunication>().getAllLogBookEntrys();

        //läuft die ganze Liste durch
        for (int i = 0; i < temp.Count; i++)
        {
            //Holt sich den Status der einzelnden Caches ab
            cacheStatus[i] = temp[i].Puzzlesolved;
            //falls das Puzzle true ist wird auch das Spiel freigeschaltet
            if (temp[i].Puzzlesolved)
            {
                games[i] = true;
            }
        }
        //Holt sich alle Highscores ab
        for (int i = 0; i < highscores.Length; i++)
        {
            //holt den highscore zu einem bestimmten Spiel
            highscores[i] = GameObject.Find("GameController").GetComponent<RESTCommunication>().getTopScoreByUser(cachnamen[i]).Points;
        }
    }

    //Updated die Position des Spielers
    public void updateGeoData(float longitude, float latitude)
    {
        //falls der User eingeloggt ist
        if (userName != "")
        {
            //gibt der API die warte
			GameObject.Find("GameController").GetComponent<RESTCommunication>().UpdatePosition(longitude,latitude);
            //setzt die Zeit wieder auf 0 bis zum nächsten Update
            timeSinceUpdate = 0;
        }
    }

    //
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

