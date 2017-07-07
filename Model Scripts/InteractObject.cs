using UnityEngine;
using System.Collections;

public class InteractObject : MonoBehaviour, IInteractable {

	Material mat;
	// Use this for initialization
	void Start () {
		gameObject.layer = 9;
		mat = GetComponent<MeshRenderer> ().material;
	}

	public void Interact(){
		mat.color = Color.green;
	}

}
