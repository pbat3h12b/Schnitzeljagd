using UnityEngine;
using System.Collections;

public class plateaway : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown(){
		this.rigidbody.useGravity = true;
	}
}
