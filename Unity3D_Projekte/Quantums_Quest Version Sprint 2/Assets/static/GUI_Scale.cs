using UnityEngine;
using System.Collections;

/// <summary>
/// Klasse erstell von Oliver Noll
/// </summary>

public class GUI_Scale : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Rect GetRelativeRect(Rect oldRect)
    {
        Rect newRect = new Rect();

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        newRect.width = (oldRect.width * screenWidth) / 100;
        newRect.height = (oldRect.height * screenHeight) / 100;

        newRect.x = (oldRect.x * screenWidth) / 100;
        newRect.y = (oldRect.y * screenHeight) / 100;

        return newRect;
    }
}
