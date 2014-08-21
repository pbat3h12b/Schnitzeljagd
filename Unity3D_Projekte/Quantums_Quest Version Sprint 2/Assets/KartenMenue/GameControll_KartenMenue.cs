using UnityEngine;
using System.Collections;

public class GameControll_KartenMenue : MonoBehaviour {
	
	[HideInInspector]
	public Texture2D imageMap;
	public Texture2D imageCurrentUser;

	private float timeSinceChance = 0;

	private GameObject gameController;

    // new
    private Component staticScript;
	
	private int screenWidth;
	private int screenHeight;
	
	private double scaleWidth;
	private double scaleHeight;

    private float mapMinLatitude = 51.73299f;
    private float mapMinLongitude = 8.73370f;
    private float mapMaxLatitude = 51.72893f;
	private float mapMaxLongitude = 8.74004f;
	
    // new
    private Rect mapTransform;

	private float mapScaleWidth;
	private float mapScaleHeight;
	private float mapPositionX;
	private float mapPositionY;
	
	private float userLatitude;
	private float userLongitude;
	private float userLatitudePosition = 0;
	private float userLongitudePosition = 0;

    // new
    private Rect userPosition;
	
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
		
		screenHeight = Screen.width;
		screenWidth = Screen.height;
		
		scaleWidth = screenWidth / 100;
		scaleHeight = screenHeight / 100;
		
		mapScaleHeight = (float)(50 * scaleHeight);
		mapScaleWidth = mapScaleHeight;
		mapPositionX = (float)(50 * scaleWidth) - (mapScaleWidth / 2);
		mapPositionY = (float)(30 * scaleHeight) - (mapScaleHeight / 2);

        // new
        mapTransform = gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(0, 0, 0, 100));
        mapTransform.width = mapTransform.height;

        // new
        userPosition.width = mapTransform.width / 20;
        userPosition.height = mapTransform.height / 20;
	}
	
	/*
	 * Aktion pro Frame durchführen
	 */
	void Update()
	{
		timeSinceChance += Time.deltaTime;
	}
	
	/*
	 * Oberfläche pro Frame konstruieren
	 */
	void OnGUI()
	{
		GUI.DrawTexture (new Rect(mapTransform.x, 
                                    mapTransform.y, 
                                    mapTransform.width, 
                                    mapTransform.height), 
                                    imageMap);

		GUI.Label (new Rect(mapPositionX, mapPositionY - 50, mapScaleWidth, mapScaleHeight), errorMessage);

		if (timeSinceLastUpdate <= 0 && CheckGeoStatus())
		{
			Input.location.Start ();

			userLatitude = Input.location.lastData.latitude;
            userPosition.x = GetLatitudePosition(51.73090f);
			userLongitude = Input.location.lastData.longitude;
            userPosition.y = GetLongitudePosition(8.73631f);
			timeSinceLastUpdate = timeBetweenUpdates;

			Input.location.Stop ();
		}
		else
		{
			timeSinceLastUpdate -= Time.deltaTime;
		}

        GUI.DrawTexture(new Rect(userPosition.y,
                                  userPosition.x,
                                  userPosition.width,
                                  userPosition.height), imageCurrentUser);
		
		if (GUI.Button(new Rect((float)((screenWidth) - ((50 * scaleWidth))), 
		                        (float)((screenHeight) - (5 * scaleHeight) * 2), 
		                        (float)(50 * scaleWidth), 
		                        (float)(5 * scaleHeight)), buttonScanValue))
		{
			Application.LoadLevel(5);
		}

        GUI.Label(new Rect(mapPositionX, mapPositionY - 50, mapScaleWidth, mapScaleHeight), " "+userLongitude+" "+userLatitude+" ");
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
        Debug.Log(latitudePosition);
        latitudePosition = mapTransform.x + ((mapTransform.width * latitudePosition) / 100);
		latitudePosition = Mathf.Clamp (latitudePosition, mapTransform.x, mapTransform.x + mapTransform.width);
        latitudePosition -= userPosition.width / 2;
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
        Debug.Log(longitudePosition);
        longitudePosition = mapTransform.y + ((mapTransform.height * longitudePosition) / 100);
		longitudePosition = Mathf.Clamp (longitudePosition, mapTransform.y, mapTransform.y + mapTransform.height);
        longitudePosition -= userPosition.height / 2;
		return longitudePosition;
	}
}