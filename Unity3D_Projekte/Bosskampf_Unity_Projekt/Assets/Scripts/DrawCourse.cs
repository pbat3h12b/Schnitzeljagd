using UnityEngine;
using System.Collections;

public class DrawCourse : MonoBehaviour {

	public Texture course;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), course);
	}
}
