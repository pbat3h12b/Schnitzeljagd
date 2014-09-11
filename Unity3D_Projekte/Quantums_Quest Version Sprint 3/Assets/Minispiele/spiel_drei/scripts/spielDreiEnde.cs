using UnityEngine;
using System.Collections;

public class spielDreiEnde : MonoBehaviour {

	//Fabian Meise

	float timer = 0;
	

	// Update is called once per frame
	void Update () {
		 timer +=Time.deltaTime;
	}

	void OnGUI(){
        if (timer > 5)
        {
            //Score Submit
            if (GUI.Button(new Rect(Screen.width / -150, 0, 200, 100), "Score Abschicken"))
            {
                //Funktion Hüppi
                GameObject.Find("GameController").GetComponent<PlayerInformation>().newScore("Zukunftsmeile", 10000);
                if (GameObject.Find("GameController").GetComponent<PlayerInformation>().getGames()[1] == true &&
                    GameObject.Find("GameController").GetComponent<PlayerInformation>().getPuzzels()[1] == false)
                {
                    GameObject.Find("GameController").GetComponent<PlayerInformation>().markPuzzel();
                }
                Application.LoadLevel(3);
            }
        }
	}
}
