using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour, IInteractable, IAttackable {

	void Awake(){
		gameObject.layer = 9;
	}

	#region IInteractable implementation

	public void Interact ()
	{
		if (transform.parent == null) {
			transform.parent = Camera.main.transform;
		} else {
			transform.parent = null;
		}
	}

	#endregion

	#region IAttackable implementation

	public void TakeDamage (float damage)
	{
		Destroy (gameObject);
	}

	#endregion


	 

}
