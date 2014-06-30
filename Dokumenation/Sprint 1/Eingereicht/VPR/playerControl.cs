using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour {

	public KeyCode leftKey = KeyCode.A;
	public KeyCode rightKey = KeyCode.D;
	public float speed = 9.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//gibt an bis wohin sich die Position y verändern darf
		Vector3 position = this.transform.position;
		position.x = Mathf.Clamp (position.x, 0.13f, 0.95f);
		if (Input.GetKey (leftKey)) {
			position.x -= Time.deltaTime * speed;
		}
		if (Input.GetKey (rightKey)) {
			position.x += Time.deltaTime * speed;
		}        
		this.transform.position = position;
	}

}
