using UnityEngine;
using System.Collections;

public class timer : MonoBehaviour {

	//startwerte
	int score = 10000;
	float XlengthTime = 6f;


	// Use this for initialization
	void Start () {

		//hier wird das Warten einmalig gestartet
		StartCoroutine(WaitTimer());
	}
	
	// Update is called once per frame
	void Update () {
		//hier wird der Zeitbalken verkürzt um die verbliebene Zeit anzuzeigen
		transform.localScale = new Vector3(XlengthTime,0.4f,1f);
	}

	//diese Funktion sollte später mal den Score an die Datenbank übergeben
	public int solved(){
		XlengthTime = 0;
		return score;
		}

	//hier ruft sich das warten immer wieder selbst auf
	IEnumerator WaitTimer() {
		XlengthTime -= 0.1f;
		score -= 300;

		//wartet und geht 5mal in der Sekunde einen Schritt weiter.
		yield return new WaitForSeconds(0.2f);

		//wenn der Timer auf 0 ist wird nicht mehr weiter gewartet/gezählt
		if (XlengthTime > 0) {
						StartCoroutine (WaitTimer ());
				} 
		else {
			XlengthTime = 0;
				}
	}
}
