using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour {

	//public GameObject block;
	//public Vector3 lastPipe;
	bool storeLastPlate = false;
	GameObject selectedPipe;
	GameObject changingPipe;
	// Use this for initialization
	void Start () {
	
	}

    void OnGUI()
    {
        if (GUI.Button(GameObject.Find("GameController").GetComponent<GUI_Scale>().GetRelativeRect(new Rect(0, 0, 20, 15)), "kill"))
        {
            Application.Quit();
        }
    }

	// Update is called once per frame
	void Update () {

		//if (Input.GetMouseButtonDown (0)) {
						/*Vector3 pz = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						RaycastHit hit;
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						if (Physics.Raycast (ray)) {
								//Instantiate(block, pz, transform.rotation);
						}*/
				//}

		//if (Input.GetMouseButtonUp (0)) {

				//}

		/* 
		foreach(Touch touch in Input.touches){
			if (touch.phase == TouchPhase.Began){
				Instantiate(block);
			}
		}

		 * Der Versuch eine Steuerung in C# zu haben.
		 * Vector3 v3;
		
		if (Input.touchCount != 1) {
			dragging = false; 
			return;
		}
		
		Touch touch = Input.touches[0];#ä
		Vector3 pos = touch.position;
		for (var touch : Touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				// Construct a ray from the current touch coordinates
				var ray = Camera.main.ScreenPointToRay (touch.position);
				if (Physics.Raycast (ray)) {
					// Create a particle if hit
					Instantiate(particle, transform.position, transform.rotation);
				}
			}
		}

		if(touch.phase == TouchPhase.Began) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(pos); 
			if(Physics.Raycast(ray, out hit) && (hit.collider.tag == "Draggable"))
			{
				Debug.Log ("Here");
				toDrag = hit.transform;
				dist = hit.transform.position.z - Camera.main.transform.position.z;
				v3 = new Vector3(pos.x, pos.y, dist);
				v3 = Camera.main.ScreenToWorldPoint(v3);
				offset = toDrag.position - v3;
				dragging = true;
			}
		}
		 if (dragging && touch.phase == TouchPhase.Moved) {
			v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
			v3 = Camera.main.ScreenToWorldPoint(v3);
			toDrag.position = v3 + offset;
		}
		if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)) {
			dragging = false;
		}*/

		/*
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				//if(touch.position )
				//{
				//
				//}
			}
		}*/
	}

	public void ChangePipePos(/*Vector3 platePos, */GameObject pipe){
		if (storeLastPlate == false) {
			//lastPipe = platePos;
			selectedPipe = pipe;
			storeLastPlate = true;
		} 
		else {
			changingPipe = pipe;
			Vector3 pipePos = selectedPipe.transform.position;
			selectedPipe.transform.position = changingPipe.transform.position;
			changingPipe.transform.position = pipePos;
			storeLastPlate = false;
		}
	}
}
