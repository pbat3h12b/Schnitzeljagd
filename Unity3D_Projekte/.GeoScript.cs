using UnityEngine;
using System.Collections;

public class GeoScript : MonoBehaviour {
	
	[HideInInspector]
	public Texture2D imageMap;
	public Texture2D imageCurrentUser;

	private GameObject gameController;
	
	private int screenWidth;
	private int screenHeight;
	
	private double scaleWidth;
	private double scaleHeight;

	private float mapMinLatitude = 0;
	private float mapMinLongitude = 0;
	private float mapMaxLatitude = 100;
	private float mapMaxLongitude = 100;

	private float mapScaleWidth;
	private float mapScaleHeight;
	private float mapPositionX;
	private float mapPositionY;

	private float userLatitude;
	private float userLongitude;

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
		// !!! "orientation" IM HAUPTPROGRAMM NICHT BENÖTIGT !!!
		Screen.orientation = ScreenOrientation.Landscape;

		gameController = GameObject.Find("GameController");
		
		screenHeight = Screen.height;
		screenWidth = Screen.width;
		
		scaleWidth = screenWidth / 100;
		scaleHeight = screenHeight / 100;

		mapScaleHeight = (float)((screenHeight / 2) - ((60 * scaleHeight) / 2));
		mapScaleWidth = mapScaleHeight;
		mapPositionX = 0f;
		mapPositionY = 0f;
	}

	/*
	 * Aktion pro Frame durchführen
	 */
	void Update()
	{

	}

	/*
	 * Oberfläche pro Frame konstruieren
	 */
	void OnGUI()
	{
		GUI.DrawTexture (new Rect(mapPositionX, mapPositionY, mapScaleWidth, mapScaleHeight), imageMap);

		if (timeSinceLastUpdate >= timeBetweenUpdates && CheckGeoStatus())
		{
			userLatitude = Input.location.lastData.latitude;
			float userLatitudePosition = GetLatitudePosition(userLatitude);
			userLongitude = Input.location.lastData.longitude;
			float userLongitudePosition = GetLatitudePosition(userLongitude);

			GUI.DrawTexture (new Rect(0, 0, 0, 0), imageCurrentUser);
		}
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
		float latitudePosition = (mapScaleWidth * 
		                          ((mapMinLatitude - latitute) * 100) / 
		                          (mapMinLatitude - mapMaxLatitude) / 100);
		return latitudePosition;
	}

	/*
	 * Längengrad relativ zur Kartenskalierung zurückgeben
	 */
	float GetLongitudePosition(float longitude)
	{
		float longitudePosition = (mapScaleHeight * 
		                           ((mapMinLongitude - longitude) * 100) / 
		                           (mapMinLongitude - mapMaxLongitude) / 100);
		return longitudePosition;
	}
}