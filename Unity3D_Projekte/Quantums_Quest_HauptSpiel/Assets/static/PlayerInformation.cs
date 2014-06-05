using UnityEngine;
using System.Collections;

public class PlayerInformation : MonoBehaviour {

    private static string userName;
    private static float longitude;
    private static float latitude;
    private static bool[] caches = {false,false,false,false,false,false};


	// Use this for initialization
	void Start () {
        Input.location.Start();
	}
	
	// Update is called once per frame
	void Update () {

        longitude = Input.location.lastData.longitude;
        latitude = Input.location.lastData.latitude;

	}

    void LogIn()
    { 

    }
}

