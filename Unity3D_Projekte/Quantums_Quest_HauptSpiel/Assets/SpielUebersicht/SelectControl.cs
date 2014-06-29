using UnityEngine;
using System.Collections;

public class SelectControl : MonoBehaviour {

	public Texture2D texture1;
	public Texture2D texture2;
	public Texture2D texture3;
	public Texture2D texture4;
	public Texture2D texture5;

	Vector2 scrollPosition = Vector2.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
 
	}

	void OnGUI()
	{
		scrollPosition = GUI.BeginScrollView(new Rect(100,100,Screen.width * 0.9f,400),scrollPosition,new Rect(0,0,500,850));

		GUI.Box (new Rect (50, 50, 420, 120), "");
		GUI.DrawTexture (new Rect (60, 60, 80, 100), texture1);
		GUI.Button (new Rect (360, 110, 100, 50), "Spielen");
		GUI.Label (new Rect (160, 60, 100, 50), "Spielname");
		GUI.Label (new Rect (160, 110, 100, 50), "Spielbar");

		GUI.Box (new Rect (50, 200, 420, 120), "");
		GUI.DrawTexture (new Rect (60, 210, 80, 100), texture2);
		GUI.Button (new Rect (360, 260, 100, 50), "Spielen");
		GUI.Label (new Rect (160, 210, 100, 50), "Spielname");
		GUI.Label (new Rect (160, 260, 100, 50), "Spielbar");

		GUI.Box (new Rect (50, 350, 420, 120), "");
		GUI.DrawTexture (new Rect (60, 360, 80, 100), texture3);
		GUI.Button (new Rect (360,410, 100, 50), "Spielen");
		GUI.Label (new Rect (160, 360, 100, 50), "Spielname");
		GUI.Label (new Rect (160, 410, 100, 50), "Spielbar");

		GUI.Box (new Rect (50, 500, 420, 120), "");
		GUI.DrawTexture (new Rect (60, 510, 80, 100), texture4);
		GUI.Button (new Rect (360,560, 100, 50), "Spielen");
		GUI.Label (new Rect (160, 510, 100, 50), "Spielname");
		GUI.Label (new Rect (160, 560, 100, 50), "Spielbar");

		GUI.Box (new Rect (50, 650, 420, 120), "");
		GUI.DrawTexture (new Rect (60, 660, 80, 100), texture5);
		GUI.Button (new Rect (360,710, 100, 50), "Spielen");
		GUI.Label (new Rect (160, 660, 100, 50), "Spielname");
		GUI.Label (new Rect (160, 710, 100, 50), "Spielbar");

		GUI.EndScrollView();
	}
}
