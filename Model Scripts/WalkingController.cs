using UnityEngine;
using System.Collections;

public enum FacingDirection {
	North,
	East,
	South,
	West
}



public class WalkingController : Controller {

	//movement information
	Vector3 walkVelocity;
	Vector3 prevWalkVelocity;
	FacingDirection facing = FacingDirection.South;
	float adjVertVelocity;
	float jumpPressTime;

	//settings
	public float walkSpeed = 3f;
	public float jumpSpeed = 6f;
	public float interactDuration = 0.1f;
	public float attackDamage = 5f;

	//delegates & events
	public delegate void FacingChangeHandler (FacingDirection fd);
	public static event FacingChangeHandler OnFacingChange;
	public delegate void HitboxEventHandler (float dur, float sec, ActionType act);
	public static event HitboxEventHandler OnInteract;

	protected override void Start(){
		base.Start ();
		if (OnFacingChange != null) {
			OnFacingChange (facing);
		}
	}

	public override void ReadInput (InputData data)
	{
		prevWalkVelocity = walkVelocity;
		ResetMovementToZero ();

		//set vertical movement
		if (data.axes[0] != 0f){
			walkVelocity += Vector3.forward * data.axes [0] * walkSpeed;
		}

		//set horizontal movement
		if (data.axes[1] != 0f){
			walkVelocity += Vector3.right * data.axes [1] * walkSpeed;
		}

		//set vertical jump
		if (data.buttons [0] == true) {
			if (jumpPressTime == 0f) {
				if (Grounded()) {
					adjVertVelocity = jumpSpeed;
				}
			}
			jumpPressTime += Time.deltaTime;
		} else {
			jumpPressTime = 0f;
		}

		//check if interact button is pressed
		if (data.buttons [1] == true) {
			if (OnInteract != null) {
				OnInteract (interactDuration, 0, ActionType.Interact);
			}
		}

		//check if attack button is pressed
		if (data.buttons [2] == true) {
			if (OnInteract != null) {
				OnInteract (interactDuration, attackDamage, ActionType.Attack);
			}
		}

		newInput = true;
	
	}

	//method that will look below our character and see if there is a collider
	bool Grounded(){
		return Physics.Raycast (transform.position, Vector3.down, coll.bounds.extents.y + 0.1f);
	}

	void LateUpdate(){
		if (!newInput) {
			prevWalkVelocity = walkVelocity;
			ResetMovementToZero ();
			jumpPressTime = 0f;
		}
		if (prevWalkVelocity != walkVelocity) {
			//check if there is a face change
			CheckForFacingChange();
		}
		rb.velocity = new Vector3(walkVelocity.x,  rb.velocity.y + adjVertVelocity, walkVelocity.z);
		newInput = false;
	}

	void CheckForFacingChange(){
		if (walkVelocity == Vector3.zero) {
			return;
		}
		if (walkVelocity.x == 0 || walkVelocity.z == 0) {
			//change our facing based on walkVelocity
			ChangeFacing(walkVelocity);
		} else {
			if (prevWalkVelocity.x == 0) {
				ChangeFacing (new Vector3 (walkVelocity.x, 0, 0));
			} else if (prevWalkVelocity.z == 0) {
				ChangeFacing (new Vector3 (0, 0, walkVelocity.z));
			} else {
				Debug.LogWarning ("Unexpected walkVelocity value.");
				ChangeFacing (walkVelocity);
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
		walkVelocity = Vector3.zero;
		adjVertVelocity = 0f;
	}

}
