using UnityEngine;
using System.Collections;

public class Shredder : MonoBehaviour {

	public void OnCollisionEnter2D(Collision2D col){
		Destroy (col.gameObject);
	}
}
