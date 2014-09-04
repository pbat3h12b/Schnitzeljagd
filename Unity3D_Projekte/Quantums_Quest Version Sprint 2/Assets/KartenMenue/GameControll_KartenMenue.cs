/// <summary>
/// Erstellt von Oliver Noll am 18.06.2014
/// Zuletzt bearbeitet von Oliver Noll am 03.09.2014
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControll_KartenMenue : MonoBehaviour {

    [HideInInspector]
    public Texture2D backgroundImage;
	public Texture2D imageMap;
	public Texture2D imageCurrentUser;
    public Texture2D imageCurrentCache;

    private List<CacheScript> caches = new List<CacheScript>();

	private GameObject gameController;
    private Component staticScript;

    private float mapMinLatitude = 51.73178f;
    private float mapMinLongitude = 8.73458f;
    private float mapMaxLatitude = 51.72919f;
	private float mapMaxLongitude = 8.73945f;
	
    private Rect backgroundTransform;
    private Rect mapTransform;
    private Rect userTransform;
    private Rect buttonScanTransform;
	
	private float timeBetweenUpdates = 5;
	private float timeOnNextUpdate = 0;
	
	private string buttonScanValue = "Scannen";

    private CacheScript nextCache;
	
	/*
	 * Noch nicht vordefinierte Variablen werden abhängig
	 * von vordefinierten Variablen initialisiert
	 */
	void Start ()
	{
		gameController = GameObject.Find("GameController");

        backgroundTransform = gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(-20, -20, 140, 140));

        // new
        mapTransform = gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(0, 0, 90, 100));

        // new
        userTransform.width = mapTransform.width / 20;
        userTransform.height = mapTransform.height / 20;

        buttonScanTransform = gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(91, 2, 8, 8));

        // Caches initialisieren
        InitializeCaches();
	}
	
	/*
	 * Aktion pro Frame durchführen (z.Z. keine Aufgabe)
	 */
	void Update()
	{
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(1);
        }
	}

    void InitializeCaches()
    {
        #region Caches
        caches.Add(new CacheScript("b.i.b. Eingang",
                            8.73707f,
                            51.73075f,
                            new Rect(GetLongitudePosition(8.73707f),
                                GetLatitudePosition(51.73075f),
                                userTransform.width,
                                userTransform.height)));

        caches.Add(new CacheScript("Zukunftsmeile",
                                    8.73807f,
                                    51.73057f,
                                    new Rect(GetLongitudePosition(8.73807f),
                                        GetLatitudePosition(51.73057f),
                                        userTransform.width,
                                        userTransform.height)));

        caches.Add(new CacheScript("Heinz Nixdorf Forum",
                                    8.73618f,
                                    51.73147f,
                                    new Rect(GetLongitudePosition(8.73618f),
                                        GetLatitudePosition(51.73147f),
                                        userTransform.width,
                                        userTransform.height)));

        caches.Add(new CacheScript("Wohnheim",
                                    8.73740f,
                                    51.72956f,
                                    new Rect(GetLongitudePosition(8.73740f),
                                        GetLatitudePosition(51.72956f),
                                        userTransform.width,
                                        userTransform.height)));

        caches.Add(new CacheScript("Fluss",
                                    8.73554f,
                                    51.73064f,
                                    new Rect(GetLongitudePosition(8.73554f),
                                        GetLatitudePosition(51.73064f),
                                        userTransform.width,
                                        userTransform.height)));

        caches.Add(new CacheScript("b.i.b. Serverraum",
                                    8.73635f,
                                    51.73106f,
                                    new Rect(GetLongitudePosition(8.73635f),
                                        GetLatitudePosition(51.73106f),
                                        userTransform.width,
                                        userTransform.height))); 
        #endregion

        bool[] cachesStatus = gameController.GetComponent<PlayerInformation>().GetCacheStatus();

        bool foundNextCache = false;
        for (int i = 0; i < caches.Count; i++)
        {
            if (cachesStatus[i])
                caches[i].Founded = true;

            if (!foundNextCache && !cachesStatus[i])
            {
                nextCache = caches[i];
                foundNextCache = true;
            }
        }
    }
	
	/*
	 * Oberfläche pro Frame konstruieren
	 */
	void OnGUI()
	{
        /*
         * Karten-Textur auf GUI wiedergeben
         */
		GUI.DrawTexture (mapTransform, 
                            imageMap);

        if (Time.time >= timeOnNextUpdate && CheckGeoStatus())
		{
			Input.location.Start ();

            float userLongitude = Input.location.lastData.longitude;
            userTransform.x = GetLongitudePosition(userLongitude);
			float userLatitude = Input.location.lastData.latitude;
            userTransform.y = GetLatitudePosition(userLatitude);
            timeOnNextUpdate = Time.time + timeBetweenUpdates;

            gameController.GetComponent<PlayerInformation>().updateGeoData(userLongitude, userLatitude);
            Debug.Log(Time.time);
			Input.location.Stop ();
		}
        else if (!CheckGeoStatus())
        {
            PlayerPrefs.SetString("errorMsg", "Keine GPS-Verbindung verfügbar!");
            Application.LoadLevel(1);
        }

        GUI.DrawTexture(userTransform, 
                        imageCurrentUser);

        GUI.DrawTexture(nextCache.Transform,
                        imageCurrentCache);
		
		if (GUI.Button(buttonScanTransform,
                        buttonScanValue))
		{
			Application.LoadLevel(5);
		}

        /*
         * Hintergrund auf GUI wiedergeben
         */
        GUI.DrawTexture(backgroundTransform,
                            backgroundImage);
	}
	
	/*
	 * Boolean ob Geo-Status verfügbar ist zurückgeben
	 * und eventuelle Fehlermeldung angeben.
	 */
	bool CheckGeoStatus()
	{
		if (Input.location.isEnabledByUser)
		{
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
	
	/*
	 * Breitengrad relativ zur Kartenskalierung zurückgeben
	 */
	float GetLatitudePosition(float latitute)
	{
		/*float latitudePosition = mapTransform.x + (mapTransform.width * 
		                                         ((mapMinLatitude - latitute) * 100) / 
		                                         (mapMinLatitude - mapMaxLatitude) / 100);*/
        float latitudePosition = ((latitute - mapMinLatitude) * 100) / (mapMaxLatitude - mapMinLatitude);
        latitudePosition = mapTransform.y + ((mapTransform.height * latitudePosition) / 100);
        latitudePosition = Mathf.Clamp(latitudePosition, mapTransform.y, mapTransform.y + mapTransform.height);
        latitudePosition -= userTransform.height / 2;
		return latitudePosition;
	}
	
	/*
	 * Längengrad relativ zur Kartenskalierung zurückgeben
	 */
    float GetLongitudePosition(float longitude)
	{
       /* float longitudePosition = mapTransform.y + (mapTransform.height *
                                                  ((mapMinLongitude - longitude) * 100) /
                                                  (mapMinLongitude - mapMaxLongitude) / 100);*/
        float longitudePosition = ((longitude - mapMinLongitude) * 100) / (mapMaxLongitude - mapMinLongitude);
        longitudePosition = mapTransform.x + ((mapTransform.width * longitudePosition) / 100);
        longitudePosition = Mathf.Clamp(longitudePosition, mapTransform.x, mapTransform.x + mapTransform.width);
        longitudePosition -= userTransform.width / 2;
		return longitudePosition;
	}
}