using UnityEngine;
using System.Collections;

public enum ActionType {
	Interact,
	Attack
}

[RequireComponent(typeof(BoxCollider))]
public class PlayerHitbox : MonoBehaviour {

	//collider movement
	float offset = 1f;
	BoxCollider col;

	//collider duration
	float duration;

	//secondary float value
	float secondary;

	//track action type
	ActionType action;

	void Awake(){
		WalkingController.OnFacingChange += RefreshFacing;
		WalkingController.OnInteract += StartCollisionCheck;
		col = GetComponent<BoxCollider> ();
		col.enabled = false;
		gameObject.layer = 8;
	}

	void Update(){
		if (col.enabled) {
			duration -= Time.deltaTime;
			if (duration <= 0) {
				col.enabled = false;
			}
		}
	}

	void StartCollisionCheck(float dur, float sec, ActionType act){
		action = act;
		col.enabled = true;
		duration = dur;
		secondary = sec;
	}

	void RefreshFacing(FacingDirection fd){
		switch (fd) {
		case FacingDirection.North:
			col.center = Vector3.forward * offset;
			break;
		case FacingDirection.East:
			col.center = Vector3.right * offset;
			break;
		case FacingDirection.West:
			col.center = Vector3.left * offset;
			break;
		default:
			col.center = Vector3.back * offset;
			break;
		}
	}

	void OnTriggerEnter(Collider col){
		if (action == ActionType.Interact && col.GetComponent<IInteractable>() != null) {
			col.GetComponent<IInteractable> ().Interact ();
		} else if (col.GetComponent<IAttackable>() != null) {
			col.GetComponent<IAttackable>().TakeDamage(secondary);
		}

	}

}
