using UnityEngine;
using System.Collections;

public class HintScript : MonoBehaviour {

	private GameObject gcScript;

	//Benötigte Variabeln für Hindernisse
	bool isBomb = false;
	bool isShear = false;

	//Ziel Erreicht
	bool isGoal = false;

	// Use this for initialization
	void Start () {
		gcScript = GameObject.FindGameObjectWithTag("GameController");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.name == "Rock(Clone)" && isBomb == false || other.name == "Hedge(Clone)" && isShear == false) {
			gcScript.SendMessage ("ChangeTrigger");
		}
		if (other.name == "Rock(Clone)" && isBomb == true || other.name == "Hedge(Clone)" && isShear == true) {
			GameObject.Destroy (other.gameObject);
		}
		if (other.name == "Bomb(Clone)") {
			isBomb = true;
			GameObject.Destroy(other.gameObject);
		}
		if (other.name == "Shear(Clone)") {
			isShear = true;
			GameObject.Destroy (other.gameObject);
		}
		if(other.name == "Waypoint11"){
			isGoal = true;
		}
		
	}


	//isGoal == True => Ende
	void OnGUI(){
		if (isGoal == true) {
			GUI.Label(new Rect(Screen.width /2,Screen.height / 2, Screen.width * 2, Screen.height * 2), "Gewonnen");
			//gcScript.SendMessage("Restart");

            //Score Submit
            if (GUI.Button(new Rect(Screen.width / -150, 0, 200, 100), "Score Abschicken"))
            {
                //Funktion Hüppi

            }
		
		}
	}
}
