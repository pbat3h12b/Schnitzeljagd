using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyControl : MonoBehaviour {
	//Erstellt von Florens Grabau
	//Variablendeklarierung
	public GameObject parent;
	float movementTimer = 1.0f;
	//Timer der dazu da ist den Spawnintervall der Gegner
	//zu bestimmen
	float spawnTimer = 1.0f;
	//Varibale um die Geschwindigkeit der Gegner zu bestimmen
	int speedY = 1;
	//Benutzt zur Positionsänderung des Gegners
	Vector3 position;
	float bottom = Screen.height - (Screen.height - 1);
	float top = Screen.height;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	//Ursprünglich waren die auskommentierten Zeilen dazu da ein stockendes Bewegen der Gegner
	//zu verursachen - allerdings hat dies dazu geführt das Projektile nicht
	//sofort verschwanden wenn sie auf einen Gegner trafen
		//movementTimer += Time.deltaTime;
		spawnTimer += Time.deltaTime;
		//position = parent.transform.position;
		//if (movementTimer > 1) 
		//{
		//	position.y -= speedY;
		//	parent.transform.position = position;
		//	movementTimer = 0;
		//}

		//Aus dem oben genannten Grund wurde die Bewegung der Gegner fließend gemacht
		position.y-=Time.deltaTime*speedY;
		parent.transform.position = position;
		//Timer um Zeit in Sekunden zwischen den Erstellungen der Wellen zu bestimmen
		if(spawnTimer > 18)
		{
			SpawnNewWave();
			spawnTimer = 0;
		}
	}

	//Erstellt die neue Welle von Gegnern indem es die Erstellungsfunktion initlevel()
	//der Methode EnemySpawn aufruft.
	void SpawnNewWave()
	{
		parent.GetComponent<EnemySpawn>().initLevel();
	}

}
