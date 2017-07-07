using UnityEngine;
using System.Collections;

public class BasicController : MonoBehaviour {

	public float speed = 6f;

	// Update is called once per frame
	void Update () {
		//look for input
		//move the character

		//if "up" button is pressed, move character away from camera;
		//ELSE if "down" button is pressed, move toward camera
		if (Input.GetKey(KeyCode.W)){
			transform.position += Vector3.forward * Time.deltaTime * speed;
		} else if (Input.GetKey(KeyCode.S)){
			transform.position += Vector3.back * Time.deltaTime * speed;
		}

		//if "left" button is pressed, move character left;
		//ELSE if "right" button is pressed, move right
		if (Input.GetKey(KeyCode.A)){
			transform.position += Vector3.left * Time.deltaTime * speed;
		} else if (Input.GetKey(KeyCode.D)){
			transform.position += Vector3.right * Time.deltaTime * speed;
		}
	}
}
