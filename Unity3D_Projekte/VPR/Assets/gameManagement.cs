using UnityEngine;
using System.Collections;

public class gameManagement : MonoBehaviour {
	// Use this for initialization
	private bool isVictory = false;
	public playerMovement player;
	[HideInInspector]
	private int screenWidth;
	private int screenHeight;
	private double scaleWidth;
	private double scaleHeight;
	// Use this for initialization
	void Start()
	{
		screenHeight = Screen.height;
		screenWidth = Screen.width;
		
		scaleWidth = screenWidth / 100;
		scaleHeight = screenHeight / 100;
	}
	

	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void ChangeState(){
		Time.timeScale = 0;
		this.isVictory = true;
		OnGUI ();
	}
	
	void OnGUI(){
		if (isVictory == true) {
			GUI.Box (new Rect (0, 0, 300, 300), "Sie haben das Spiel gewonnen!");
		}
		
		if (GUI.Button(new Rect((float)((screenWidth) - ((10 * scaleWidth))), (float)((screenHeight / 2) + (20 * scaleHeight)), (float)(10 * scaleWidth), (float)(5 * scaleHeight)),"Hoch"))
		{
			player.movementUp();
		}
		if (GUI.Button(new Rect((float)((screenWidth) - ((10 * scaleWidth))), (float)((screenHeight / 2) + (30 * scaleHeight)), (float)(10 * scaleWidth), (float)(5 * scaleHeight)),"Runter"))
		{
			player.movementDown();
		}
		if (GUI.Button(new Rect((float)((screenWidth) - ((10 * scaleWidth))), (float)((screenHeight / 2) + (40 * scaleHeight)), (float)(10 * scaleWidth), (float)(5 * scaleHeight)),"Rechts"))
		{
			player.movementRight();
		}
		if (GUI.Button(new Rect((float)((screenWidth) - ((10 * scaleWidth))), (float)((screenHeight / 2) + (50 * scaleHeight)), (float)(10 * scaleWidth), (float)(5 * scaleHeight)),"Links"))
		{
			player.movementLeft();
		}
	}
}