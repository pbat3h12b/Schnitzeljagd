using UnityEngine;
using System.Collections;

public class SendDamageCollider : MonoBehaviour {

	//Klasse zur Lebensverwaltung von Objekten
	//Erstellt von Fabian Meise am 23.8.2014
	//Zuletzt bearbeitet am 28.8.2014

	#region Attribute
	//Schadenswert der übergeben wird
	public float damageValue = 1;
	//Tagname von GameObject
	//Damit z.B. feindliche Gegner sich nicht selber abschießen
	public string tagName="player";
	#endregion

	#region Methoden
	//Es werden alle 3 Triggermethoden benutzt um mögliche Fehler
	//zu vermeiden

	//wird aufgerufen wenn ein der Collider eines anderen
	//Gameobjects den Trigger berührt
	//other ist das andere Gameobject
	void OnTriggerEnter(Collider other)
	{	
		//Aufruf von Senddamage
		Senddamage(other);
	}
	//wird aufgerufen wenn ein der Collider eines anderen
	//Gameobjects im Trigger befindet
	//other ist das andere Gameobject
	void OnTriggerStay(Collider other)
	{
		//Aufruf von Senddamage
		Senddamage(other);
	}
	//wird aufgerufen wenn ein der Collider eines anderen
	//Gameobjects den Trigger berührt
	//other ist das andere Gameobject
	void OnTriggerExit(Collider other)
	{
		//Aufruf von Senddamage
		Senddamage(other);
	}

	
	//Methode zum Senden von Schaden an Objekt
	void Senddamage(Collider other)
	{
		//Wenn der Collider nicht auf der selben Seite ist
		if(other.gameObject.tag!=tagName)
		{
		//Schadenssendung durch Aufrufen der Methode ApplyDamage
		other.gameObject.SendMessage("ApplyDamage",
		                             damageValue,
		                             SendMessageOptions.DontRequireReceiver);	
		}
		GameObject.Destroy (this.gameObject);
	}
	#endregion
}