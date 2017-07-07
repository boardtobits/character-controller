using UnityEngine;
using System.Collections;

public class VehicleController : Controller, IInteractable {

	//Vehicle stats
	public float maxSpeed = 6f;
	public float timeZeroToMax = 2.5f;
	public float timeMaxToZero = 6f;
	public float timeBrakeToZero = 1f;
	public float turnAnglePerSec = 90f;
	public float exitDistance = 2f;

	public GameObject passengerModel;

	//stats calculated at runtime
	float accelRatePerSec;
	float decelRatePerSec;
	float brakeRatePerSec;

	//current vehicle state
	float forwardVelocity;
	float currentTurn;
	bool accelChange;
	bool inReverse;

	//vehicle passenger information
	Controller exitController;


	protected override void Start(){
		base.Start ();
		accelRatePerSec = maxSpeed / timeZeroToMax;
		decelRatePerSec = -maxSpeed / timeMaxToZero;
		brakeRatePerSec = -maxSpeed / timeBrakeToZero;
		forwardVelocity = 0f;
		currentTurn = 0f;
		inReverse = false;
		passengerModel.SetActive (false);
		gameObject.layer = 9;
	}

	public override void ReadInput (InputData data)
	{
		//register and execute vehicle controls
		//steering
		if (data.axes[1] != 0){
			currentTurn = turnAnglePerSec * Time.deltaTime * (data.axes [1] > 0 ? 1 : -1);
		}

		//reverse
		if (data.axes [0] < 0) {
			inReverse = true;
		}

		//accelerate button
		if (data.buttons [0] == true) {
			Accelerate (accelRatePerSec);
		}

		//brake button
		if (data.buttons [1] == true) {
			Brake (brakeRatePerSec);
		}

		//exit button
		if (data.buttons [2] == true) {
			Exit();
		}

		newInput = true;
	}

	void LateUpdate(){
		//if moving, turn vehicle
		if (forwardVelocity != 0f) {
			rb.rotation = Quaternion.Euler (rb.rotation.eulerAngles + new Vector3 (0, currentTurn, 0));
		}

		//if no acceleration input, decelerate
		if (!accelChange) {
			Brake (decelRatePerSec);
		}

		//move vehicle based on current velocity
		rb.velocity = transform.forward * forwardVelocity;

		//reset for next frame
		accelChange = false;
		currentTurn = 0f;
		inReverse = false;
		newInput = false;
	}

	void Accelerate(float accel){
		float reverseFactor = inReverse ? -1 : 1;
		forwardVelocity += accel * Time.deltaTime * reverseFactor;
		forwardVelocity = Mathf.Clamp (forwardVelocity, -maxSpeed, maxSpeed);
		accelChange = true;
	}

	void Brake(float decel){
		if (forwardVelocity == 0) {
			accelChange = true;
			return;
		}
		float reverseFactor = Mathf.Sign (forwardVelocity);
		forwardVelocity = Mathf.Abs (forwardVelocity);
		forwardVelocity += decel * Time.deltaTime;
		forwardVelocity = Mathf.Max (forwardVelocity, 0) * reverseFactor;
		accelChange = true;
	}

	void Exit(){
		if (PlaceDriver ()) {
			exitController.Activate ();
		}
	}

	public override void Activate ()
	{
		exitController = InputManager.ins.controller;
		base.Activate ();
		passengerModel.SetActive (true);
	}

	public override void Deactivate ()
	{
		//deactivate the collider for one frame
		PauseCollider();
		base.Deactivate ();
		passengerModel.SetActive (false);
	}

	public void Interact(){
		Activate ();
	}

	bool PlaceDriver(){
		Vector3 potentialExitPoint = transform.position + transform.right * -exitDistance;
		if (Physics.OverlapBox (potentialExitPoint + Vector3.up, Vector3.one * 0.5f).Length > 0) {
			Debug.Log ("Obstacle in the way!");
			return false;
		}
		exitController.transform.position = potentialExitPoint + Vector3.up;
		return true;
	}

	IEnumerator PauseCollider(){
		GetComponent<Collider> ().enabled = false;
		yield return null;
		GetComponent<Collider> ().enabled = true;
	}

}
