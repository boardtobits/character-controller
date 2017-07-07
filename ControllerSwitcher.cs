using UnityEngine;
using System.Collections;

public class ControllerSwitcher : MonoBehaviour {

	public Controller walker;
	public Controller vehicle;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.V)) {
			if (InputManager.ins.controller == walker) {
				vehicle.Activate();
			} else {
				walker.Activate();
			}
		}
	}
}
