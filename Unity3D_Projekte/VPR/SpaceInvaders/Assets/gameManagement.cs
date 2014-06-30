using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gameManagement : MonoBehaviour {

	public GameObject Block;

	private List<GameObject> blocks = new List<GameObject>();

	public float speedX;
	// Use this for initialization
	void Start () {
		speedX = -0.05f;
		initLevel ();
	}
	
	// Update is called once per frame
	void Update () {
		enemyMovement();
	}
		
	void initLevel(){
		for (int y = 10; y < 13; y++) {
			for (int x = -6; x < 6; x++) {
				GameObject tempBlock = (GameObject)Instantiate(Block,new Vector3(x*0.8f,y*0.8f,0),Quaternion.identity); //Quaternion hilft uns beim drehen
				tempBlock.tag = "Enemy";
				//if(y <5)
				//	tempBlock.GetComponent<BlockScript>().setBlockType(BlockScript.BlockType.Red);
				blocks.Add(tempBlock);
			}
		}
	}
	void enemyMovement(){
				foreach (GameObject b in blocks) {
						Vector3 position = b.transform.position;
						position.x += speedX;
						b.transform.position = position;

				}
		}

	public void enemyChange(){
		if (speedX < 0f) {
						speedX += 0.1f;
				}
		else if (speedX > 0f) {
						speedX -= 0.1f;
				}

		foreach (GameObject b in blocks) {
			Vector3 position = b.transform.position;
			position.y -=0.1f;
			position.x += speedX;
			b.transform.position = position;
		}

		}
}
