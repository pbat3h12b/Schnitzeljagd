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

	//sobald auf das Rohr geklickt wird, wird die funktion "ChangePipePos" im playercontroller aufgerufen
	void OnMouseDown(){
		//das GameObject schickt sich selbst an die Funktion um bearbeitet zu werden.
		GameObject.Find("playerController").GetComponent<playerControl>().SendMessage("ChangePipePos", this.gameObject);
	}
}
