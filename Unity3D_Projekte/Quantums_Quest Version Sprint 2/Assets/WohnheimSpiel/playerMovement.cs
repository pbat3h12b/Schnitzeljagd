using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {
	public KeyCode leftKey = KeyCode.A;
	public KeyCode rightKey = KeyCode.D;
	public KeyCode topKey = KeyCode.W;
	public KeyCode bottomKey = KeyCode.S;
	public float speed = 500.0f;
	private Vector3 position;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		position = this.transform.position;
		if (Input.GetKey (leftKey)) {
			position.x -= Time.deltaTime * speed;
		}
		if (Input.GetKey (rightKey)) {
			position.x += Time.deltaTime * speed;
		} 
		if (Input.GetKey (topKey)) {
			position.y += Time.deltaTime * speed;
		}
		if (Input.GetKey (bottomKey)) {
			position.y -= Time.deltaTime * speed;
		}      
		this.transform.position = position;
	}

	public void movementUp(){
		position.y += Time.deltaTime * speed;
		this.transform.position = position;
		}
	public void movementDown(){
		position.y -= Time.deltaTime * speed;
		this.transform.position = position;
	}
	public void movementRight(){
		position.x += Time.deltaTime * speed;
		this.transform.position = position;
	}
	public void movementLeft(){
		position.x -= Time.deltaTime * speed;
		this.transform.position = position;
	}
	

}
