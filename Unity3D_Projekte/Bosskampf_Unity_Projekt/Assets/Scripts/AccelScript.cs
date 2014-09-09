using UnityEngine;
using System.Collections;

public class AccelScript : MonoBehaviour {
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(Input.acceleration.x * Time.deltaTime , Input.acceleration.y * Time.deltaTime,0 );
	}
}
