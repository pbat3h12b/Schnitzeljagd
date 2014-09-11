using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    // Zeit seit des nächsten Updates
    private static float timeOnNextUpdate = 0;
    // Zeit zwischen Updates
    private static int timeBetweenUpdates = 5;
    //Das letzte secret was vom Spieler gefunden wurde
    private static string lastFoundSecret = "";
    // Längengrad des Benutzers
    private static float userLongitude = 0;
    // Breitengrad des Benutzers
    private static float userLatitude = 0;

    private List<CacheScript> _caches;

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

        /* Ausführen, wenn die Zeit des nächsten Updates erreicht wurde 
         * und die GPS-Verbindung keine Fehler aufweist
         */
        if (Time.time >= timeOnNextUpdate && CheckGeoStatus())
        {
            // GPS-Service starten
            Input.location.Start();

            // Längengrad auslesen
            userLongitude = Input.location.lastData.longitude;
            // Breitengrad festlegen
            userLatitude = Input.location.lastData.latitude;

            // Längen- und Breitengrad an die API schicken
            updateGeoData(userLongitude, userLatitude);

            // Zeitpunkt des nächsten Updates festlegen
            timeOnNextUpdate = Time.time + timeBetweenUpdates;
            // GPS-Service beenden
            Input.location.Stop();
        }
	}

    /*
     * Boolean ob Geo-Status verfügbar ist zurückgeben
     * und eventuelle Fehlermeldung angeben.
     */
    bool CheckGeoStatus()
    {
        // Wenn GPS vom Benutzer aktiviert wurde
        if (Input.location.isEnabledByUser)
        {
            // Wenn die GPS-Verbindung keine Fehler aufweist
            if (Input.location.status != LocationServiceStatus.Failed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    // gibt Längengrad des Benutzers zurück
    public float getUserLongitude()
    {
        return userLongitude;
    }

    // gibt Breitengrad des Benutzers zurück
    public float getUserLatitude()
    {
        return userLatitude;
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
    public void getUserData()
    {
        //Holt alle Informationen vom Sever ab
        List<Logbookentry> temp = GameObject.Find("GameController").GetComponent<RESTCommunication>().getAllLogBookEntrys();
        
        for (int i = 0; i < temp.Count; i++)
        {
            cacheStatus[i] = true;
        }
        //läuft die ganze Liste durch
        for (int i = 0; i < temp.Count; i++)
        {
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
        //gibt der API die warte
        GameObject.Find("GameController").GetComponent<RESTCommunication>().UpdatePosition(longitude,latitude);
        //setzt die Zeit wieder auf 0 bis zum nächsten Update
        if (GameObject.Find("GameController").GetComponent<RESTCommunication>().TestServerConnection().Success == false)
        {
            Application.LoadLevel(11);
        }
    }

    //Gibt neuen Score och
    public void newScore(string SpielID, int Score)
    {

        //Übergibt der API dem den Score zum Spiel
        GameObject.Find("GameController").GetComponent<RESTCommunication>().SubmitGameScore(Score, SpielID);
    }

    //Überprüft die Gültigkeit des eingegebenen secrets
    public bool checkCach(string secret)
    {
        //Fragt API ob es möglich is
        if (GameObject.Find("GameController").GetComponent<RESTCommunication>().checkCacheSecret(secret).Success)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Spieler hat Spiel geschafft
    public void markPuzzel()
    {
        //Sagt der API das der Spieler das Spiel für den nächsten Cache geschafft hat
        GameObject.Find("GameController").GetComponent<RESTCommunication>().markPuzzelSolved();
    }

    //Gibt die Highscores zurück
    public int[] getHighscores()
    {
        return highscores;
    }

    //Setzt das zu letzt gefundene Secret
    public void setCachSecret(string secret)
    {
        lastFoundSecret = secret;
    }

    //Gibt secret zurück
    public string getSecret()
    {
        return lastFoundSecret;
    }

    //gibt game Sats zurück
    public bool[] getGames()
    {
        return cacheStatus;
    }

    public bool[] getPuzzels()
    {
        return games;
    }

    /// <summary>
    /// Methode von Oliver Noll
    /// </summary>
    void ReadCacheList()
    {
        _caches = new List<CacheScript>();

        // Muss einzeln erstellt werden, da XML-Dateien nicht auf Smartphones gelesen werden können
        _caches.Add(new CacheScript("b.i.b. Eingang", 8.73707f, 51.73075f));
        _caches.Add(new CacheScript("Zukunftsmeile", 8.73807f, 51.73057f));
        _caches.Add(new CacheScript("Heinz Nixdorf Forum", 8.73618f, 51.73147f));
        _caches.Add(new CacheScript("Wohnheim", 8.73740f, 51.72956f));
        _caches.Add(new CacheScript("Fluss", 8.73554f, 51.73064f));
        _caches.Add(new CacheScript("b.i.b. Serverraum", 8.73635f, 51.73106f));
    }

    /// <summary>
    /// Methode von Oliver Noll
    /// </summary>
    /// <returns></returns>
    public List<CacheScript> GetNextCaches()
    {
        ReadCacheList();

        bool foundNextCache = false;
        List<CacheScript> nextCaches = new List<CacheScript>();

        if (!cacheStatus[_caches.Count - 1])
        {
            for (int i = 0; i < _caches.Count; i++)
            {
                if (cacheStatus[i])
                    _caches[i].Founded = true;

                if (!foundNextCache && !cacheStatus[i])
                {
                    nextCaches.Add(_caches[i]);
                    foundNextCache = true;
                }
            }
        }
        else
        {
            nextCaches.AddRange(_caches);
        }

        return nextCaches;
    }
}

