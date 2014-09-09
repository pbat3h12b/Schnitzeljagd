using UnityEngine;
using System.Collections;

public class TouchMove : MonoBehaviour {

	// Update is called once per frame
	void Update ()
	{
		if(Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);
			float x = -2.4f + 4.8f * touch.position.x / Screen.width;
			float y = -4.43f + 8.86f * touch.position.y / Screen.height;

			transform.position = new Vector3(x,y,0);
		}
	}
}
