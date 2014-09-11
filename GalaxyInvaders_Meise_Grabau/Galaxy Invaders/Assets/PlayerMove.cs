using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	[SerializeField]
	//Erstellt von Florens Grabau
	//Zuletzt verändert am: 08.09.2014
	//Variablendeklarierung
	float horizontalLimit = 6, verticalLimit = 8, dragSpeed = .1f;
	
	Transform cashedTransform;
	//Startposition des Spielers
	Vector3 startingPos;
	
	// Use this for initialization
	void Start () 
	{
		cashedTransform = transform;
		startingPos = cashedTransform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.touchCount > 0)
		{
			Vector2 deltaPosition = Input.GetTouch (0).deltaPosition;
			//Switch Anweisung die prüft wie weit die Touch Phase fortgeschritten ist, also
			//prüft ob der Spieler das Objekt wirklich mit dem Finger bewegt oder nur
			//einmal auf den Bilschirm klickt
			switch(Input.GetTouch(0).phase)
			{
			case TouchPhase.Began:
				break;
				
			case TouchPhase.Moved:
				DragObject(deltaPosition);
				break;
				
			case TouchPhase.Ended:
				break;
			}
		}
	}
	
	// Methode die aufgerufen wird um das Objekt zu bewegen 
	//wennn der Finger über das Spielfeld gezogen wird.
	void DragObject(Vector3 deltaPosition)
	{
		//Berechnung um das Objekt zu bewegen
		//Die Berechnung besteht aus zwei Mathf.Clamps um die 
		//Positionsveränderung und die minimalen/maximalen X/Y Positionen
		//zu denen das Objekt gezogen werden darf zu bestimmen
		cashedTransform.position = new Vector3
											  (Mathf.Clamp ((deltaPosition.x * dragSpeed) + cashedTransform.position.x,
		                                                    startingPos.x - horizontalLimit, startingPos.x + horizontalLimit), 
		                                       				cashedTransform.position.y,
		                                       Mathf.Clamp((deltaPosition.y * dragSpeed) + cashedTransform.position.y,
		            										startingPos.y - verticalLimit, startingPos.y + verticalLimit));
		//Die einzelnen Mathf.Clamps erklärt:
		//Mathf.Clamp((Positionsveränderung des Objektes,
		//minimale Position zu der sich das Objekt bewegen darf,
		//maximale Position zu der sich das Objekt bewegen darf)
	}
}