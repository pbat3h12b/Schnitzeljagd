using UnityEngine;
using System.Collections;

public class GameControll_KartenMenue : MonoBehaviour {

    [HideInInspector]
    public Texture2D backgroundImage;
	public Texture2D imageMap;
	public Texture2D imageCurrentUser;

	private GameObject gameController;

    // new
    private Component staticScript;

    private float mapMinLatitude = 51.73178f;
    private float mapMinLongitude = 8.73458f;
    private float mapMaxLatitude = 51.72919f;
	private float mapMaxLongitude = 8.73945f;
	
    // new
    private Rect backgroundTransform;
    private Rect mapTransform;
    private Rect userTransform;
    private Rect buttonScanTransform;
	
	private float timeBetweenUpdates = 5;
	private float timeSinceLastUpdate = 0;
	
	private string errorMessage = "";
	private string buttonScanValue = "Scannen";
	
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
	}
	
	/*
	 * Aktion pro Frame durchführen (z.Z. keine Aufgabe)
	 */
	void Update()
	{

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

        /*
         * Fehlermeldung auf GUI wiedergeben
         */
        GUI.Label(new Rect(mapTransform.x,
                            mapTransform.y - 50,
                            mapTransform.width,
                            mapTransform.height), 
                            errorMessage);

		if (timeSinceLastUpdate <= 0 && CheckGeoStatus())
		{
			Input.location.Start ();

			float userLatitude = Input.location.lastData.latitude;
            userTransform.y = GetLatitudePosition(userLatitude);
			float userLongitude = Input.location.lastData.longitude;
            userTransform.x = GetLongitudePosition(userLongitude);
			timeSinceLastUpdate = timeBetweenUpdates;

			Input.location.Stop ();
		}
		else
		{
			timeSinceLastUpdate -= Time.deltaTime;
		}

        GUI.DrawTexture(userTransform, 
                        imageCurrentUser);
		
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
				if(Input.location.status != LocationServiceStatus.Running)
				{
					Input.location.Start ();
				}
				
				errorMessage = "";
				return true;
			}
			else
			{
				errorMessage = "Unable to determine device location";
				return false;
			}
		}
		else
		{
			errorMessage = "Location-Service not enabled";
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