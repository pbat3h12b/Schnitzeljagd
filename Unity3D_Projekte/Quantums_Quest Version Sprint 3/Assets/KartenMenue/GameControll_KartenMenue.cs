/// <summary>
/// Erstellt von Oliver Noll am 18.06.2014
/// Zuletzt bearbeitet von Oliver Noll am 03.09.2014
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class GameControll_KartenMenue : MonoBehaviour {

    // Hintergrund-Bild
    public Texture2D backgroundImage;
    // Bild der Karte
	public Texture2D imageMap;
    // Bild des Benutzers
	public Texture2D imageCurrentUser;
    // Bild des nächsten Caches
    public Texture2D imageCurrentCache;
    // Zurück-Button-Bild
    public Texture2D backgroundBack;
    // GUI-Style für Zurück-Button
    public GUIStyle styleBack = new GUIStyle();

    //Ein Objekt des GameControllers
	private GameObject gameController;

    // Mindest-Breitengrad
    private float mapMinLatitude = 51.73178f;
    // Mindest-Längengrad
    private float mapMinLongitude = 8.73458f;
    // Maximaler Breitengrad
    private float mapMaxLatitude = 51.72919f;
    // Maximaler Längengrad
	private float mapMaxLongitude = 8.73945f;
	
    // Rechteck (x, y, Breite, Höhe) für Hintergrund
    private Rect backgroundTransform;
    // Rechteck (x, y, Breite, Höhe) für die Karte
    private Rect mapTransform;
    // Rechteck (x, y, Breite, Höhe) für den Benutzer
    private Rect userTransform;
    // Rechteck (x, y, Breite, Höhe) für das Scan-Bild
    private Rect buttonScanTransform;
	
    // Zeit zwischen Updates der Geo-Daten
	private float timeBetweenUpdates = 5;
    // Zeit des nächsten Updates
	private float timeOnNextUpdate = 0;

    // Cachescript für den nächsten Script
    private List<CacheScript> _nextCaches = new List<CacheScript>();

    // GUI-Skin
    public GUISkin guiSkin;
	
	/*
	 * Noch nicht vordefinierte Variablen werden abhängig
	 * von vordefinierten Variablen initialisiert
	 */
	void Start ()
	{
        // Fehlermeldung zurücksetzen
        PlayerPrefs.SetString("errorMsg", "");

        // Den Game-Manager suchen
		gameController = GameObject.Find("GameController");

        // Die Background-Größe und -Position festlegen
        backgroundTransform = gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(-20, -20, 140, 140));

        // Die Karten-Größe und -Position festlegen
        mapTransform = gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(0, 0, 90, 100));

        // Größe des Benutzer-Bildes festlegen
        userTransform.width = mapTransform.width / 20;
        userTransform.height = mapTransform.height / 20;

        // Größe und Position des "Scan"-Buttons festlegen
        buttonScanTransform = gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(90, 0, 10, 10));
        buttonScanTransform.height = buttonScanTransform.width;

        // Den nächsten Cache festlegen
        _nextCaches = gameController.GetComponent<PlayerInformation>().GetNextCaches();

        // Den Button Hintergrund vom Zurück-Button festlegen
        styleBack.normal.background = backgroundBack;
        gameController.GetComponent<PlayerInformation>().getUserData();
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

        // Längengrad auslesen
        float userLongitude = gameController.GetComponent<PlayerInformation>().getUserLongitude();
        // x-Position des Benutzers festlegen
        userTransform.x = GetLongitudePosition(userLongitude);
        // Breitengrad festlegen
        float userLatitude = gameController.GetComponent<PlayerInformation>().getUserLongitude();
        // y-Position des Benutzers festlegen
        userTransform.y = GetLatitudePosition(userLatitude);

        // Benutzer-Bild auf der GUI anzeigen
        GUI.DrawTexture(userTransform, 
                        imageCurrentUser);

        // Nächsten Cache oder alle Cache (wenn letzter freigeschaltet) anzeigen
        foreach (CacheScript nextCache in _nextCaches)
        {
            // Größe & Position vom nächsten Cache im Rechteck
            Rect nextCacheTransform = new Rect(0, 0, 0, 0);
            // Position-x vom Cache
            nextCacheTransform.x = GetLongitudePosition(nextCache.Longitude);
            // Position-y vom Cache
            nextCacheTransform.y = GetLatitudePosition(nextCache.Latitude);
            // Höhe und Breite vom Cache
            nextCacheTransform.width = userTransform.width;
            nextCacheTransform.height = userTransform.height;
            // Nächsten Cache auf GUI anzeigen
            GUI.DrawTexture(nextCacheTransform,
                            imageCurrentCache);
        }
		
        // Wenn "Scan"-Button geklickt wurde
		if (GUI.Button(buttonScanTransform,
                        "",
                        guiSkin.button))
		{
            // Code-Eingabe-GUI laden
			Application.LoadLevel(5);
		}

        /*
         * Hintergrund auf GUI wiedergeben
         */
        GUI.DrawTexture(backgroundTransform,
                            backgroundImage);

        // Wird ausgeführt, wenn Zurück-Button oben links oder der Zurück-Button von Endgeräten mit Android gedrückt wurde
        if (GUI.Button(gameController.GetComponent<GUI_Scale>().GetRelativeRect(new Rect(1, 1, 20, 10)), "", styleBack) || Input.GetKeyDown(KeyCode.Escape))
        {
            //Springt eine Szene zurück
            Application.LoadLevel(1);
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