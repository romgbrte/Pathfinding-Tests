using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	#region Variables
	private float screenHeight,
				  screenWidth,
				  labelHeight,
				  labelWidth;
	static string projectInfo = "Spring 2015   --   Robert Rabel   --   Artificial Intelligence Pathfinding";
	static GUIStyle goalText = new GUIStyle();
	static GUIStyle pathText = new GUIStyle();
	static GUIStyle algoText = new GUIStyle();
	static GUIStyle projText = new GUIStyle();
	#endregion
	
	// Use this for initialization
	void Start () {
		screenHeight 	= Screen.height;
		screenWidth 	= Screen.width;
		labelHeight 	= screenHeight * 0.1f;
		labelWidth 		= screenWidth * 0.4f;
		goalText.fontSize	= 20;
		algoText.fontSize	= 20;
		pathText.fontSize	= 20;
		projText.fontSize	= 10;
		goalText.alignment	= TextAnchor.MiddleLeft;
		algoText.alignment	= TextAnchor.MiddleCenter;
		pathText.alignment	= TextAnchor.MiddleRight;
		projText.alignment	= TextAnchor.MiddleCenter;
		goalText.normal.textColor	= Color.green;
		algoText.normal.textColor	= Color.yellow;
		pathText.normal.textColor	= Color.cyan;
		projText.normal.textColor	= Color.white;
	}
	
	public void OnGUI() {
		GUI.Label(new Rect((Screen.width * 0.45f) - (labelWidth * 0.5f), // loc x
		                   (Screen.height * 0.05f) - (labelHeight * 0.5f), // loc y
		                   labelWidth, labelHeight),
		                   "Starting Point: (" + (int)FriendScript.HUDstart.x + ", " + (int)FriendScript.HUDstart.z + ")" + 
		                   "\nNext Goal: (" + (int)FriendScript.HUDgoal.x + ", " + (int)FriendScript.HUDgoal.z + ")",
		                   goalText);
		                   
		GUI.Label(new Rect((Screen.width * 0.5f) - (labelWidth * 0.5f), // loc x
		                   (Screen.height * 0.05f) - (labelHeight * 0.5f), // loc y
		                   labelWidth, labelHeight),
		                   "Algorithm: " + FriendScript.algorithmLabel + "\nDegree: " + FriendScript.degreeLabel + "\nWalls: " + FriendScript.wallLabel,
		                   algoText);
		
		GUI.Label(new Rect((Screen.width * 0.55f) - (labelWidth * 0.5f), // loc x
		                   (Screen.height * 0.05f) - (labelHeight * 0.5f), // loc y
		                   labelWidth, labelHeight),
		                   "Steps to goal: " + FriendScript.pathCount + "\nTotal Searched: " + FriendScript.totalConsidered + "\nTime taken: " + FriendScript.timeElapsed,
		                   pathText);
		                   
		GUI.Label(new Rect((Screen.width * 0.5f) - (labelWidth * 0.5f), // loc x
		                   (Screen.height * 0.99f) - (labelHeight * 0.5f), // loc y
		                   labelWidth, labelHeight),
		                   projectInfo,
		                   projText);
	}
}
