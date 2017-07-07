using UnityEngine;
using System.Collections;

public class SceneFill : MonoBehaviour {

	public Transform obsPrefab;
	public int obsCount = 50;

	// Use this for initialization
	void Start () {
		FillScene ();
	}

	void FillScene(){
		int oc = obsCount;
		while (oc > 0) {
			//populate our obstacles
			float x = Random.value * 100f - 50f;
			float z = Random.value * 100f - 50f;

			if (x > -5f && x < 5f && z > -5f && z < 5f)
				continue;

			Quaternion rot = Quaternion.Euler (0f, Random.value * 360f, 0f);
			Instantiate (obsPrefab, new Vector3 (x, 1, z), rot, transform);

			oc--;
		}
	
	}


}
