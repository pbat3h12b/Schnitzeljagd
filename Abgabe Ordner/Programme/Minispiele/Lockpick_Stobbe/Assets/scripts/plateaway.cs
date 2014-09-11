using UnityEngine;
using System.Collections;

public class plateaway : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	//wenn auf eine Platte geklickt wird verschwindet sie dadurch, das ihre Schwerkraft an gestellt wird und sie aus dem Bildschirm fällt.
	void OnMouseDown(){
		this.rigidbody.useGravity = true;
	}
}
