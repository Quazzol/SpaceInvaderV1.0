using UnityEngine;
using System.Collections;

public class EnemyFormation : MonoBehaviour {
	public GameObject enemyPrefab;
	public float speed = 5f;
	public float width = 10f;
	public float height = 5f;
	public float spawnDelay = 0.5f;

	private bool isMovingRight;
	private float xMin, xMax;

	private void Start () {
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint (new Vector3(0,0, distanceToCamera));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint (new Vector3(1,0, distanceToCamera));
		xMax = rightBoundary.x;
		xMin = leftBoundary.x;
		spawnEnemies ();
	}
	
	private void Update () {
		if (isMovingRight)
			transform.position += Vector3.right * speed * Time.deltaTime;
		else
			transform.position += Vector3.left * speed * Time.deltaTime;
	
		float rightEdgeOfFormation = transform.position.x + (0.5f * width);
		float leftEdgeOfFormation = transform.position.x - (0.5f * width);

		if (leftEdgeOfFormation < xMin)
			isMovingRight = true;
		else if (rightEdgeOfFormation > xMax)
			isMovingRight = false;
	
		if (allMembersDead ())
			spawnUntilFull ();

	}

	public void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
	}

	private void spawnEnemies(){
		foreach (Transform child in transform){
			GameObject enemy = Instantiate (enemyPrefab, child.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}
	}

	private Transform nextFreePosition(){
		foreach (Transform childTransform in transform) {
			if (childTransform.childCount == 0)
				return childTransform;
		}
		return null;
	}

	private void spawnUntilFull(){
		Transform freePos = nextFreePosition ();
		if(freePos){
			GameObject enemy = Instantiate (enemyPrefab, freePos.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePos;
		}
		if(nextFreePosition())
			Invoke ("spawnUntilFull",spawnDelay);
	}

	private bool allMembersDead(){
		foreach(Transform childTransform in transform){
			if (childTransform.childCount > 0)
				return false;
		}
		return true;
	}
		
}
