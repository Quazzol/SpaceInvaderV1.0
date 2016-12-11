using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour {
	private ScoreKeeper scoreKeeper;

	public float health = 150;
	public GameObject laserPreFab;
	public float laserSpeed = 5f;
	public float shotsPerSeconds = 0.5f;
	public int scoreValue = 150;
	public AudioClip fireSound, deadSound;


	public void Start(){
		scoreKeeper = GameObject.Find ("Score").GetComponent<ScoreKeeper> ();
	}

	private void FixedUpdate(){
		float probability = Time.deltaTime * shotsPerSeconds;
		if(Random.value < probability)
			fire ();
	}

	public void setEnemyInfo(int _level){
		health = 150f + (_level * 50);
		shotsPerSeconds = 0.5f + (_level * 0.01f);
		scoreValue = 150  + (_level * 50);
		PlayerControl.isLevelUp = false;
	}
		

	public void OnCollisionEnter2D(Collision2D col){
		Laser missile = col.gameObject.GetComponent<Laser> ();
		if (missile) {
			missile.Hit ();
			print ("1EnemyHealth : " + health);
			health -= missile.GetDamage ();
			print ("2EnemyHealth : " + health);
			if (health <= 0)
				die ();
		}
	}
		
	private void fire(){
		GameObject laser = Instantiate (laserPreFab, transform.position, Quaternion.identity) as GameObject;
		laser.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, -laserSpeed,0);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
	}
		
	private void die(){
		Destroy (gameObject);
		scoreKeeper.Score (scoreValue);
		AudioSource.PlayClipAtPoint (deadSound, transform.position);
	}

}
