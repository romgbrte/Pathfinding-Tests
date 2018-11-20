using UnityEngine;
using System.Collections;

public class CoordButtonControls : MonoBehaviour {
	
	#region Variables
	const float zero = 0.0f;
	public static GameObject locationObject;
	public static Vector3 rayCastDirection = new Vector3(zero, zero, -1);
	public const int rayCastDistance = 10;
	#endregion
	
	
	void Update() {
		
		if(Input.GetMouseButtonUp(0) && !FriendScript.GetBeginState()) {
			PushButton();
		}
	
	}
	
	
	void PushButton() {
		Ray myRay = new Ray(this.gameObject.transform.position, rayCastDirection);
		RaycastHit hit;
		
		if(Physics.Raycast(myRay, out hit, rayCastDistance)) {
			locationObject = hit.transform.gameObject;
			//Debug.Log("Hit something...?");
			
			if(locationObject.tag == "xbutton") {
				//Debug.Log("click X");
				if(FriendScript.GetUserInputX() < 19) {
					//x = FriendScript.GetUserInputX();
					//x++;
					FriendScript.SetUserInputX(FriendScript.GetUserInputX() + 1);
					FriendScript.SetHUDgoalX(FriendScript.GetUserInputX());
				}
				else {
					FriendScript.SetUserInputX(1.0f);
					FriendScript.SetHUDgoalX(1.0f);
				}
				
			}
			else if(locationObject.tag == "zbutton") {
				//Debug.Log("click Z");
				if(FriendScript.GetUserInputZ() < 19) {
					//z = FriendScript.GetUserInputZ();
					//z++;
					FriendScript.SetUserInputZ(FriendScript.GetUserInputZ() + 1);
					FriendScript.SetHUDgoalZ(FriendScript.GetUserInputZ());
				}
				else {
					FriendScript.SetUserInputZ(1.0f);
					FriendScript.SetHUDgoalZ(1.0f);
				}
				
			}
		}
		else {
			Debug.Log("No raycast hit");
		}
	}
}
