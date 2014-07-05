using UnityEngine;
using System.Collections;

public class GameControll_KartenMenue : MonoBehaviour {
	
	[HideInInspector]
	public Texture2D imageMap;
	public Texture2D imageCurrentUser;

	private float timeSinceChance = 0;

	private GameObject gameController;
	
	private int screenWidth;
	private int screenHeight;
	
	private double scaleWidth;
	private double scaleHeight;
	
	private float mapMinLatitude = 51.72901f;
	private float mapMinLongitude = 8.73424f;
	private float mapMaxLatitude = 51.73263f;
	private float mapMaxLongitude = 8.73991f;
	
	private float mapScaleWidth;
	private float mapScaleHeight;
	private float mapPositionX;
	private float mapPositionY;
	
	private float userLatitude = 51.72901f;
	private float userLongitude = 8.73424f;
	private float userLatitudePosition = 0;
	private float userLongitudePosition = 0;
	
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
	}
	
	/*
	 * Aktion pro Frame durchführen
	 */
	void Update()
	{
		timeSinceChance += Time.deltaTime;

		if (timeSinceChance > 30) {
			Application.LoadLevel(5);
				}

	}
	
	/*
	 * Oberfläche pro Frame konstruieren
	 */
	void OnGUI()
	{
		GUI.DrawTexture (new Rect(mapPositionX, mapPositionY, mapScaleWidth, mapScaleHeight), imageMap);
		GUI.Label (new Rect(mapPositionX, mapPositionY - 50, mapScaleWidth, mapScaleHeight), errorMessage);

		if (timeSinceLastUpdate <= 0 && CheckGeoStatus())
		{
			Input.location.Start ();

			userLatitude = Input.location.lastData.latitude;
			userLatitudePosition = GetLatitudePosition(userLatitude);
			userLongitude = Input.location.lastData.longitude;
			userLongitudePosition = GetLatitudePosition(userLongitude);

			timeSinceLastUpdate = timeBetweenUpdates;

			Input.location.Stop ();
		}
		else
		{
			timeSinceLastUpdate -= Time.deltaTime;
		}
		
		GUI.DrawTexture (new Rect(userLongitudePosition, 
		                          userLatitudePosition, 
		                          mapScaleWidth / 20, 
		                          mapScaleHeight / 20), imageCurrentUser);
		
		if (GUI.Button(new Rect((float)((screenWidth) - ((50 * scaleWidth))), 
		                        (float)((screenHeight) - (5 * scaleHeight) * 2), 
		                        (float)(50 * scaleWidth), 
		                        (float)(5 * scaleHeight)), buttonScanValue))
		{
			Application.LoadLevel(5);
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
		float latitudePosition = mapPositionY + (mapScaleWidth * 
		                                         ((mapMinLatitude - latitute) * 100) / 
		                                         (mapMinLatitude - mapMaxLatitude) / 100);
		latitudePosition = Mathf.Clamp (latitudePosition, mapPositionX, mapPositionX + mapScaleWidth);
		return latitudePosition;
	}
	
	/*
	 * Längengrad relativ zur Kartenskalierung zurückgeben
	 */
	float GetLongitudePosition(float longitude)
	{
		float longitudePosition = mapPositionX + (mapScaleHeight * 
		                                          ((mapMinLongitude - longitude) * 100) / 
		                                          (mapMinLongitude - mapMaxLongitude) / 100);
		longitudePosition = Mathf.Clamp (longitudePosition, mapPositionY, mapPositionY + mapScaleHeight);
		return longitudePosition;
	}
}