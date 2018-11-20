using UnityEngine;
using System.Collections;

public class CamSwitch : MonoBehaviour {
	
	public Camera maincam;
	public Camera topdown;
	
	// Use this for initialization
	void Start () {
		maincam.enabled = true;
		topdown.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyUp(KeyCode.C)) {
			maincam.enabled = !maincam.enabled;
			topdown.enabled = !topdown.enabled;
		}
	
	}
}
