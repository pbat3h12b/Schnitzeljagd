/// <summary>
/// Erstellt von Oliver Noll am 18.06.2014
/// Zuletzt bearbeitet von Oliver Noll am 03.09.2014
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class GameControll_KartenMenue : MonoBehaviour {

    [HideInInspector]
    public Texture2D backgroundImage;
	public Texture2D imageMap;
	public Texture2D imageCurrentUser;
    public Texture2D imageCurrentCache;

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

    private CacheScript _nextCache;

    public GUISkin guiSkin;
	
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

        buttonScanTransform = gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(90, 0, 10, 10));
        buttonScanTransform.height = buttonScanTransform.width;

        _nextCache = gameController.GetComponent<PlayerInformation>().GetNextCache();
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
			Input.location.Stop ();
		}
        else if (!CheckGeoStatus())
        {
            PlayerPrefs.SetString("errorMsg", "Keine GPS-Verbindung verfügbar!");
            Application.LoadLevel(1);
        }

        GUI.DrawTexture(userTransform, 
                        imageCurrentUser);

        // Draw next Cache
        Rect nextCacheTransform = new Rect(0, 0, 0, 0);
        nextCacheTransform.x = GetLongitudePosition(_nextCache.Longitude);
        nextCacheTransform.y = GetLatitudePosition(_nextCache.Latitude);
        nextCacheTransform.width = userTransform.width;
        nextCacheTransform.height = userTransform.height;
        GUI.DrawTexture(nextCacheTransform,
                        imageCurrentCache);
		
		if (GUI.Button(buttonScanTransform,
                        "",
                        guiSkin.button))
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