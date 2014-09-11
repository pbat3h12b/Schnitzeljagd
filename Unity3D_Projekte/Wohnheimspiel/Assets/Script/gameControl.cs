using UnityEngine;
using System.Collections;
// ToDo: GUI Größenanpassung bearbeiten
//       Waypoints hinzufügen und Bewegungsmuster erweitern
//		 Sprites hinzufügen
//		 Objekte hinzufügen
public class gameControl : MonoBehaviour {
	//PUBLIC
	public Texture2D arrowUp;
	public Texture2D arrowDown;
	public Texture2D arrowLeft;
	public Texture2D arrowRight;
	//PRIVATE
	private Transform[] waypoints;
	
	//Variabeln für das Anpassen der GUI
	float nativeHeight = 1920.00f;
	float nativeWidth = 1280.00f;
	float screenHeight;
	float screenWidth;
	float rx;
	float ry;
	
	
	//Benötigte Variabeln zum bewegen
	int currentWaypoint = 0;
	Transform player;
	float speed = 6.0f;
	Vector3 targetPos;
	Vector3 moveDirection;
	Vector3 deltaPos;
	int selectedDirection = 0;
	int nextWaypoint = 0;
	
	//Benötigte Variabeln für Hindernisse
	bool isTrigger = false;
	
	//Benötigt für den Restart
	Vector3 startPosition;
	public GameObject Bomb;
	public GameObject Shear;
	public GameObject Hedge;
	public GameObject Rock;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").transform;
		GameObject parentGameObject = GameObject.FindGameObjectWithTag ("Waypoints");
		waypoints = parentGameObject.GetComponentsInChildren<Transform> ();
		//Die Startpositionen für den Restart
		startPosition = player.position;
		Instantiate (Bomb, GameObject.Find ("Waypoint5").transform.position,Quaternion.identity);
		Instantiate (Hedge,GameObject.Find ("Waypoint11").transform.position - new Vector3(1.7f,0.0f), Quaternion.identity);
		Instantiate (Shear,GameObject.Find ("Waypoint7").transform.position, Quaternion.identity);
		Instantiate (Rock,GameObject.Find ("Waypoint3").transform.position + new Vector3(1.0f,0.2f),Quaternion.identity);
		//Hier werden die Größen des Bildschirms ermittelt
		screenHeight = Screen.height;
		screenWidth = Screen.width;
		//Hier werden die Relationen zwischen der vordefinierten und der Tatsächlichen größe ermittelt
		rx = screenWidth / nativeWidth;
		ry = screenHeight / nativeHeight;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnGUI(){
		//Hier wird die größe des GUI-Rechtecks an die Größe des Bildschirms angepasst
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,new Vector3(rx,ry,1));
		
		//PFEIL-BUTTONS
		
