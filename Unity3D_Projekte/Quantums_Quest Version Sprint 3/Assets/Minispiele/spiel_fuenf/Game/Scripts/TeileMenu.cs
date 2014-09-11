using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeileMenu : MonoBehaviour {

    //Aktueller Punkt für die Anzeige 
    private int spotNo = 1;

    //Positionen für die Anzeige der zu-/geangelten Elemente
    private Transform[] spots;


	// Use this for initialization
	void Start () 
    {
        spots = GameObject.FindGameObjectWithTag("spots").GetComponentsInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Überprüfung ob alle Elemente gesammelt wurden
        if (spotNo == 10)
        {
            GameObject.Find("GameManager").GetComponent<GameManagement>().GameEnded();
        }
	}

    //Liefert den Punkt an den das geangelte Element angezeigt wird
    public Transform getSpot()
    {
        Transform spot = spots[spotNo];
        spotNo++;
        return spot;
    }

    
    void OnMouseDown()
	{
        toggleMenuVisibility();
	}

    void OnMouseUp()
    {
        toggleMenuVisibility();
    }

	void OnMouseOver()
	{
        toggleMenuVisibility();
	}

    void OnMouseExit()
    {
        toggleMenuVisibility();
    }

    //Sichtbarkeit der Anzeiger der zu-/geangelten Elemente umstellen
    private void toggleMenuVisibility()
    {
        
    }
}
