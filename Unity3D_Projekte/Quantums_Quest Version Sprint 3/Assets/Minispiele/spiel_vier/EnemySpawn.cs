using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public partial class EnemySpawn : MonoBehaviour {

	//Erstellt von Florens Grabau
	Vector3 pos;
	Vector3 posOld;
	Quaternion rot;
	//GameObject für die Level 1 Gegner, benutzt in: SpawnL2Enemy();
	public GameObject enemyL1;
	//GameObject für die Level 2 Gegner, benutzt in: SpawnL2Enemy();
	public GameObject enemyL2;
	//GameObject für die Level 3 Gegner, benutzt in: SpawnL3Enemy();
	public GameObject enemyL3;
	//GameObject für das Parent Objekt der Gegner benutzt in: SpawnL1Enemy();
														    //SpawnL2Enemy();
															//SpawnL3Enemy();
	public GameObject parent;
	//Liste für die Gegner GameObjects
	public List<GameObject> blocks;
	//Variable die später dazu benutzt wird eine Random Zahl zu erzeugen 
	//die einer der Faktoren für das erzeugen der Gegner ist
	int rnd = 0;
	//Der Zähler um die Wahrscheinlichkeit der stärkeren Gegner zu verringern>
	//benutzt in initlevel()
	int counter = 0; 

	// Use this for initialization
	//Aufrufen von initlevel()
	void Start () {
	
		initLevel ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	//Erstellung einer Level 3 Gegnerwelle (1 sehr starker Gegner)
	//Wird von initlevel() aufgerufen
	public void SpawnL3Enemy(int y)
	{
		//Spalten
		for (int x=-1; x<0; x++) 
		{
			pos= parent.transform.position;
			posOld = parent.transform.position;
			pos.x = pos.x-2.5f;
			//rot.z = 180f;
			rot = parent.transform.rotation;
			parent.transform.position = pos;
			//parent.transform.rotation = rot;
			GameObject tempBlock = (GameObject)Instantiate 
				(enemyL3, new Vector3 (x * 2.5f, y*2.5f, 0), Quaternion.identity);
			//pos.y = Screen.height;
			//parent.gameObject.transform.position = pos;
			tempBlock.transform.parent = parent.gameObject.transform;
			blocks.Add (tempBlock);
			parent.transform.position = posOld;
			//pos = new Vector3(0,0,0);
		}
	}

	//Erstellung einer Level 2 Gegnerwelle (2 stärkere Gegner)
	//Wird von initlevel() aufgerufen
	public void SpawnL2Enemy(int y)
	{
		for (int x=-2; x<0; x++) 
		{
			// Level 2 Gegnerwelle generieren
			pos= parent.transform.position;
			posOld = parent.transform.position;
			pos.x = pos.x-3.8f;
			parent.transform.position = pos;
			GameObject tempBlock = (GameObject)Instantiate 
				(enemyL2, new Vector3 (x * 2.5f, y*2.5f, 0), Quaternion.identity);
			//pos.y = Screen.height;
			//parent.gameObject.transform.position = pos;
			tempBlock.transform.parent = parent.gameObject.transform;
			blocks.Add (tempBlock);
			parent.transform.position = posOld;
			//pos = new Vector3(0,0,0);
		}
	}

	//Erstellung einer Level 1 Gegnerwelle (5 schwache Gegner)
	//Wird von initlevel() aufgerufen
	public void SpawnL1Enemy(int y)
	{
		for (int x=-2; x<3; x++) 
		{
			pos= parent.transform.position;
			posOld = parent.transform.position;
			// Level 1 Gegnerwelle generieren
			GameObject tempBlock = (GameObject)Instantiate
									(enemyL1, new Vector3 (x * 2.5f, y*2.5f, 0), Quaternion.identity);
			//pos = this.gameObject.transform.position;
			//pos.x = Screen.width/2 ;
			//pos.y = Screen.height;
			//parent.gameObject.transform.position = pos;
			tempBlock.transform.parent = parent.gameObject.transform;
			blocks.Add (tempBlock);
			parent.transform.position = posOld;
			//pos = new Vector3(0,0,0);
		}
	}

	//Grundlegende Methode um die Gegnerwellen mithilfe von den Methoden
	//SpawnL1Enemy,SpawnL2Enemy und SpawnL3Enemy
	public void initLevel()
	{
		counter = 0;
		//Zeilenanzahl
		for (int y=4; y<8; y++) 
		{
			//Random Zahl um das Gegnerlevel zu bestimmen
			for(int x=1;x<5;x++)
			{
				rnd = Random.Range (1, 5);
			}
			//Sollten vorher drei oder mehr andere Wellen aufgetreten sein und die Random
			//Zahl gleich oder größer gleich 3 sein wird der Boss gespawnt und der Zähler für die anderen Wellen
			//wieder auf 0 gesetzt
				if (rnd >= 3 && counter >= 3) 
				{
					//Reihe aus einem Gegner
					SpawnL3Enemy(y);
					counter=0;
				}
			//Sollten vorher zwei schwächere Wellen aufgetreten sein und die Random
			//Zahl gleich 2 sein werden zwei stärkere Gegner gespawnt und der Zähler für die stärkste Welle
			//eins hochgezählt
				else if (rnd <= 2 && counter == 2) 
				{
					// Reihe aus drei etwas stärkeren Gegnern
					SpawnL2Enemy(y);
					counter++;
				}
			//Sollte keiner der anderen beiden Fälle auftreten wird der normale Gegner
			//gespawnt und der Zähler für die anderen beiden Wellen eins hochgezählt
				else
				{
					//Reihe aus fünf schwachen Gegnern
					SpawnL1Enemy(y);
					counter++;
				}
					// Block generieren
					//spawnL1Enemy();
					///spawnL2Enemy();
					//spawnL3Enemy();
					//GameObject tempBlock = (GameObject)Instantiate(enemy,new Vector3(3,2.0f,0),Quaternion.identity);
					//tempBlock.transform.parent = this.gameObject.transform;
					//blocks.Add(tempBlock);	
		}
	}
}
