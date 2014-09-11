using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagement : MonoBehaviour {

    //
	public GameObject element;

    //
    public GameObject pickUpElement;

    //Liste der zu Angelnden Elemente
    [HideInInspector]
    public List<Object> aElemente;

    //Punkte
    [HideInInspector]
    public float score;

    //Aktueller Punkt für die Anzeige 
    private int spotNo = 1;

    //Positionen für die Anzeige der zu-/geangelten Elemente
    private Transform[] spots;
    
    

	// Use this for initialization
	void Start () 
    {
        aElemente = new List<Object>();
		InitGame ();
	}
	
	// Update is called once per frame
	void Update () 
    {
       
        //scaleScreen();
	}


    //Erzeugt jewweils 10 Elemente von: 
	void InitGame()
	{	 
		for (int i = 0; i < 10; i++) 
		{

            Instantiate(element);
            aElemente.Add(Instantiate(pickUpElement));
		}
	}


    public void GameEnded()
    {
        Time.timeScale = 0;
        Application.LoadLevel("GameOverMenu");
    }

    public void getScore() 
    {
        int maxPointsAtPercent = 85;

         float runTime = GameObject.Find("TimeLine").GetComponent<timeLine>().runTime; 
        
         int maxPointsAtTime = (int)((runTime / 100) * maxPointsAtPercent);           
         

        float timeLeft = GameObject.Find("TimeLine").GetComponent<timeLine>().startTime; 
        
        score = timeLeft / (maxPointsAtTime / 100);                              
        
    }

   

}
