using UnityEngine;
using System.Collections;

public class pipeControl : MonoBehaviour {

	//Vector3 pipePos;
	//GameObject script;

	// Use this for initialization
	void Start () {

		//script = GameObject.Find ("playerComtroller").GetComponent<playerControl>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown(){
		GameObject.Find("playerController").GetComponent<playerControl>().SendMessage("ChangePipePos", this.gameObject);
	}
}