		//Hier werden die Bewegungs-Buttons eingerichtet mit welchen der Spieler sich am Ende bewegt.
		//Rechts
		if(GUI.Button(new Rect(screenWidth / rx - screenWidth / 5,screenHeight / ry - screenHeight / 2, screenWidth / 5, screenHeight / 2 ), arrowRight)) {
			if(selectedDirection == 0){
				if(currentWaypoint == 0 || currentWaypoint == 2 || currentWaypoint == 3 || currentWaypoint == 8 || currentWaypoint == 9 || currentWaypoint == 10)
				{
					selectedDirection = 1;
					WalkRight();
				}
			}
		}
		//Links
		if(GUI.Button(new Rect(screenWidth / rx - screenWidth / 5 * 3,screenHeight / ry - screenHeight / 2, screenWidth / 5, screenHeight / 2), arrowLeft)) {
			if(selectedDirection == 0)
			{
				if(currentWaypoint == 3 || currentWaypoint == 4 || currentWaypoint == 6 || currentWaypoint == 10)
				{
					selectedDirection = 2;
					WalkLeft();
				}
			}
		}
		//Hoch
		if(GUI.Button(new Rect(screenWidth / rx - screenWidth / 5 * 2,screenHeight / ry - screenHeight / 2 * 2, screenWidth / 5, screenHeight / 2), arrowUp)) {
			if(selectedDirection == 0)
			{
				if(currentWaypoint == 3 || currentWaypoint == 5 || currentWaypoint == 6 || currentWaypoint == 4 || currentWaypoint == 9)
				{
					selectedDirection = 3;
					WalkUp();
				}
			}
		}
		//Runter
		if(GUI.Button(new Rect(screenWidth / rx - screenWidth / 5 * 2,screenHeight / ry - screenHeight / 2, screenWidth / 5, screenHeight / 2), arrowDown)) {
			if(selectedDirection == 0)
			{
				if(currentWaypoint == 1 || currentWaypoint == 2 || currentWaypoint == 4 || currentWaypoint == 7 || currentWaypoint == 8)
				{
					selectedDirection = 4;
					WalkDown();
				}
			}
		}
		
	}
	
	
	void WalkRight(){
		switch (currentWaypoint) {
		case 0:	
			nextWaypoint = 1;
			break;
		case 2:
			nextWaypoint = 3;
			break;	
		case 3:
			nextWaypoint = 4;
			break;
		case 8:
			nextWaypoint = 6;
			break;
		case 9:
			nextWaypoint = 10;
			break;
		case 10:
			nextWaypoint = 11;
			break;
		}
		StartCoroutine ("Walk");
	}
	
	void WalkLeft(){
		switch (currentWaypoint) {
		case 3:
			nextWaypoint = 2;
			break;
		case 4:
			nextWaypoint = 3;
			break;
		case 6:
			nextWaypoint = 8;
			break;
		case 10:
			nextWaypoint = 9;
			break;
		}
		StartCoroutine ("Walk");
		
	}
	
	void WalkUp(){
		switch (currentWaypoint) {
		case 3:	
			nextWaypoint = 1;
			break;
		case 4:
			nextWaypoint = 7;
			break;
		case 5:
			nextWaypoint = 2;
			break;
		case 6:
			nextWaypoint = 4;
			break;
		case 9:
			nextWaypoint = 8;
			break;
		}
		StartCoroutine("Walk");
		
	}
	
	void WalkDown(){
		switch (currentWaypoint) {
		case 1:
			nextWaypoint = 3;
			break;
		case 2:
			nextWaypoint = 5;
			break;
		case 4:
			nextWaypoint = 6;
			break;
		case 7:
			nextWaypoint = 4;
			break;
		case 8: 
			nextWaypoint = 9;
			break;
		}
		StartCoroutine ("Walk");
	}
	
	//Methode zur Fortbewegung
	IEnumerator Walk(){
		targetPos = waypoints [nextWaypoint].position;
		while (Vector3.Distance(waypoints[nextWaypoint].position, player.position) > Time.deltaTime * speed && isTrigger == false) {
			deltaPos = targetPos - player.position;
			moveDirection = deltaPos.normalized * speed;
			player.Translate (moveDirection * Time.deltaTime, Space.World);
			yield return new WaitForFixedUpdate(); 
		}
		if (isTrigger == false) {
			WaypointChange ();
		}
		else {
			targetPos = waypoints [currentWaypoint].position;
			while (Vector3.Distance(waypoints[currentWaypoint].position, player.position) > Time.deltaTime * speed) {
				deltaPos = targetPos - player.position;
				moveDirection = deltaPos.normalized * speed;
				player.Translate (moveDirection * Time.deltaTime, Space.World);
				yield return new WaitForFixedUpdate(); 
			}
			isTrigger = false;
			selectedDirection = 0;
		}
	}
	
	
	//Ändert den Wegpunkt je nach ausgewählter Richtung und Aktuellem Wegpunkt
	void WaypointChange(){
		switch (selectedDirection) {
			//Nach Rechts
		case 1:
			switch (currentWaypoint) {
			case 0:
				currentWaypoint = 1;
				break;
			case 2:
				currentWaypoint = 3;
				break;
			case 3:
				currentWaypoint = 4;
				break;	
			case 8:
				currentWaypoint = 6;
				break;
			case 9:
				currentWaypoint = 10;
				break;
			case 10:
				currentWaypoint = 11;
				break;
			}
			break;
			//Nach Links
		case 2: 
			switch (currentWaypoint) {
			case 3: 
				currentWaypoint = 2;
				break;
			case 4: 
				currentWaypoint = 3;
				break;
			case 6:
				currentWaypoint = 8;
				break;
			case 10:
				currentWaypoint = 9;
				break;
			}
			
			break;
			//Nach Oben
		case 3: 
			switch (currentWaypoint) {
			case 3:
				currentWaypoint = 1;
				break;
			case 4:
				currentWaypoint = 7;
				break;
			case 5:
				currentWaypoint = 2;
				break;
			case 6:
				currentWaypoint = 4;
				break;
			case 9:
				currentWaypoint = 8;
				break;
			}
			break;
			//Nach Unten
		case 4:
			switch (currentWaypoint) {
			case 1:
				currentWaypoint = 3;
				break;
			case 2:
				currentWaypoint = 5;
				break;
			case 4:
				currentWaypoint = 6;
				break;
			case 7:
				currentWaypoint = 4;
				break;
			case 8:
				currentWaypoint = 9;
				break;
			}
			break;
			
		}
		//Setzt die ausgewählte Richtung wieder auf 0 damit der Spieler sich weiterbewegen kann.
		selectedDirection = 0;
	}
	
	public void ChangeTrigger(){
		if (isTrigger == false)
			isTrigger = true;
		else
			isTrigger = false;
	}

	public void Restart(){
		currentWaypoint = 0;
		player.position = startPosition;
		Instantiate (Bomb, GameObject.Find ("Waypoint5").transform.position,Quaternion.identity);
		Instantiate (Hedge,GameObject.Find ("Waypoint11").transform.position - new Vector3(0.5f,0.0f), Quaternion.identity);
		Instantiate (Shear,GameObject.Find ("Waypoint7").transform.position, Quaternion.identity);
		Instantiate (Rock,GameObject.Find ("Waypoint3").transform.position + new Vector3(1.35f,0.2f),Quaternion.identity);
		}
}