using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public GameObject thePlayer;
	public float playerSpeed;
	public float jumpSpeed = 20.0f;
	public bool isGrounded;

	// Use this for initialization
	void Start () {
		isGrounded = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float hPos = Input.GetAxis("Horizontal");
		float vPos = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(hPos, 0.0f, vPos);

		//rigidbody.AddForce(movement * playerSpeed * Time.deltaTime);

		if(Input.GetKeyDown(KeyCode.Space) && isGrounded) {
			isGrounded = false;

		}
	}

	// lots of expensive calculations this way, should just transform/lock for 1-2 sec
	void OnCollisionStay(Collision collision) {
		if(collision.transform.tag == "floor") {
			isGrounded = true;
		}
	}

}
