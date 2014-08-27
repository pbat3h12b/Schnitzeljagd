using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {

	public Texture2D switchOn;
	public Texture2D switchOff;
	public bool switchState;
	public int id;

	private CommandManager cm;

	// Use this for initialization
	void Start () {
		renderer.material.SetTexture ("_MainTex", switchOff);
		switchState = false;
		cm = GameObject.FindGameObjectWithTag("GameController").GetComponent<CommandManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ToggleSwitch()
	{
		cm.ChangeSwitch(id);

		if(switchState)
		{
			renderer.material.SetTexture ("_MainTex", switchOn);
		}
		else
		{
			renderer.material.SetTexture ("_MainTex", switchOff);
		}
	}

	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(0))
			ToggleSwitch();
	}
}
