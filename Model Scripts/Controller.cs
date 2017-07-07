using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Controller : MonoBehaviour {

	//TODO: Add method ReadInput
	public abstract void ReadInput(InputData data);

	public bool presentWhenInactive = true;

	protected Rigidbody rb;
	protected Collider coll;
	protected Camera cam;
	protected bool newInput;

	void Awake(){
		rb = GetComponent<Rigidbody>();
		coll = GetComponent<Collider> ();
		cam = GetComponentInChildren<Camera> ();
	}

	protected virtual void Start(){
		if (InputManager.ins.controller != this) {
			Deactivate ();
		}
	}

	public virtual void Activate(){
		InputManager.ins.controller.Deactivate ();
		InputManager.ins.controller = this;
		cam.gameObject.SetActive (true);
		gameObject.SetActive (true);

	}

	public virtual void Deactivate(){
		cam.gameObject.SetActive (false);
		if (!presentWhenInactive) {
			gameObject.SetActive (false);
		}
	}
}
