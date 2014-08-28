using UnityEngine;
using System.Collections;

public class Regulator : MonoBehaviour {
	
	public Camera camera;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void Movement()
	{

		Vector3 position = transform.position;

		if (Input.touchCount == 1)
		{
			position = camera.ScreenToWorldPoint(Input.touches[0].position);
			Debug.Log(camera.ScreenToWorldPoint(Input.touches[0].position));
			position.x = 1.938336f;
			position.z = -1;
			position.y = Mathf.Clamp (position.y, -3.755138f, 2.190039f);
		}
			
		transform.position = position;
	}

	void OnMouseOver()
	{
		Movement ();
	}
}
