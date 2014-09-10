using UnityEngine;
using System.Collections;

public class pipeCreator : MonoBehaviour {


	//public List<GameObject> pipes; <- array ist besser, da ich sowieso eine feste größe habe.
	public GameObject[] pipePrefabs = new GameObject[6];
	public GameObject platePrefab;
	//Die Abkuerzungen sind dazu da um anzuzeigen was verbunden wird. Dabei ist: L = Left, R = Right, T = Top, B = Bottom
	//0 = LR
	//1 = TB
	//2 = TL
	//3 = TR
	//4 = LB
	//5 = RB

	//das spielfeld
	public int[,] pipes = new int[5,5];

	//
	int[] allocate = new int[6];

	// Use this for initialization
	void Start () {

		//der Versuch die Bildgröße auf die Bildschirmgröße anzupassen
		//Camera cam = Camera.main;

		//uhrsprünglich sollte das Vertauschen durch drag&drop gehen.
		/*
		for(int i = 0; i < 5; i++)
		{
			//pipePrefabs[i].tag = "Draggable";
		}*/

		Vector3 position;
		position.x = -2.0f;
		position.y = -2.0f;
		position.z = -2.0f;
		for (int row = 0; row < 5; row++) {//row = Reihe
			for (int col = 0; col < 5; col++) {//col = Spalte
				for(int rand = 0; rand <1; rand++)
				{
					int randPrefab = Random.Range(0, 5);
					if(allocate[randPrefab] != 5)
					{
						pipes[row,col] = randPrefab;
						allocate[randPrefab]++;
					}

					else
					{
					rand--;
					}
				}

				// hier musste ich mir die rotation des objekts holen. "Quaternion.identity" funktioniert hier nicht.
				Instantiate(pipePrefabs[pipes[row,col]], position, pipePrefabs[pipes[row,col]].transform.rotation);

				//eigene Position für die Platten, da sie über den Rohren liegen müssen
				Vector3 platePos = position;
				platePos.z = position.z -4;
				Instantiate(platePrefab, platePos, platePrefab.transform.rotation);
				position.x++;
			}
			position.x = -2.0f;
			position.y++;
		}
		position.x = -2.0f;
		position.y = -2.0f;

	}
	
	// Update is called once per frame
	void Update () {

	}
}
