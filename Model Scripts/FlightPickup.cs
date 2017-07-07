using UnityEngine;
using System.Collections;

public class FlightPickup : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0f, Time.deltaTime * 30f, 0f));
	}

	void OnTriggerEnter(Collider c){
		if (c.tag == "Player" && c.GetComponent<FlightController>() == null) {
			FlightController fc = c.gameObject.AddComponent<FlightController> ();
			fc.Activate ();
			gameObject.SetActive (false);
		}

	}

}
