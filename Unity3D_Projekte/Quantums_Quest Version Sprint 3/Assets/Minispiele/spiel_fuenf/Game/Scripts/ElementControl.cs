using UnityEngine;
using System.Collections;

public class ElementControl : MonoBehaviour {

    //Position des Startpunktes des ELements
    private Vector3 spawnP;

    //Position des Endpunktes des Elements
    private Vector3 finishP;

	// Use this for initialization
	void Start () 
    {
        Spawn();
    }

    
    private void Spawn()
    {
        GetRandomSpawn();
        
       
        finishP.z = 10.0f;
    }
	
	// Update is called once per frame
	void Update () {
        
        Move(spawnP, finishP);
	}
	
    //Berechnet einen zufälligen Startpunkt für das Element
	public void GetRandomSpawn()
	{
		
		Vector3 spawnPosition = new Vector3 ();
		Camera cam = Camera.main;

		
		
		int spawnSite = Random.Range (1, 5);

        // 1 = top site | 2 = right site | 3 = bottom site | 4 = left site 
		switch (spawnSite)
		{
		case 1:
			spawnPosition.x = Random.Range(0f, Screen.width);
			spawnPosition.y = 0;
			break;
		case 2:
			spawnPosition.x = Screen.width;
			spawnPosition.y = Random.Range(0f, Screen.height);
			break;
		case 3:
			spawnPosition.x = Random.Range(0f, Screen.width);
			spawnPosition.y = Screen.height;
			break;
		case 4:
			spawnPosition.x = 0;
			spawnPosition.y = Random.Range(0f, Screen.height);
			break;
		}

        spawnPosition.z = 10.0f;

        spawnPosition = cam.ScreenToWorldPoint(spawnPosition);
        
        
		transform.position = spawnPosition;

        spawnP = spawnPosition;

        
        GetRandomFinish(spawnSite);

    }

    //Berechnet einen zufälligen Endpunkt für das Element, übergeben wird die Seite an dem der Startpunkt des Elements liegt
    private void GetRandomFinish(int spawnSite)
    {
        Vector3 finishPosition = new Vector3();
        Camera cam = Camera.main;


       
        int finishSite = Random.Range(1, 5);
        while (finishSite == spawnSite)
        {
            finishSite = Random.Range(1, 5);
        }   

        

         // 1 = top site | 2 = right site | 3 = bottom site | 4 = left site 
        switch (finishSite)
        {
            case 1:
                finishPosition.x = Random.Range(0f, Screen.width);
                finishPosition.y = 0;
                break;
            case 2:
                finishPosition.x = Screen.width;
                finishPosition.y = Random.Range(0f, Screen.height);
                break;
            case 3:
                finishPosition.x = Random.Range(0f, Screen.width);
                finishPosition.y = Screen.height;
                break;
            case 4:
                finishPosition.x = 0;
                finishPosition.y = Random.Range(0f, Screen.height);
                break;

            
        }

        finishPosition.z = 10.0f;
        finishPosition = cam.ScreenToWorldPoint(finishPosition);
        finishP = finishPosition;
 
    }

    //Ruft die Methode zur Zeitstrafe im SCript timeLine, beim Klick/Touch auf das Element, auf
	private void OnMouseDown()
	{
        GameObject.Find("TimeLine").GetComponent<timeLine>().punishment();
    }

    //Berechnung der Blickrichtung und der Bewegung des Elements
    void Move(Vector3 spawnPoint, Vector3 finishPoint)
    {
        float rotationSpeed = 6.0f;
        float speed = 6.0f;

        Quaternion rotation = Quaternion.LookRotation(finishPoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        Vector3 waypointDirection = finishPoint - transform.position;
        float speedElement = Vector3.Dot(waypointDirection.normalized, transform.forward);
        speed = speed + speedElement;
        transform.Translate(0, 0, Time.deltaTime * speed);

        //Abfrage ob der Endpunkt erreicht ist
        if (Vector3.Distance(finishP, transform.position) < Time.deltaTime * speed)
        {
            Spawn();
        }
    }
}



