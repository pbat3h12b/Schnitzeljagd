using UnityEngine;
using System.Collections;

public class ProjectilController : MonoBehaviour {

	//Skript zur Kontrolle des Projektils
	//Erstellt von Fabian Meise am 23.8.2014

	#region Attribute
	//Bewegungsgeschwindigkeit
	public float speed=0.04f;
	//Um Sicher Zustellen, dass das Projectil außerhalb des
	//Bildschirms verschwindet wird es an den Rändern "zerstört"
	//oberer Rand des Bildschirms
	public float borderTop=5;
	//unterer Rand des Bildschirms
	public float borderbottom=-5;
	#endregion

	#region Methoden
	// Update is called once per frame
	void Update () {
		//Vorwärtsbewegung des Projectils
		transform.position = new Vector3 (transform.position.x,
		                                  transform.position.y + speed);
		Checkposition();
	}

	//Positionkontrollieren
	void Checkposition()
	{
		if (transform.position.y > borderTop)
		{
			Destroythis();
		}
		else if (transform.position.y < borderbottom)
		{
			Destroythis();
		}
	}

	//Gameobject wird zerstört
	void Destroythis()
	{
		GameObject.Destroy (this.gameObject);
	}
	#endregion
}