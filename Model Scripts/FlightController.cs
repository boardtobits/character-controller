using UnityEngine;
using System.Collections;

public class FlightController : Controller {

	//movement information
	Vector3 flyVelocity;
	Vector3 prevFlyVelocity;
	FacingDirection facing = FacingDirection.South;

	//settings
	public float hSpeed = 3f;
	public float vSpeed = 2.5f;

	//delegates & events
	public delegate void FacingChangeHandler (FacingDirection fd);
	public static event FacingChangeHandler OnFacingChange;

	//exit controller
	Controller exitController;

	protected override void Start(){
		base.Start ();
		if (OnFacingChange != null) {
			OnFacingChange (facing);
		}
	}

	public override void ReadInput (InputData data)
	{
		prevFlyVelocity = flyVelocity;
		ResetMovementToZero ();

		//set vertical movement
		if (data.axes[0] != 0f){
			flyVelocity += Vector3.forward * data.axes [0] * hSpeed;
		}

		//set horizontal movement
		if (data.axes[1] != 0f){
			flyVelocity += Vector3.right * data.axes [1] * hSpeed;
		}

		//increase elevation
		if (data.buttons [0] == true) {
			flyVelocity += Vector3.up * vSpeed;
		}

		//decrease elevation
		if (data.buttons [1] == true) {
			flyVelocity += Vector3.up * -vSpeed * 1.5f;
		}

		//check exit button
		if (data.buttons [2] == true) {
			Exit ();
		}

		newInput = true;

	}

	void LateUpdate(){
		if (!newInput) {
			prevFlyVelocity = flyVelocity;
			ResetMovementToZero ();
		}
		if (prevFlyVelocity != flyVelocity) {
			//check if there is a face change
			CheckForFacingChange();
		}
		rb.velocity = flyVelocity;
		newInput = false;
	}

	void CheckForFacingChange(){
		if (flyVelocity == Vector3.zero) {
			return;
		}
		if (flyVelocity.x == 0 || flyVelocity.z == 0) {
			//change our facing based on walkVelocity
			ChangeFacing(flyVelocity);
		} else {
			if (prevFlyVelocity.x == 0) {
				ChangeFacing (new Vector3 (flyVelocity.x, 0, 0));
			} else if (prevFlyVelocity.z == 0) {
				ChangeFacing (new Vector3 (0, 0, flyVelocity.z));
			} else {
				Debug.LogWarning ("Unexpected walkVelocity value.");
				ChangeFacing (flyVelocity);
			}
		}
	}

	//NOTE: Method assumes only X OR Z value will be non-zero in dir parameter, will default to Z value
	void ChangeFacing(Vector3 dir){
		if (dir.z != 0) {
			facing = (dir.z > 0) ? FacingDirection.North : FacingDirection.South;
		} else if (dir.x != 0) {
			facing = (dir.x > 0) ? FacingDirection.East : FacingDirection.West;
		}

		//call change facing event
		if (OnFacingChange != null){
			OnFacingChange(facing);
		}

	}

	void ResetMovementToZero(){
		flyVelocity = Vector3.zero;
	}

	void Exit(){
		exitController.Activate ();
	}

	public override void Activate ()
	{
		exitController = InputManager.ins.controller;
		GetComponent<Rigidbody> ().useGravity = false;
		OnFacingChange += GetComponentInChildren<FacingSphere> ().RefreshFacing;
		base.Activate ();
	}

	public override void Deactivate ()
	{
		GetComponent<Rigidbody> ().useGravity = true;
		base.Deactivate ();
		Destroy (this);
	}

}
