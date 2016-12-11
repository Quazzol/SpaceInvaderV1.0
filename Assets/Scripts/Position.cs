using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

	private void OnDrawGizmos(){
		Gizmos.DrawSphere (transform.position, 1);
	}
}
