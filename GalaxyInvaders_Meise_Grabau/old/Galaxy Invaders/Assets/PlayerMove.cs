using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	[SerializeField]
	//Wird noch kommentiert
	float horizontalLimit = 6, verticalLimit = 8, dragSpeed = .9f;
	
	Transform cashedTransform;
	
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
	
	void DragObject(Vector3 deltaPosition)
	{
		cashedTransform.position = new Vector3(Mathf.Clamp ((deltaPosition.x * dragSpeed) + cashedTransform.position.x,
		                                                    startingPos.x - horizontalLimit, startingPos.x + horizontalLimit), cashedTransform.position.y,
		                                       Mathf.Clamp((deltaPosition.y * dragSpeed) + cashedTransform.position.y,
		            										startingPos.y - verticalLimit, startingPos.y + verticalLimit));

	}
}