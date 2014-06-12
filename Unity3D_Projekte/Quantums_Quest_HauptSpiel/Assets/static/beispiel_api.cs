using UnityEngine;
using System.Collections;

public class beispiel_api : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool logIn(string Username)
    {
        return true;
    }

    public bool[] getCaches(string Username)
    {
        return null;
    }

    public int[] getHighscors(string Username)
    {
        return null;
    }

    public void updateGeoData(float longitude, float latitude)
    {

    }

    public void updateCach(int ID)
    {

    }

    public void newScore(int SpielID, float score)
    {
    }

    public bool newRegistrierung(string Username, string Password, string Email)
    {
        return true;
    }

    public bool newLogBook(string text, int CachID)
    {
        return true;
    }
    
}
