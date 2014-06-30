using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {

	public gameManagement management;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("Trigger Enter");
				if (other.gameObject.tag == "Enemy") {
						management.enemyChange ();
				}
		}
}
