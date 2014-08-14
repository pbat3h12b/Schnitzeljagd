using UnityEngine;
using System.Collections;

public class QRCode : MonoBehaviour {

    [HideInInspector]
    private GameObject gameController;

    public Texture2D Absenden;
    public Texture2D Zurueck;
    public Texture2D Weiter;

    GUIStyle styleWeiter = new GUIStyle();
    GUIStyle styleZurueck = new GUIStyle();
    GUIStyle styleAbsenden = new GUIStyle();

	string qrcode = "";

	// Use this for initialization
	void Start () {
        gameController = GameObject.Find("GameController");

        styleAbsenden.normal.background = Absenden;
        styleWeiter.normal.background = Weiter;
        styleZurueck.normal.background = Zurueck;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
        
        GUI.Label(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(30, 5, 40, 10)), "QR-Code Scanner");
        qrcode = GUI.TextField(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(30, 35, 20, 10)), qrcode);
        GUI.Label(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(35, 30, 20, 10)), "Bitte Code eingeben");
        if (GUI.Button(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(53, 35, 20, 10)), "",styleAbsenden))
        {
        }
        if (GUI.Button(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(25, 55, 20, 10)), "",styleZurueck))
        {
            Application.LoadLevel(4);
        }
        if (GUI.Button(gameController.GetComponent<PlayerInformation>().GetRelativeRect(new Rect(55, 55, 20, 10)), "", styleWeiter))
        {
            Application.LoadLevel(6);
        }
	}
}
