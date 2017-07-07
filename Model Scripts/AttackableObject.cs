using UnityEngine;
using System.Collections;

public class AttackableObject : MonoBehaviour, IAttackable {

	Material mat;
	float totalHP = 50f;
	float currentHP;
	public Gradient hpColor;

	// Use this for initialization
	void Start () {
		gameObject.layer = 9;
		mat = GetComponent<MeshRenderer> ().material;
		currentHP = totalHP;
		AdjustColor ();
	}
	
	void AdjustColor(){
		mat.color = hpColor.Evaluate (currentHP / totalHP);
	}

	public void TakeDamage(float damage){
		currentHP -= damage;
		currentHP = Mathf.Max (currentHP, 0);
		AdjustColor ();
	}
}
