using UnityEngine;
using System.Collections;

public class homeButtonControl : MonoBehaviour {

	
    //Bei klick auf das Icon wird das Hauptmenü angezeigt
    void OnMouseDown()
    {
        Application.LoadLevel("Menu");
    }
}
