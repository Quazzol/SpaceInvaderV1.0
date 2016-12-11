using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
	public float damage = 100f;

	public void Hit(){
		Destroy (gameObject);
	}

	public float GetDamage(){
		return damage;
	}
}
